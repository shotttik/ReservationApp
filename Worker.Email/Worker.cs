using Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog.Context;
using Shared.RabbitMq;
using System.Text;
using System.Text.Json;

namespace Worker.Email
{
    public class EmailWorker :BackgroundService
    {
        private readonly RabbitMQSettings _settings;
        private readonly string _queue;
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailWorker> _logger;

        public EmailWorker(
            IOptions<RabbitMQSettings> options,
            IEmailService emailService,
            ILogger<EmailWorker> logger)
        {
            _settings = options.Value;
            _queue = _settings.Queue(QueueType.Email);
            _emailService = emailService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailWorker starting. Queue: {QueueName}, Host: {Host}, Port: {Port}",
                _queue, _settings.Host, _settings.Port);

            // simple retry loop to avoid crashing the host on connection errors
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory
                    {
                        HostName = _settings.Host,
                        Port = _settings.Port,
                        UserName = _settings.Username,
                        Password = _settings.Password,
                        VirtualHost = _settings.VirtualHost
                    };

                    await using var connection = await factory.CreateConnectionAsync(cancellationToken);
                    await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

                    await channel.QueueDeclareAsync(
                        queue: _queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null,
                        cancellationToken: cancellationToken);

                    await channel.BasicQosAsync(
                        prefetchSize: 0,
                        prefetchCount: 1,
                        global: false,
                        cancellationToken: cancellationToken);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        var correlationId = Guid.NewGuid().ToString();
                        using (LogContext.PushProperty("CorrelationId", correlationId))
                        {

                            try
                            {
                                byte [] body = ea.Body.ToArray();
                                var json = Encoding.UTF8.GetString(body);

                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                };

                                var emailMessage = JsonSerializer.Deserialize<EmailMessage>(json, options);
                                if (emailMessage is null)
                                {
                                    _logger.LogError(
                                        "Failed to deserialize message to {TypeName}. Raw message: {RawMessage}",
                                        nameof(EmailMessage),
                                        json);

                                    await channel.BasicNackAsync(
                                        deliveryTag: ea.DeliveryTag,
                                        multiple: false,
                                        requeue: false,
                                        cancellationToken: cancellationToken);

                                    return;
                                }

                                _logger.LogInformation(
                                    "Received EmailMessage for {ToEmail}.",
                                    emailMessage.ToEmail
                                    );

                                await _emailService.SendEmail(emailMessage);

                                _logger.LogInformation(
                                    "Successfully processed EmailMessage for {ToEmail}.",
                                    emailMessage.ToEmail);

                                await channel.BasicAckAsync(
                                    deliveryTag: ea.DeliveryTag,
                                    multiple: false,
                                    cancellationToken: cancellationToken);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex,
                                    "Error while processing message from queue {QueueName}. Nacking message.",
                                    _queue);

                                await channel.BasicNackAsync(
                                    deliveryTag: ea.DeliveryTag,
                                    multiple: false,
                                    requeue: false,
                                    cancellationToken: cancellationToken);
                            }
                        }
                        ;
                    };
                    await channel.BasicConsumeAsync(
                        _queue,
                        autoAck: false,
                        consumer: consumer,
                        cancellationToken: cancellationToken);

                    // Wait here until cancellation is requested
                    _logger.LogInformation("EmailWorker is now consuming messages from {QueueName}.", _queue);

                    await Task.Delay(Timeout.Infinite, cancellationToken);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("EmailWorker cancellation requested. Stopping.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Unhandled exception in EmailWorker. Will retry connection in {DelaySeconds} seconds.",
                        10);

                    // avoid tight retry loop on connection failures (like RabbitMQ down)
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                        _logger.LogInformation("EmailWorker stopping during backoff delay.");
                        break;
                    }
                }
            }

            _logger.LogInformation("EmailWorker stopped.");
        }
    }
}

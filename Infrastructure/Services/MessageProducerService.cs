using Infrastructure.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.RabbitMq;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class MessageProducerService :IMessageProducerService, IAsyncDisposable
    {
        private readonly RabbitMQSettings _settings;
        private readonly ConnectionFactory _factory;

        private IConnection? _connection;
        private IChannel? _channel;
        private bool _initialized;
        private readonly SemaphoreSlim _initLock = new(1, 1);

        public MessageProducerService(IOptions<RabbitMQSettings> options)
        {
            _settings = options.Value;

            _factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };
        }

        // Public API
        public Task PublishEmailAsync(object payload, CancellationToken cancellationToken = default)
            => PublishAsync(payload, routingKey: _settings.RouteKey(QueueType.Email), cancellationToken);

        public Task PublishSmsAsync(object payload, CancellationToken cancellationToken = default)
            => PublishAsync(payload, routingKey: _settings.RouteKey(QueueType.SMS), cancellationToken);

        // Core publish method
        private async Task PublishAsync(object payload, string routingKey, CancellationToken cancellationToken)
        {
            await EnsureInitializedAsync(cancellationToken);

            var json = JsonSerializer.Serialize(payload);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties
            {
                Persistent = true // mark message as persistent
            };

            // _channel is guaranteed after EnsureInitializedAsync
            await _channel!.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: props,
                body: body,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Lazily create connection + channel once, using async CreateConnectionAsync.
        /// Safe for singleton usage (thread-safe).
        /// </summary>
        private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
        {
            if (_initialized) return;

            await _initLock.WaitAsync(cancellationToken);
            try
            {
                if (_initialized) return;

                _connection = await _factory.CreateConnectionAsync(cancellationToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

                // Declare direct exchange once
                await _channel.ExchangeDeclareAsync(
                    exchange: _settings.ExchangeName,
                    type: ExchangeType.Direct,
                    durable: true,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                // Declare queues + bindings once (email + sms)
                await _channel.QueueDeclareAsync(
                    queue: _settings.Queue(QueueType.Email),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                await _channel.QueueDeclareAsync(
                    queue: _settings.Queue(QueueType.SMS),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                await _channel.QueueBindAsync(
                    queue: _settings.Queue(QueueType.Email),
                    exchange: _settings.ExchangeName,
                    routingKey: _settings.RouteKey(QueueType.Email),
                    arguments: null,
                    cancellationToken: cancellationToken);

                await _channel.QueueBindAsync(
                    queue: _settings.Queue(QueueType.SMS),
                    exchange: _settings.ExchangeName,
                    routingKey: _settings.RouteKey(QueueType.SMS),
                    arguments: null,
                    cancellationToken: cancellationToken);

                _initialized = true;
            }
            finally
            {
                _initLock.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
            {
                await _channel.DisposeAsync();
            }

            _connection?.Dispose();
            _initLock.Dispose();
        }
    }
}

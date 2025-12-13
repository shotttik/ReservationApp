using Application.Options;
using Domain.Interfaces.Services;
using Infrastructure.Services;
using Serilog;
using Shared.RabbitMq;
using Worker.Email;

var builder = Host.CreateApplicationBuilder(args);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.AddSerilog(Log.Logger);
// configuration
var configuration = builder.Configuration;
builder.Services.Configure<RabbitMQSettings>(
    configuration.GetSection("RabbitMQ"));
builder.Services.Configure<SmtpSettings>(
    configuration.GetSection("SmtpSettings"));
// services
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddHostedService<EmailWorker>();

var host = builder.Build();
host.Run();
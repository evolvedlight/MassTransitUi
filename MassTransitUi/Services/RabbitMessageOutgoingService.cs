using MassTransitUi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MassTransitUi.Services
{
    public class RabbitMessageOutgoingService
    {
        private readonly MassTransitSettings _settings;
        private readonly IConnection _conn;
        private readonly IModel _channel;

        public RabbitMessageOutgoingService(ILogger<RabbitMessageOutgoingService> logger, IOptions<MassTransitSettings> settings)
        {
            _settings = settings.Value;
            var factory = new ConnectionFactory {
                UserName = _settings.UserName,
                Password = _settings.Password,
                HostName = _settings.HostName,
                VirtualHost = _settings.VirtualHost
            };

            _conn = factory.CreateConnection();

            _channel = _conn.CreateModel();
        }

        public void SendMessage(FailedMessage failedMessage)
        {
            var props = _channel.CreateBasicProperties();
            props.ContentType = "text/test";
            props.DeliveryMode = 2;
            _channel.BasicPublish("", failedMessage.Queue, basicProperties: props, failedMessage.Content);
        }
    }
}

using System;
using System.Linq;
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
        private readonly ILogger<RabbitMessageOutgoingService> _logger;

        public RabbitMessageOutgoingService(ILogger<RabbitMessageOutgoingService> logger, IOptions<MassTransitSettings> settings)
        {
            _logger = logger;

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

            var deliveredFailedMessageProperties = System.Text.Json.JsonSerializer.Deserialize<BasicPropertiesHolder>(failedMessage.Properties);

            props.AppId = deliveredFailedMessageProperties.AppId;
            props.ClusterId = deliveredFailedMessageProperties.ClusterId;
            props.ContentEncoding = deliveredFailedMessageProperties.ContentEncoding;
            props.ContentType = deliveredFailedMessageProperties.ContentType;
            props.CorrelationId = deliveredFailedMessageProperties.CorrelationId;
            props.DeliveryMode = deliveredFailedMessageProperties.DeliveryMode;
            props.Expiration = deliveredFailedMessageProperties.Expiration;
            props.Headers = deliveredFailedMessageProperties.Headers.ToDictionary(x => x.Key, x => (object)Convert.FromBase64String(x.Value.ToString()));
            props.MessageId = deliveredFailedMessageProperties.MessageId;
            props.Persistent = deliveredFailedMessageProperties.Persistent;
            props.Priority = deliveredFailedMessageProperties.Priority;
            props.ReplyTo = deliveredFailedMessageProperties.ReplyTo;
            if (deliveredFailedMessageProperties.ReplyToAddress != null) props.ReplyToAddress = deliveredFailedMessageProperties.ReplyToAddress;
            props.Timestamp = deliveredFailedMessageProperties.Timestamp;
            props.Type = deliveredFailedMessageProperties.Type;
            props.AppId = deliveredFailedMessageProperties.UserId;

            var queue = failedMessage.Queue.Replace("_error", "");

            _channel.BasicPublish("", queue, basicProperties: props, failedMessage.Content);

            _logger.LogInformation("Retried {MessageId} into {queue}", props.MessageId, queue);
        }
    }
}

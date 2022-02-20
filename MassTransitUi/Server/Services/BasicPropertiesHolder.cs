using System.Collections.Generic;
using RabbitMQ.Client;

namespace MassTransitUi.Server.Services
{
    internal class BasicPropertiesHolder
    {
        public string UserId { get; set; }
        //
        // Summary:
        //     Message timestamp.
        public AmqpTimestamp Timestamp { get; set; }
        //
        // Summary:
        //     Convenience property; parses RabbitMQ.Client.IBasicProperties.ReplyTo property
        //     using RabbitMQ.Client.PublicationAddress.TryParse(System.String,RabbitMQ.Client.PublicationAddress@),
        //     and serializes it using RabbitMQ.Client.PublicationAddress.ToString. Returns
        //     null if RabbitMQ.Client.IBasicProperties.ReplyTo property cannot be parsed by
        //     RabbitMQ.Client.PublicationAddress.TryParse(System.String,RabbitMQ.Client.PublicationAddress@).
        public PublicationAddress ReplyToAddress { get; set; }
        //
        // Summary:
        //     Destination to reply to.
        public string ReplyTo { get; set; }
        //
        // Summary:
        //     Message priority, 0 to 9.
        public byte Priority { get; set; }
        //
        // Summary:
        //     Sets RabbitMQ.Client.IBasicProperties.DeliveryMode to either persistent (2) or
        //     non-persistent (1).
        public bool Persistent { get; set; }
        //
        // Summary:
        //     Application message Id.
        public string MessageId { get; set; }
        //
        // Summary:
        //     Message header field table. Is of type System.Collections.Generic.IDictionary`2.
        public IDictionary<string, object> Headers { get; set; }
        //
        // Summary:
        //     Message expiration specification.
        public string Expiration { get; set; }
        //
        // Summary:
        //     Non-persistent (1) or persistent (2).
        public byte DeliveryMode { get; set; }
        //
        // Summary:
        //     Application correlation identifier.
        public string CorrelationId { get; set; }
        //
        // Summary:
        //     MIME content type.
        public string ContentType { get; set; }
        //
        // Summary:
        //     MIME content encoding.
        public string ContentEncoding { get; set; }
        //
        // Summary:
        //     Intra-cluster routing identifier (cluster id is deprecated in AMQP 0-9-1).
        public string ClusterId { get; set; }
        //
        // Summary:
        //     Application Id.
        public string AppId { get; set; }
        //
        // Summary:
        //     Message type name.
        public string Type { get; set; }
    }
}
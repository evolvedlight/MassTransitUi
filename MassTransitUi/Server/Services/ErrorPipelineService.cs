using MassTransit;
using MassTransit.Courier;
using MassTransit.Serialization;
using MassTransitUi.Server.Data;
using MassTransitUi.Server.Hubs;
using MassTransitUi.Shared;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace MassTransitUi.Server.Services
{
    public class ErrorPipelineService : IErrorPipelineService
    {
        private readonly IServiceProvider _sp;

        public ErrorPipelineService(IServiceProvider sp)
        {
            _sp = sp;
        }

        public async Task Process(string queueName, BasicDeliverEventArgs ea)
        {
            
            if (ea.BasicProperties.ContentType == "application/vnd.masstransit+json")
            {
                var body = ea.Body;
                var reader = new StreamReader(new MemoryStream(body.ToArray()));
                var jsonReader = new JsonTextReader(reader);

                var res = SerializerCache.Deserializer.Deserialize<MessageEnvelope>(jsonReader);

                var headers = ea.BasicProperties.Headers.ToDictionary(x => x.Key, x => System.Text.Encoding.UTF8.GetString((byte[])x.Value));

                var failedMessage = new FailedMessage() {
                    MessageId = res.MessageId,
                    Queue = queueName,
                    ErrorMessage = headers[MessageHeaders.FaultMessage],
                    ReceivedTsUtc = DateTime.UtcNow,
                    Headers = headers?.Select((item) => new FailedMessageHeader { Key = item.Key, Value = item.Value.ToString() })?.ToList(),
                    Content = body.ToArray(),
                    Properties = System.Text.Json.JsonSerializer.Serialize(ea.BasicProperties)
                };

                using (var scope = _sp.CreateScope())
                {
                    var massTransitUiContext = scope.ServiceProvider.GetService<MassTransitUiContext>();
                    var hubContext = scope.ServiceProvider.GetService<IHubContext<ErrorQueueHub>>();
                    massTransitUiContext.Add(failedMessage);
                    await massTransitUiContext.SaveChangesAsync();

                    await hubContext.Clients.All.SendAsync("NewErrorInQueue", failedMessage);
                }
            }
            else
            {
                // TODO: put in some final error queue
            }
        }
    }
}

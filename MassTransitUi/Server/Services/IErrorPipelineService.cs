using RabbitMQ.Client.Events;

namespace MassTransitUi.Server.Services
{
    public interface IErrorPipelineService
    {
        Task Process(string queueName, BasicDeliverEventArgs ea);
    }
}
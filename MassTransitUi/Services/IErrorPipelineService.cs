using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace MassTransitUi.Services
{
    public interface IErrorPipelineService
    {
        Task Process(string queueName, BasicDeliverEventArgs ea);
    }
}
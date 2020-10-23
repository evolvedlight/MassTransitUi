using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransitUi.Services
{
    public class RabbitErrorQueuesMonitor : IHostedService, IDisposable
    {
        private IConnection _conn;
        private IModel _channel;
        private readonly IErrorPipelineService _errorPipelineService;
        private readonly MassTransitSettings _settings;

        public RabbitErrorQueuesMonitor(IErrorPipelineService errorPipelineService, IOptions<MassTransitSettings> settings)
        {
            _errorPipelineService = errorPipelineService;
            _settings = settings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ConnectionFactory factory = new ConnectionFactory();

            factory.UserName = _settings.UserName;
            factory.Password = _settings.Password;
            factory.HostName = _settings.HostName;
            factory.VirtualHost = _settings.VirtualHost;

            _conn = factory.CreateConnection();

            _channel = _conn.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                await _errorPipelineService.Process("test_queue_error", ea);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            var consumerTag = _channel.BasicConsume("test_queue_error", false, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
            }

            if (_conn != null && _conn.IsOpen)
            {
                _conn.Close();
            }
            return Task.CompletedTask;
        }

        private bool disposedValue;
        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_channel != null && _channel.IsOpen)
                    {
                        _channel.Close();
                    }

                    if (_conn != null && _conn.IsOpen)
                    {
                        _conn.Close();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RabbitErrorQueuesMonitor()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        
    }
}

﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransitUi.Server.Services
{
    public class RabbitErrorQueuesMonitor : IHostedService, IDisposable
    {
        private IConnection _conn;
        private readonly Dictionary<string, IModel> _channels;
        private readonly ILogger<RabbitErrorQueuesMonitor> _logger;
        private readonly IErrorPipelineService _errorPipelineService;
        private readonly IManagementApiService _managementApiService;
        private readonly MassTransitSettings _settings;

        public RabbitErrorQueuesMonitor(ILogger<RabbitErrorQueuesMonitor> logger, IErrorPipelineService errorPipelineService, IOptions<MassTransitSettings> settings, IManagementApiService managementApiService)
        {
            _logger = logger;
            _errorPipelineService = errorPipelineService;
            _managementApiService = managementApiService;
            _settings = settings.Value;
            _channels = new Dictionary<string, IModel>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Connecting to {_settings.HostName}:{_settings.VirtualHost}");
            var factory = new ConnectionFactory {
                UserName = _settings.UserName,
                Password = _settings.Password,
                HostName = _settings.HostName,
                VirtualHost = _settings.VirtualHost
            };

            _conn = factory.CreateConnection();

            foreach (var queueName in (await _managementApiService.GetQueues()).Where(q => q.EndsWith("_error")))
            {
                var channel = _conn.CreateModel();

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (ch, ea) =>
                {
                    await _errorPipelineService.Process(queueName, ea);

                    channel.BasicAck(ea.DeliveryTag, false);
                };
                _channels[queueName] = channel;

                var consumerTag = channel.BasicConsume(queueName, false, consumer);     
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach ((var _, var channel) in _channels)
            {
                if (channel != null && channel.IsOpen)
                {
                    channel.Close();
                }
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
                    foreach ((var _, var channel) in _channels)
                    {
                        if (channel != null && channel.IsOpen)
                        {
                            channel.Close();
                        }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HareDu.Core.Configuration;
using HareDu.Registration;
using Microsoft.Extensions.Options;

namespace MassTransitUi.Services
{
    public class ManagementApiService : IManagementApiService
    {
        private readonly MassTransitSettings _settings;
        private readonly HareDuConfig _config;

        public ManagementApiService(IOptions<MassTransitSettings> settings)
        {
            _settings = settings.Value;

            var provider = new HareDuConfigProvider();
            _config = provider.Configure(c =>
            {
                c.Broker(b =>
                {
                    b.ConnectTo(_settings.ManagementEndpoint);
                    b.UsingCredentials(_settings.UserName, _settings.Password);
                    b.TimeoutAfter(TimeSpan.FromSeconds(5));
                });
                c.Diagnostics(y =>
                {
                    y.Probes(z =>
                    {
                        z.SetMessageRedeliveryThresholdCoefficient(0.60M);
                        z.SetSocketUsageThresholdCoefficient(0.60M);
                        z.SetConsumerUtilizationThreshold(0.65M);
                        z.SetQueueHighFlowThreshold(90);
                        z.SetQueueLowFlowThreshold(10);
                        z.SetRuntimeProcessUsageThresholdCoefficient(0.65M);
                        z.SetFileDescriptorUsageThresholdCoefficient(0.65M);
                        z.SetHighConnectionClosureRateThreshold(90);
                        z.SetHighConnectionCreationRateThreshold(60);
                    });
                });
            });
        }

        public async Task<IEnumerable<string>> GetQueues()
        {
            var factory = new BrokerObjectFactory(_config);
            var obj = factory.Object<HareDu.Queue>();
            var result = await obj.GetAll();

            return result.Data.Select(x => x.Name);
        }
    }
}

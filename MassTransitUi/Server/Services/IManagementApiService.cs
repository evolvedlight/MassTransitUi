using System.Collections.Generic;
using System.Threading.Tasks;

namespace MassTransitUi.Server.Services
{
    public interface IManagementApiService
    {
        Task<IEnumerable<string>> GetQueues();
    }
}
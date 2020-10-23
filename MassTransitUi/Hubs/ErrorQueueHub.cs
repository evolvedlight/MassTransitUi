using MassTransitUi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MassTransitUi.Hubs
{
    public class ErrorQueueHub : Hub
    {
        private readonly MassTransitUiContext _dbContext;

        public ErrorQueueHub(MassTransitUiContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async override Task OnConnectedAsync()
        {
            var failedMessages = await _dbContext.FailedMessages.ToListAsync();

            foreach (var failedMessage in failedMessages)
            {
                await Clients.Caller.SendAsync("NewErrorInQueue", System.Text.Json.JsonSerializer.Serialize(failedMessage));
            }
        }
    }
}

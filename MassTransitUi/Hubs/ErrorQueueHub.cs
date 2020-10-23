using MassTransitUi.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MassTransitUi.Hubs
{
    public class ErrorQueueHub : Hub
    {
        private readonly MassTransitUiContext _dbContext;
        private readonly IHubContext<ErrorQueueHub> _hubContext;

        public ErrorQueueHub(MassTransitUiContext dbContext, IHubContext<ErrorQueueHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        public async override Task OnConnectedAsync()
        {
            var existing = await _dbContext.FailedMessages.ToListAsync();

            foreach (var thing in existing)
            {
                await _hubContext.Clients.All.SendAsync("NewErrorInQueue", System.Text.Json.JsonSerializer.Serialize(thing));
            }
        }
    }
}

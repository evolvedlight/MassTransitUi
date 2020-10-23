using System.Threading.Tasks;
using MassTransitUi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MassTransitUi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly IHubContext<ErrorQueueHub> hubContext;

        public SampleController(IHubContext<ErrorQueueHub> hubContext, ILogger<SampleController> logger)
        {
            this.hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await hubContext.Clients.All.SendAsync("NewErrorInQueue", 1, 2);

            return Ok();
        }
    }
}

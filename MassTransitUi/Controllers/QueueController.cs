using System.Threading.Tasks;
using MassTransitUi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MassTransitUi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly ILogger<QueueController> _logger;
        private readonly IManagementApiService _managementApiService;

        public QueueController(ILogger<QueueController> logger, IManagementApiService managementApiService)
        {
            _logger = logger;
            _managementApiService = managementApiService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetQueues()
        {
            return Ok(await _managementApiService.GetQueues());
        }
    }
}

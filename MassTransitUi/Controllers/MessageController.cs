using System.Threading.Tasks;
using MassTransitUi.Models;
using MassTransitUi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MassTransitUi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MassTransitUiContext _dbContext;
        private readonly MassTransitSettings _settings;
        private readonly RabbitMessageOutgoingService _rabbit;

        public MessageController(MassTransitUiContext dbContext, IOptions<MassTransitSettings> settings, RabbitMessageOutgoingService rabbit)
        {
            _dbContext = dbContext;
            _settings = settings.Value;
            _rabbit = rabbit;
        }

        [HttpPost("{messageId}/retry")]
        public async Task<IActionResult> RetryMessage(long messageId)
        {
            var record = await _dbContext
                .FailedMessages
                .Include(m => m.Headers)
                .SingleOrDefaultAsync(m => m.Id == messageId);

            if (record == null)
            {
                return BadRequest("No message found with this ID");
            }

            // Resend message
            _rabbit.SendMessage(record);

            _dbContext.FailedMessages.Remove(record);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{messageId}/delete")]
        public async Task<IActionResult> DeleteMessage(long messageId)
        {
            var record = await _dbContext
                .FailedMessages
                .Include(m => m.Headers)
                .SingleOrDefaultAsync(m => m.Id == messageId);

            if (record == null)
            {
                return BadRequest("No message found with this ID");
            }

            _dbContext.FailedMessages.Remove(record);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}

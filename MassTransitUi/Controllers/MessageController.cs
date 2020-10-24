using System.Threading.Tasks;
using MassTransitUi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace MassTransitUi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MassTransitUiContext _dbContext;
        private readonly MassTransitSettings _settings;

        public MessageController(MassTransitUiContext dbContext, IOptions<MassTransitSettings> settings)
        {
            _dbContext = dbContext;
            _settings = settings.Value;
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
            var factory = new ConnectionFactory();

            factory.UserName = _settings.UserName;
            factory.Password = _settings.Password;
            factory.HostName = _settings.HostName;
            factory.VirtualHost = _settings.VirtualHost;

            var _conn = factory.CreateConnection();

            var _channel = _conn.CreateModel();
            IBasicProperties props = _channel.CreateBasicProperties();
            props.ContentType = "text/test";
            props.DeliveryMode = 2;
            _channel.BasicPublish("", "test_queue", basicProperties: props, record.Content);

            //_dbContext.FailedMessageHeaders.RemoveRange(record.Headers)
            _dbContext.FailedMessages.Remove(record);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}

using System.Threading.Tasks;
using MassTransitUi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace MassTransitUi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MassTransitUiContext _dbContext;

        public MessageController(MassTransitUiContext dbContext)
        {
            _dbContext = dbContext;
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
            ConnectionFactory factory = new ConnectionFactory();
            // "guest"/"guest" by default, limited to localhost connections
            factory.UserName = "nzyevrow";
            factory.Password = "6bjmHqKxdNyMTL48RqTgWXH90cgXJTF6";
            factory.HostName = "sparrow.rmq.cloudamqp.com";
            factory.VirtualHost = "nzyevrow";

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

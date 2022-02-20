
using MassTransitUi.Server.Data;
using MassTransitUi.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MassTransitUi.Blazor.Server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MessageController : ControllerBase
  {
    private readonly MassTransitUiContext _dbContext;
    private readonly RabbitMessageOutgoingService _rabbit;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ILogger<MessageController> logger, MassTransitUiContext dbContext, RabbitMessageOutgoingService rabbit)
    {
      _dbContext = dbContext;
      _rabbit = rabbit;
      _logger = logger;
    }

    [HttpGet("{messageId}")]
    public async Task<IActionResult> GetMessageById(long messageId) {
      var record = await _dbContext
          .FailedMessages
          .Include(m => m.Headers)
          .SingleOrDefaultAsync(m => m.Id == messageId);

      if (record == null)
      {
        return BadRequest("No message found with this ID");
      }

      return Ok(record);
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

      _logger.LogInformation($"Retrying message {messageId}");

      return Ok();
    }

    [HttpPost("retry-all")]
    public async Task<IActionResult> RetryAll()
    {
      var records = await _dbContext
          .FailedMessages
          .Include(m => m.Headers)
          .ToListAsync();

      // Resend message
      foreach (var record in records) {
        _logger.LogInformation($"Retrying message {record.MessageId}");
        _rabbit.SendMessage(record);
        _dbContext.FailedMessages.Remove(record);
        await _dbContext.SaveChangesAsync();
      }
      
      return Ok();
    }

    [HttpDelete("{messageId}")]
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

    [HttpDelete("delete-all")]
    public async Task<IActionResult> DeleteAllMessages()
    {
      await _dbContext.Database.ExecuteSqlRawAsync("delete from FailedMessages");

      return Ok();
    }
  }
}

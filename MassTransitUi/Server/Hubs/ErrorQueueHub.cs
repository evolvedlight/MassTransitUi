﻿using MassTransitUi.Server.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MassTransitUi.Server.Hubs
{
  public class ErrorQueueHub : Hub
  {
    private readonly MassTransitUiContext _dbContext;

    public ErrorQueueHub(MassTransitUiContext dbContext)
    {
       _dbContext = dbContext;
    }

    public override async Task OnConnectedAsync()
    {
      var failedMessages = await _dbContext.FailedMessages.ToListAsync();

      foreach (var failedMessage in failedMessages)
      {
        await Clients.Caller.SendAsync("NewErrorInQueue", failedMessage);
      }
    }
  }
}

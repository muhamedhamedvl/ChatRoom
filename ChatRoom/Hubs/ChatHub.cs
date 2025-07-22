using System;
using System.Threading.Tasks;
using ChatRoom.Models;
using ChatRoom.Models.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class ChatHub : Hub
{
    private readonly ChatRoomContext _dbContext;

    public ChatHub(ChatRoomContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SendMessage(string username, string message)
    {
        var chatMessage = new ChatMessage
        {
            Username = username,
            Message = message,
            SentAt = DateTime.UtcNow
        };

        _dbContext.ChatMessages.Add(chatMessage);
        await _dbContext.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", username, message, chatMessage.SentAt.ToString("HH:mm"));
    }

    public override async Task OnConnectedAsync()
    {
        var recentMessages = await _dbContext.ChatMessages
            .OrderByDescending(m => m.SentAt)
            .Take(20)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        foreach (var msg in recentMessages)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", msg.Username, msg.Message, msg.SentAt.ToString("HH:mm"));
        }

        await base.OnConnectedAsync();
    }
}

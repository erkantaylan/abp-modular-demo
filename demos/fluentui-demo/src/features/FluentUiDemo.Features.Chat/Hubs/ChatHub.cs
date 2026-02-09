using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentUiDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FluentUiDemo.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task SendMessage(string message)
    {
        var userName = Context.User?.Identity?.Name ?? "Anonymous";
        _chatService.SendMessage(userName, message);
        await Task.CompletedTask;
    }

    public List<ChatMessage> GetMessageHistory()
    {
        return _chatService.GetMessageHistory();
    }

    public List<string> GetOnlineUsers()
    {
        return _chatService.GetOnlineUsers();
    }

    public override async Task OnConnectedAsync()
    {
        var userName = Context.User?.Identity?.Name ?? "Anonymous";
        _chatService.AddUser(Context.ConnectionId, userName);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _chatService.RemoveUser(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}

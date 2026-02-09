using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FluentUiDemo.Services;

public class ChatMessage
{
    public string User { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class ChatService
{
    private readonly List<ChatMessage> _messages = new();
    private readonly ConcurrentDictionary<string, string> _onlineUsers = new();
    private readonly object _messageLock = new();

    public event Action<ChatMessage>? MessageReceived;
    public event Action<string>? UserJoined;
    public event Action<string>? UserLeft;
    public event Action<List<string>>? OnlineUsersChanged;

    public void SendMessage(string user, string message)
    {
        var chatMessage = new ChatMessage
        {
            User = user,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        lock (_messageLock)
        {
            _messages.Add(chatMessage);
        }

        MessageReceived?.Invoke(chatMessage);
    }

    public List<ChatMessage> GetMessageHistory()
    {
        lock (_messageLock)
        {
            return _messages.ToList();
        }
    }

    public void AddUser(string connectionId, string userName)
    {
        _onlineUsers.TryAdd(connectionId, userName);
        UserJoined?.Invoke(userName);
        OnlineUsersChanged?.Invoke(GetOnlineUsers());
    }

    public void RemoveUser(string connectionId)
    {
        if (_onlineUsers.TryRemove(connectionId, out var userName))
        {
            UserLeft?.Invoke(userName);
            OnlineUsersChanged?.Invoke(GetOnlineUsers());
        }
    }

    public List<string> GetOnlineUsers()
    {
        return _onlineUsers.Values.Distinct().OrderBy(u => u).ToList();
    }
}

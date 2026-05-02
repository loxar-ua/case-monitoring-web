using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class ChatHub : Hub
{
    // Використовуємо ConcurrentQueue для безпечного доступу з різних потоків
    private static readonly ConcurrentQueue<(string EventId, string User, string Message)> _messages = new();

    public async Task SendMessage(string eventId, string user, string message)
    {
        var msg = (eventId, user, message);
        _messages.Enqueue(msg);

        // Відправляємо всім у групі eventId, включаючи відправника
        await Clients.Group(eventId).SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinEvent(string eventId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, eventId);

        // Відправляємо історію саме цьому користувачу (Caller)
        var history = _messages.Where(m => m.EventId == eventId).ToList();
        foreach (var msg in history)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", msg.User, msg.Message);
        }
    }
}
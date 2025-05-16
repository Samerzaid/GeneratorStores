using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace GeneratorStores.DataAccess.Repositories;

public class ChatHub : Hub
{
    public async Task SendMessage(string senderId, string message)
    {
        string groupName = "admin"; // Single group for all users to the admin
        await Clients.Group(groupName).SendAsync("ReceiveMessage", senderId, message, DateTime.UtcNow);
    }

    public async Task JoinAdminGroup()
    {
        string groupName = "admin";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveAdminGroup()
    {
        string groupName = "admin";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
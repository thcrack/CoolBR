using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CoolBR.Services.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            if (message.Length == 0) return;
            Console.WriteLine($"User: {user}, Message: {message}");
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
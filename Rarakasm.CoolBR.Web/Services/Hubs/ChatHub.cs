using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Rarakasm.CoolBR.Web.Models;

namespace Rarakasm.CoolBR.Web.Services.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatHistoryService _chatHistoryService;
        public ChatHub(ChatHistoryService chatHistoryService)
        {
            _chatHistoryService = chatHistoryService;
        }

        public async Task SendMessage(string user, string message)
        {
            if (message.Length == 0) return;
            Console.WriteLine($"User: {user}, Message: {message}");
            _chatHistoryService.Add(new ChatMessage(DateTime.Now, user, message));
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GetHistoryMessages()
        {
            await Clients.Caller.SendAsync("ReceiveMessages", 
                _chatHistoryService.GetMessages());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Rarakasm.CoolBR.Web.Models;

namespace Rarakasm.CoolBR.Web.Services
{
    public class ChatHistoryService
    {
        private readonly ILogger<ChatHistoryService> _logger;
        
        private readonly List<ChatMessage> _history = new List<ChatMessage>();
        
        public ChatHistoryService(ILogger<ChatHistoryService> logger)
        {
            _logger = logger;
        }

        public void Add(ChatMessage message)
        {
            _logger.LogInformation(
                "Service received message: " +
                $"[{message.time}] {message.user}: {message.content}");
            _history.Add(message);
        }

        public IEnumerable<ChatMessage> GetMessages(int maxCount = 10)
        {
            // Get the last {maxCount} elements of the history
            return _history.Skip(Math.Max(0, _history.Count() - maxCount));
        }
    }
}
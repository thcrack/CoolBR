using System;
using System.Text.Json;

namespace Rarakasm.CoolBR.Web.Models
{
    public class ChatMessage
    {
        public ChatMessage(DateTime time, string user, string content)
        {
            this.time = time;
            this.user = user;
            this.content = content;
        }

        public DateTime time { get; set; }
        public string user { get; set; }
        public string content { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
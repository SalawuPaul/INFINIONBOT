using Azure.AI.OpenAI;
using System;
namespace INFINIONGPT.Models
{
    public class ChatMessages
    {
        public string userMessage { get; set; }
        public string BotMessage { get; set; }
        public DateTime MessageTimeStamp { get; set; } = DateTime.UtcNow;

    }
}

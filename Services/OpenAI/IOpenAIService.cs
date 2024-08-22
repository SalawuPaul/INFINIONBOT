using INFINIONGPT.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INFINIONGPT.Services.OpenAI
{
    public interface IOpenAIService
    {
        public Task<ChatMessages> ChatCompletionAsync(string message, List<ChatMessages> chatMessages);
        Task<ChatMessages> ChatCompletionAsync(string text);
    }
}

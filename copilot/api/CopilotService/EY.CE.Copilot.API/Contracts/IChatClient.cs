using EY.CE.Copilot.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IChatClient
    {
        Task<string> StartChat();
        Task<IActionResult> GetStatus(string token, Chat.Status endpoint, string chatId);
        Task<IActionResult> PostChat(string token, Chat.Input input);
        void SaveResponseToRedis(string key, Chat.Message message, string status, int statusCode);
        Task PostFeedback(Chat.MessageFeedback feedback);
        Task PostCopilotFeedback(Chat.CopilotFeedback feedback);
        Task<FileContentResult> GetExportFile(string chatId, int messageId, string fileType);
        Task<string> GetDataFromCacheOrDatabase(string key);
    }
}

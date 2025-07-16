using EY.CapitalEdge.HelpCopilot.Models;
using EY.CapitalEdge.HelpCopilot.Models.Chat;
using EY.CapitalEdge.HelpCopilot.Models.ChatSensitiveDataSupport;

namespace EY.CapitalEdge.HelpCopilot.Services
{
    public interface IHelpCopilotService
    {
        Task<SessionResponse> SessionAsync(RequestContext context);
        Task<ConversationResponse> StartConversationAsync(RequestContext context);
        Task<ChatResponse> ChatAsync(RequestContext context, ChatInput input);
        Task<ChatResponse> ChatSensitiveDataSupportAsync(RequestContext context, ChatSensitiveDataSupportInput input);
    }
}

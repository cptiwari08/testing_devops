using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.HelpCopilot.Models
{
    [ExcludeFromCodeCoverage]
    public class ConversationResponse
    {
        public required string ConversationReferenceId { get; set; }
        public required string WelcomeTextMessage { get; set; }
    }
}
namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class ConversationResponse
    {
        public required string ConversationReferenceId { get; set; }
        public required string WelcomeTextMessage { get; set; }
    }
}
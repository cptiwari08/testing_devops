namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class BackendInput
    {
        public BackendInput()
        {
            ChatHistory = [];
            Context = [];
        }

        public required Guid ChatId { get; set; }

        public required string InstanceId { get; set; }
        public required string ProjectFriendlyId { get; set; }

        public string? Question { get; set; }

        public List<ChatHistory> ChatHistory { get; set; }

        public Dictionary<string, object> Context { get; set; }
    }
}

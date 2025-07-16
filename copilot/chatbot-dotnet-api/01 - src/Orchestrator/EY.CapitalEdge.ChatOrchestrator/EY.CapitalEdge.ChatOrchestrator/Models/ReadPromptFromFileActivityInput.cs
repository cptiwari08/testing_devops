namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class ReadPromptFromFileActivityInput
    {
        public required string InstanceId { get; set; }
        public required Guid ChatId { get; set; }
        public required string FileName { get; set; }
        public string? ProjectFriendlyId { get; set; }
    }
}

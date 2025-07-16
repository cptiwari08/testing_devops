namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class SummarizeActivityInput
    {
        public required string InstanceId { get; set; }
        public required Guid ChatId { get; set; }
        public required string Task { get; set; }
        public string? ProjectFriendlyId { get; set; }
    }
}

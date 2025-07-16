using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    [ExcludeFromCodeCoverage]
    public class FollowUpSuggestionInput
    {
        public required string InstanceId { get; set; }
        public required Guid ChatId { get; set; }
        public required ChatQuestion Input { get; set; }
        public required List<ChatHistory> ChatHistory { get; set; }
        public string? ProjectFriendlyId { get; set; }
    }
}

using EY.CapitalEdge.ChatOrchestrator.Utils;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class HttpCallActivityInput
    {
        public required string InstanceId { get; set; }
        public required Guid ChatId { get; set; }
        public required SerializableHttpRequestMessage SerializedRequest { get; set; }
        public string? ProjectFriendlyId { get; set; }
    }
}

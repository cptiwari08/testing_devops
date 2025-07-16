using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class ChatHistoryResponse
    {
        [JsonPropertyName("key")]
        public required string Key { get; set; }
        [JsonPropertyName("content")]
        public required List<ChatHistoryService.Message> Content { get; set; }
    }
}

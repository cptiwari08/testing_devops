using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class ProjectContextResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public required string Title { get; set; }
        [JsonPropertyName("key")]
        public required string Key { get; set; }
        [JsonPropertyName("value")]
        public required string Value { get; set; }
        [JsonPropertyName("isEnabled")]
        public required bool IsEnabled { get; set; }
    }
}

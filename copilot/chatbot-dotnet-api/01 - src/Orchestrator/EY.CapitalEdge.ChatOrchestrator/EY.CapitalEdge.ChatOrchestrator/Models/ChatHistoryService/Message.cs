using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models.ChatHistoryService
{
    public class Message
    {
        [JsonPropertyName("messageId")]
        public int MessageId { get; set; }

        [JsonPropertyName("role")]
        public required string Role { get; set; }

        [JsonPropertyName("content")]
        public required string Content { get; set; }

        [JsonPropertyName("sources")]
        public string[]? Sources { get; set; }

        [JsonPropertyName("inputSources")]
        public string[]? InputSources { get; set; }

        [JsonPropertyName("context")]
        public Dictionary<string, object?>? Context { get; set; }

        [JsonPropertyName("response")]
        public List<SourceDto> Response { get; set; } = [];

        [JsonPropertyName("createdTime")]
        public DateTime? CreatedTime { get; set; }

        [JsonPropertyName("lastUpdatedTime")]
        public DateTime? LastUpdatedTime { get; set; }

        [JsonPropertyName("followUpSuggestions")]
        public List<FollowUpSuggestionsResponse>? FollowUpSuggestions { get; set; }
    }

    public class SourceDto
    {
        [JsonPropertyName("sourceName")]
        public required string SourceName { get; set; }
        [JsonPropertyName("content")]
        public string? Content { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("sqlQuery")]
        public object? SqlQuery { get; set; }
        [JsonPropertyName("citingSources")]
        public List<CitingSource>? CitingSources { get; set; }
    }
}


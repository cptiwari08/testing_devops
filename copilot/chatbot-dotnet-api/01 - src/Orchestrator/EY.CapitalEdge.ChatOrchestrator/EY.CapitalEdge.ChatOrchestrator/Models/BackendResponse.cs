using System.Numerics;
using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class BackendResponse
    {
        public BackendResponse()
        {
            CitingSources = [];
            Status = string.Empty;
            FollowUpSuggestions = [];
        }

        public required string Backend { get; set; }

        public required object Response { get; set; }

        public List<CitingSource>? CitingSources { get; set; }

        public List<FollowUpSuggestionsResponse>? FollowUpSuggestions { get; set; }

        public object? RawReponse { get; set; }

        public string Status { get; set; }

        public object? Sql { get; set; }
    }

    public class CitingSource
    {
        [JsonPropertyName("sourceName")]
        public required string SourceName { get; set; }

        [JsonPropertyName("sourceType")]
        public string? SourceType { get; set; }

        [JsonPropertyName("sourceValue")]
        public object? SourceValue { get; set; }
    }
}

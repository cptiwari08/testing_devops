using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    [ExcludeFromCodeCoverage]
    public class FollowUpSuggestionsResponse
    {
        [JsonPropertyName("id")]
        public required int Id { get; set; }
        [JsonPropertyName("suggestionText")]
        public required string SuggestionText { get; set; }
    }
}

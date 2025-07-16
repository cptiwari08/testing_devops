using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    [ExcludeFromCodeCoverage]
    public class Suggestion
    {
        [JsonPropertyName("idSuggestion")]
        public int Id { get; set; }
        [JsonPropertyName("chunk")]
        public required string Chunk { get; set; }
        [JsonPropertyName("source")]
        public required string Source { get; set; }
        [JsonPropertyName("appAffinity")]
        public string? AppAffinity { get; set; }
        [JsonPropertyName("embedding")]
        public required float[] Embedding { get; set; }
        [JsonPropertyName("visibleToAssistant")]
        public bool? VisibleToAssistant { get; set; }
        [JsonPropertyName("isIncluded")]
        public bool? IsIncluded { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class GenericIndex
    {
        [JsonPropertyName("@search.score")]
        public float SearchScore { get; set; }
        [JsonPropertyName("chunk")]
        public required string Chunk { get; set; }
        [JsonPropertyName("embedding")]
        public required float[] Embedding { get; set; }
        [JsonPropertyName("indexType")]
        public required string IndexType { get; set; }
        [JsonPropertyName("metadata")]
        public required Metadata Metadata { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Metadata
    {
        [JsonPropertyName("prjsuggestions")]
        public required PrjSuggestions PrjSuggestions { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PrjSuggestions
    {
        [JsonPropertyName("source")]
        public required string Source { get; set; }
        [JsonPropertyName("appAffinity")]
        public string? AppAffinity { get; set; }
        [JsonPropertyName("idSuggestion")]
        public int Id { get; set; }
        [JsonPropertyName("visibleToAssistant")]
        public bool? VisibleToAssistant { get; set; }
        [JsonPropertyName("isIncluded")]
        public bool? IsIncluded { get; set; }
    }
}

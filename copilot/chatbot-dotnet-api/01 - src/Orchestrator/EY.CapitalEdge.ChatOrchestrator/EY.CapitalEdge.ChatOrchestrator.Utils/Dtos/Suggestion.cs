using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Utils.Dtos
{
    public class Suggestion
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("sqlQuery")]
        public string? SqlQuery { get; set; }
        [JsonPropertyName("source")]
        public string? Source { get; set; }
    }
}

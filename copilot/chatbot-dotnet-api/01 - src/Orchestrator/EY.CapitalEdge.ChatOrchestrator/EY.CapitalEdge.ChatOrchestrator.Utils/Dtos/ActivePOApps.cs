using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Utils.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ActivePOApps
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
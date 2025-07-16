using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Utils.Dtos
{
    [ExcludeFromCodeCoverage]
    public class AppInfo
    {
        public AppInfo()
        {
            TeamTypeIds = [];
        }
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("teamTypeIds")]
        public int[] TeamTypeIds { get; set; }
    }
}

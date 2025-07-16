using Newtonsoft.Json;

namespace EY.CE.Copilot.API.Models
{
    public class DocsRequest
    {
        [JsonProperty("filter")]
        public string? Filter { get; set; }

        [JsonProperty("generatorType")]
        public string GeneratorType { get; set; }

        [JsonProperty("user")]
        public string? User { get; set; }
    }
}

using Newtonsoft.Json;

namespace EY.CE.Copilot.API.Models
{
    public class SourceConfiguration
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("originalDisplayName")]
        public string OriginalDisplayName { get; set; }

        [JsonProperty("isQuestionTextBoxEnabled")]
        public bool IsQuestionTextBoxEnabled { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }

        [JsonProperty("ordinal")]
        public int Ordinal { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
}

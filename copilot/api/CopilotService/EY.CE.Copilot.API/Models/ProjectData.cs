using Newtonsoft.Json;

namespace EY.CE.Copilot.API.Models
{
    public class ProjectData
    {
        [JsonProperty("data")]
        public Details Data { get; set; }
    }

    public class Details
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("areaId")]
        public string AreaId { get; set; }

        [JsonProperty("clientSize")]
        public string? ClientSize { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("countryId")]
        public string CountryId { get; set; }
        
        [JsonProperty("isConfidential")]
        public bool IsConfidential { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("typeId")]
        public string TypeId { get; set; }

        [JsonProperty("sector")]
        public string Sector { get; set; }

        [JsonProperty("sectorId")]
        public string SectorId { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("regionId")]
        public string RegionId { get; set; }

        [JsonProperty("isAssistantEnabled")]
        public bool IsAssistantEnabled { get; set; }
    }
}
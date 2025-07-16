using Newtonsoft.Json;

namespace EY.CE.Copilot.API.Models
{
    public class Data    {
        [JsonProperty("givenName")]
        public string? GivenName { get; set; }

        [JsonProperty("surname")]
        public string? Surname { get; set; }

        [JsonProperty("mail")]
        public required string Mail { get; set; }

        [JsonProperty("userType")]
        public required string UserType { get; set; }

        [JsonProperty("accountType")]
        public required string AccountType { get; set; }

        [JsonProperty("photo")]
        public string? Photo { get; set; }
    }

    public class PortalUser
    {
        [JsonProperty("data")]
        public List<Data>? Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

}

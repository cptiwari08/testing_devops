using Newtonsoft.Json;

namespace EY.CE.Copilot.API.Models.Portal
{
    public class AppPlatform
    {
        [JsonProperty("appPlatformId")]
        public string AppPlatformId { get; set; }

        [JsonProperty("appPlatformName")]
        public string AppPlatformName { get; set; }

        [JsonProperty("apps")]
        public List<string> Apps { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("oldRoles")]
        public List<object> OldRoles { get; set; }

        [JsonProperty("newRoles")]
        public List<object> NewRoles { get; set; }
    }

    public class Data
    {
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("projectDescription")]
        public string ProjectDescription { get; set; }

        [JsonProperty("users")]
        public List<User> Users { get; set; }
    }

    public class UserPlatforms
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class Ssp
    {
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("oldRoles")]
        public object OldRoles { get; set; }

        [JsonProperty("newRoles")]
        public object NewRoles { get; set; }
    }

    public class User
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonProperty("ssp")]
        public Ssp Ssp { get; set; }

        [JsonProperty("action")]
        public int Action { get; set; }

        [JsonProperty("appPlatforms")]
        public List<AppPlatform> AppPlatforms { get; set; }
    }
}

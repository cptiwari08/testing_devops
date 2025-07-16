using Newtonsoft.Json;
using System.Text.Json;

namespace EY.CE.Copilot.API.Models
{
    public class Conversation
    {
        [JsonProperty("key")] 
        public string Key { get; set; }

        [JsonProperty("content")] 
        public List<Chat.Message> Content { get; set; }
    }

    public class OutputConversation
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("content")]
        public JsonDocument Content { get; set; }
    }
}

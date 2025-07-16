using System.Text.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    public class ChatQuestion
    {
        public ChatQuestion()
        {
            Context = [];
        }
        [JsonPropertyName("messageId")]
        public required int MessageId { get; set; }
        [JsonPropertyName("chatId")]
        public required Guid ChatId { get; set; }
        [JsonPropertyName("projectFriendlyId")]
        public required string ProjectFriendlyId { get; set; }
        [JsonPropertyName("question")]
        public required string Question { get; set; }
        [JsonPropertyName("sources")]
        public required string[] Sources { get; set; }
        [JsonPropertyName("inputSources")]
        public required string[] InputSources { get; set; }
        [JsonPropertyName("context")]
        public required Dictionary<string, object?> Context { get; set; }
        [JsonPropertyName("token")]
        public required string Token { get; set; }
    }
}

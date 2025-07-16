using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EY.CapitalEdge.HelpCopilot.Models.Chat
{
    [ExcludeFromCodeCoverage]
    public class ChatInput
    {
        [JsonPropertyName("query")]
        public required string Query { get; set; }
        [JsonPropertyName("use_openai")]
        public required bool UseOpenai { get; set; }
        [JsonPropertyName("conversationReferenceId")]
        public required string ConversationReferenceId { get; set; }
        [JsonPropertyName("responseType")]
        public required string ResponseType { get; set; }
    }
}

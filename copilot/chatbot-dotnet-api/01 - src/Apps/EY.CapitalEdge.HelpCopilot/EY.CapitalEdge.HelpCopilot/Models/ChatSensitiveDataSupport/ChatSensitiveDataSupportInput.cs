using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EY.CapitalEdge.HelpCopilot.Models.ChatSensitiveDataSupport
{
    [ExcludeFromCodeCoverage]
    public class ChatSensitiveDataSupportInput
    {
        [JsonPropertyName("query")]
        public required string Query { get; set; }
        [JsonPropertyName("conversationReferenceId")]
        public required string ConversationReferenceId { get; set; }
        [JsonPropertyName("isSensitiveInfo")]
        public required bool IsSensitiveInfo { get; set; }
        [JsonPropertyName("externalQuestionId")]
        public required string ExternalQuestionId { get; set; }
        [JsonPropertyName("chatHistory")]
        public required ChatHistory[] ChatHistory { get; set; }
        [JsonPropertyName("responseType")]
        public required string ResponseType { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ChatHistory
    {
        public required string Question { get; set; }
        public required AnswerDto[] Answers { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AnswerDto
    {
        public required string Answer { get; set; }
        public bool? IsMessageLiked { get; set; }
        public int? Score { get; set; }
        public DocumentDto[]? Documents { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class DocumentDto
    {
        public required string DocumentName { get; set; }
        public PageDto[]? Pages { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PageDto
    {
        public int PageNumber { get; set; }
        public string? MetaData { get; set; }
    }
}

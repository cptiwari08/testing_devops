using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.HelpCopilot.Models.Chat
{
    [ExcludeFromCodeCoverage]
    public class ChatResponse
    {
        public bool IsSuccess { get; set; }
        public Message[]? Messages { get; set; }
        public int Status { get; set; }
        public int Code { get; set; }
        public string? Message { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Message
    {
        public Guid ConversationGuid { get; set; }
        public int ConversationStatusId { get; set; }
        public Guid MessageGuid { get; set; }
        public int MessageTypeId { get; set; }
        public required string MessageText { get; set; }
        public DateTime MessageCreatedDate { get; set; }
        public User? User { get; set; }
        public object? AdditionalInfo { get; set; }
        public bool? IsMessageLiked { get; set; }
        public double Score { get; set; }
        public bool ShowFeedbackOptions { get; set; }
        public bool ShowTypingEffect { get; set; }
        public Guid ChatResponseGuid { get; set; }
        public int CurrentResponseCount { get; set; }
        public int TotalResponseCount { get; set; }
        public Document[]? Documents { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class User
    {
        public Guid Guid { get; set; }
        public required string DisplayName { get; set; }
        public required string EmailId { get; set; }
        public DateTime CreatedDate { get; set; }
        public object? UserIdentity { get; set; }
        public object? Roles { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Document
    {
        public bool Active { get; set; }
        public Guid DocumentGuid { get; set; }
        public required string DocumentName { get; set; }
        public bool IsVideoDocument { get; set; }
        public Page[]? Pages { get; set; }
        public string? Version { get; set; }
        public string? VideoUrl { get; set; }
        public object? Category { get; set; }
        public object? SubCategory { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Page
    {
        public int PageNumber { get; set; }
        public object? MetaData { get; set; }
    }
}
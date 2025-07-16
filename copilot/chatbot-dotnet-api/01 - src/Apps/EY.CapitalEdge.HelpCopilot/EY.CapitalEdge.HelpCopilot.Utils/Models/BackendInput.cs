using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.HelpCopilot.Utils.Models
{
    [ExcludeFromCodeCoverage]
    public class BackendInput
    {
        public BackendInput()
        {
            ChatHistory = [];
            Context = [];
        }

        public required Guid ChatId { get; set; }

        public required string InstanceId { get; set; }

        public required string ProjectFriendlyId { get; set; }

        public string? Question { get; set; }

        public List<Message> ChatHistory { get; set; }

        public Dictionary<string, object> Context { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Message
    {
        public int MessageId { get; set; }
        public required string Role { get; set; }
        public required string Content { get; set; }
        public Document[]? Documents { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Document
    {
        public Guid? DocumentGuid { get; set; }
        public string? DocumentName { get; set; }
        public Page[]? Pages { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Page
    {
        public int? PageNumber { get; set; }
        public object? MetaData { get; set; }
    }
}

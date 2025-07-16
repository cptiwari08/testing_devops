using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantChatHistory")]
    public class ChatHistory
    {
        public int ID { get; set; }
        public string ChatId { get; set; }
        public int? MessageId { get; set; }
        public string? InstanceId { get; set; }
        public string Message { get; set; }
        public long? SuggestionId { get; set; }
        public string? AdditionalInfo { get; set; }
        public long TimeToResolve { get;set; }
        public string UserRole { get;set; }
        [MaxLength(255)]
        public string UserId { get; set; }
        public string? Status { get; set; }
        public int? StatusCode { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

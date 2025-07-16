using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantMessageFeedbacks")]
    public class MessageFeedback
    {
        public int ID { get; set; }
        public bool? IsLiked { get; set; }
        public string ChatId { get; set; }
        public long MessageId { get; set; }
        public string? InstanceId { get; set; }
        [MaxLength(255)]
        public string UserId { get; set; }
        public string FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool? Consent { get; set; }
    }
}

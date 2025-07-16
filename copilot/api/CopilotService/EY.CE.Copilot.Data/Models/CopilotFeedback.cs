using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantFeedbacks")]
    public class CopilotFeedback
    {
        public int ID { get; set; }
        public int Rating { get; set; }
        [MaxLength(255)]
        public string UserId { get; set; }
        public string FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

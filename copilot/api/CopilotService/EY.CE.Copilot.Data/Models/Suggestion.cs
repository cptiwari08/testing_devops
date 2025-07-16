using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace EY.CE.Copilot.Data.Models
{
    [Table("AssistantSuggestions")]
    public class Suggestion
    {
        public int ID { get; set; }
        public string Source { get; set; }
        public string SuggestionText { get; set; }
        public string AppAffinity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AnswerSQL { get; set; }
        public bool? VisibleToAssistant { get;set; }
        public bool? IsIncluded { get; set; }
    }
}

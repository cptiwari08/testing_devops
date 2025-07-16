using EY.CE.Copilot.API.Static;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EY.CE.Copilot.API.Models
{
    public class SuggestionUpdate
    {
        public int ID { get; set; }
        [SpecialCharNotAllowed()]
        [ScriptNotAllowed()]
        [StringLength(100)]
        public string? Source { get; set; }
        [SpecialCharNotAllowed()]
        [ScriptNotAllowed()]
        [StringLength(255)]
        public string? SuggestionText { get; set; }
        [SpecialCharNotAllowed()]
        [ScriptNotAllowed()]
        [StringLength(100)]
        public string? AppAffinity { get; set; }
        [StringLength(5000)]
        public string? AnswerSQL { get; set; }
        public bool? VisibleToAssistant { get; set; }
        public bool? IsIncluded { get; set; }

    }

    public class SuggestionInsert
    {
        [SpecialCharNotAllowed()]
        [ScriptNotAllowed()]
        [StringLength(100)]
        public string Source { get; set; }
        [SpecialCharNotAllowed()]
        [ScriptNotAllowed()]
        [StringLength(255)]
        public string SuggestionText { get; set; }
        [SpecialCharNotAllowed()]
        [ScriptNotAllowed()]
        [StringLength(100)]
        public string AppAffinity { get; set; }
        [StringLength(5000)]
        public string? AnswerSQL { get; set; }
        public bool? VisibleToAssistant { get; set; }
        public bool? IsIncluded { get; set; }
    }
}

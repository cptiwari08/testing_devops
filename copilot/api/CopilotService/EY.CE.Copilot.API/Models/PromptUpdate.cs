namespace EY.CE.Copilot.API.Models
{
    public class PromptUpdate
    {
        public int? ID { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Key { get; set; }
        public string? Prompt { get; set; }
        public string? OriginalPrompt { get; set; }
        public string? Description { get; set; }
        public string? Agent { get; set; }
        public bool? IsActive { get; set; }
    }
}

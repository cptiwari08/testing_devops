namespace EY.CE.Copilot.API.Models
{
    public class GlossaryUpdate
    {
        public int? ID { get; set; }
        public string? Context { get; set; }
        public string? TableName { get; set; }
        public string? Tag { get; set; }
        public bool? IsSchema { get; set; }
    }
}

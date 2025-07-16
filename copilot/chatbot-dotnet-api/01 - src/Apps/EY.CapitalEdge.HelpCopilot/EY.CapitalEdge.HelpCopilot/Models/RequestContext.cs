namespace EY.CapitalEdge.HelpCopilot.Models
{
    public class RequestContext
    {
        public required string InstanceId { get; set; }
        public required Guid ChatId { get; set; }
        public required string ProjectFriendlyId { get; set; }
        public required string Token { get; set; }
    }
}

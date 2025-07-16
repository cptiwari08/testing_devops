using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.HelpCopilot.Models
{
    [ExcludeFromCodeCoverage]
    public class SessionResponse
    {
        public bool IsSuccess { get; set; }
        public string[]? UserRoles { get; set; }
        public string? Guid { get; set; }
        public string? EmailId { get; set; }
        public string? DisplayName { get; set; }
    }
}

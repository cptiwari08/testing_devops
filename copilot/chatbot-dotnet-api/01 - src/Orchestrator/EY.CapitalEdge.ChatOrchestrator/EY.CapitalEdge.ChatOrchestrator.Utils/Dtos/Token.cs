using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Utils.Dtos
{
    [ExcludeFromCodeCoverage]
    public class Token
    {
        public string? UniqueName { get; set; }
        public string? Email { get; set; }
        public string? FamilyName { get; set; }
        public string? GivenName { get; set; }
        public string? UserType { get; set; }
        public string? Oid { get; set; }
        public string? CeOid { get; set; }
        public string? Upn { get; set; }
        public string? SpUrl { get; set; }
        public string? PoAppUrl { get; set; }
        public string? PoApiUrl { get; set; }
        public string? CopilotAppUrl { get; set; }
        public string? CopilotApiUrl { get; set; }
        public string? ProjectId { get; set; }
        public string? ProjectFriendlyId { get; set; }
        public List<string> Scope { get; set; } = new List<string>();
        public long Nbf { get; set; }
        public long Exp { get; set; }
        public long Iat { get; set; }
        public string? Iss { get; set; }
        public string? Aud { get; set; }
        public string? AiSearchInstanceName { get; set; }
        public string? MetadataIndexName { get; set; }
    }
}

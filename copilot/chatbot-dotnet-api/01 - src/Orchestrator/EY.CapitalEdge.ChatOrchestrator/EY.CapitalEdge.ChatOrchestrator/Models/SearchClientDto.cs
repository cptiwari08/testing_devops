using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    [ExcludeFromCodeCoverage]
    public class SearchClientDto
    {
        public required string ServiceEndpoint { get; set; }
        public required string Credential { get; set; }
        public required string IndexName { get; set; }
    }
}

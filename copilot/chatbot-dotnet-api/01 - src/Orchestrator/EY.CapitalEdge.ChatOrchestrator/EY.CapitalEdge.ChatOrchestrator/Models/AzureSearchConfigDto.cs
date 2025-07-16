using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    [ExcludeFromCodeCoverage]
    public class AzureSearchConfigDto
    {
        public required string Endpoint { get; set; }
        public required string Key { get; set; }
        public required string Version { get; set; }
    }
}

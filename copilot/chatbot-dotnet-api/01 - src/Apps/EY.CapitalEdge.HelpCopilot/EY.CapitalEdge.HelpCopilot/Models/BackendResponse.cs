using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.HelpCopilot.Models
{
    [ExcludeFromCodeCoverage]
    public class BackendResponse
    {
        public BackendResponse()
        {
            Backend = "help-copilot";
            CitingSources = [];
        }

        public string Backend { get; }        

        public required object Response { get; set; }

        public List<CitingSource> CitingSources { get; set; }

        public object? RawReponse { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CitingSource
    {
        public CitingSource()
        {
            SourceName = "help-copilot";
            SourceType = "documents";
        }

        public string SourceName { get; }

        public string SourceType { get; }
        
        public object? SourceValue { get; set; }
    }
}

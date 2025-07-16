using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.HelpCopilot.Services
{
    [ExcludeFromCodeCoverage]
    public class HelpCopilotServiceException : Exception
    {
        public HelpCopilotServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

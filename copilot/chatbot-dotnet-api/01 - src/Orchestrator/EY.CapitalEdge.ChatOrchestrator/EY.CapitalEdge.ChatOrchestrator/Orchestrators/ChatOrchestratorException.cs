using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Orchestrators
{
    [ExcludeFromCodeCoverage]
    public class ChatOrchestratorException : Exception
    {
        public ChatOrchestratorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

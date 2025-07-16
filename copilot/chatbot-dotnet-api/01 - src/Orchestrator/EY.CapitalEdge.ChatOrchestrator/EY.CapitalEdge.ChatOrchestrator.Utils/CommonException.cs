using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Utils
{
    [ExcludeFromCodeCoverage]
    public class CommonException: Exception
    {
        public CommonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

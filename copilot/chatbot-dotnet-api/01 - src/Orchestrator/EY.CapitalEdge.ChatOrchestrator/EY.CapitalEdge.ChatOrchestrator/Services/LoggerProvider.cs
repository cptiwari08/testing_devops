using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    [ExcludeFromCodeCoverage]
    public class LoggerProvider : ILoggerProvider
    {
        public ILogger GetLogger(FunctionContext context, string categoryName)
        {
            return context.GetLogger(categoryName);
        }
    }
}
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface ILoggerProvider
    {
        ILogger GetLogger(FunctionContext context, string categoryName);
    }
}
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;

namespace EY.CE.Copilot.API.Contracts
{
    public interface ILoggerHelperSingleton
    {
        void Log(AppLogLevel logLevel, string message, string className, string methodName, Exception exception = null, string errorCode = null);
    }
}
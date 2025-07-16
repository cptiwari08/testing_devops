using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;

namespace EY.CE.Copilot.API.Common
{
    public class BaseClass
    {
        private IAppLoggerService _logger;
        private readonly string _className;

        public BaseClass(IAppLoggerService loggerService, string className)
        {
            _logger = loggerService;
            _className = className;
        }

        public void Log(AppLogLevel logLevel, string message, string methodName = null, Exception exception = null, string errorCode = null)
        {
            _logger.Log(logLevel, _className, message, methodName, exception, errorCode);
        }
    }
}
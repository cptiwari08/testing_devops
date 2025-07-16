using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using EY.SaT.CapitalEdge.Extensions.Logging.Models;

namespace EY.CE.Copilot.API.Common
{
    public class LoggerHelperSingleton : ILoggerHelperSingleton
    {
        private readonly string workspaceId;
        private IAppLogger _logger;
        public LoggerHelperSingleton(IConfiguration configuration, IAppLogger logger)
        {
            _logger = logger;
            workspaceId = configuration[ConfigMap.WORKSPACE_ID];
        }
        public void Log(AppLogLevel logLevel, string message, string className, string methodName, Exception exception = null, string errorCode = null)
        {
            var appLogContext = new AppLogContext();

            appLogContext.ProjectID = GetGuidFromString(workspaceId);
            appLogContext.AppPlatformName = Constants.ApplicationPlatformName;
            // Assigning empty guid in case correlation id is null or empty.
            appLogContext.CorrelationID = Guid.Empty.ToString();
            appLogContext.ModuleName = Constants.ModuleName;
            _logger.Log(logLevel, appLogContext, className, message, methodName, exception, errorCode);
        }
        private static Guid GetGuidFromString(string guidString)
        {
            Guid guid;
            Guid.TryParse(guidString, out guid);
            return guid;
        }
    }
}
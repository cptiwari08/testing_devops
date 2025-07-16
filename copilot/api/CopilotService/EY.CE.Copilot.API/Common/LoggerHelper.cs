using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Models;

namespace EY.CE.Copilot.API.Common
{
    public static class LoggerHelper
    {
        public static AppLogContext CreateLoggerContext(IHttpContextAccessor httpContextAccessor, IConfiguration Configuration)
        {
            var appLogContext = new AppLogContext();

            appLogContext.ProjectID = GetGuidFromString(Configuration[ConfigMap.WORKSPACE_ID]);
            appLogContext.AppPlatformName = Constants.ApplicationPlatformName;
            // Assigning empty guid in case correlation id is null or empty.
            appLogContext.CorrelationID = Guid.Empty.ToString();
            appLogContext.ModuleName = Constants.ModuleName;

            if (httpContextAccessor != null && httpContextAccessor.HttpContext != null &&
              httpContextAccessor.HttpContext.Request != null && httpContextAccessor.HttpContext.Request.Headers.Any())
            {
                var requestHeaders = httpContextAccessor.HttpContext.Request.Headers;
                var httpContext = httpContextAccessor.HttpContext;
                if (httpContext.Items.TryGetValue(Constants.UserMail, out object? mail))
                    appLogContext.UserID = mail.ToString();
                if (httpContext.Items.TryGetValue(Constants.CorrelationId, out object? correlationId))
                    appLogContext.CorrelationID = correlationId.ToString();
                appLogContext.UserAgent = requestHeaders.FirstOrDefault(a => a.Key.Equals(Constants.UserAgent, StringComparison.InvariantCultureIgnoreCase)).Value;
            }

            return appLogContext;
        }

        public static Guid GetGuidFromString(string guidString)
        {
            Guid guid;
            Guid.TryParse(guidString, out guid);
            return guid;
        }
    }
}
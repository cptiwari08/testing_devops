using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SAT.CE.CoreServices.DependencyInjection;
using EY.SAT.CE.CoreServices.Interfaces;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
namespace EY.CE.Copilot.API.Authorization
{
    public class UserAuthorization(RequestDelegate next,
        IConfiguration configuration,
        ILoggerHelperSingleton appLogger)
    {
        private readonly RequestDelegate _next = next;
        private readonly string workspaceId = configuration[ConfigMap.WORKSPACE_ID];
        private readonly ILoggerHelperSingleton _logger = appLogger;

        public async Task InvokeAsync(HttpContext context, ICETokenService _tokenService)
        {
            string? authHeader = context.Request.Headers[HeaderNames.Authorization].FirstOrDefault();
            try
            {
                _logger.Log(AppLogLevel.Trace, "validating autho token", nameof(UserAuthorization), nameof(InvokeAsync));
                if (context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated)
                {
                    _logger.Log(AppLogLevel.Trace, "claims received", nameof(UserAuthorization), nameof(InvokeAsync));
                    var response = context.User;
                    if (isInternalUser(response))
                    {
                        string CorrelationId = Guid.NewGuid().ToString();
                        context.Items[Constants.CorrelationId] = CorrelationId;
                        context.Response.Headers[Constants.CorrelationId] = CorrelationId;
                        if (response.Claims.Any(claim => claim.Type == ClaimTypes.Email))
                        {
                            context.Items[Constants.UserMail] = response.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
                        }
                        if (response.Claims.Any(claim => claim.Type == Constants.CustomClaimTypes.SpUrl))
                        {
                            context.Items[Constants.CustomClaimTypes.SpUrl] = response.Claims.First(claim => claim.Type == Constants.CustomClaimTypes.SpUrl).Value;
                        }
                        if (response.Claims.Any(claim => claim.Type == Constants.CustomClaimTypes.POApiURL))
                        {
                            context.Items[Constants.CustomClaimTypes.POApiURL] = response.Claims.First(claim => claim.Type == Constants.CustomClaimTypes.POApiURL).Value;
                        }
                        context.Items[HeaderNames.Authorization] = authHeader;
                        await _next(context);
                        return;
                    }
                    else
                    {
                        _logger.Log(AppLogLevel.Trace, "user_type is not present or not allowed", nameof(UserAuthorization), nameof(InvokeAsync));
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        context.Response.Headers[HeaderNames.ContentType] = Constants.ContentType.ApplicationJson;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            error_code = "USER_TYPE_NOT_ALLOWED",
                            message = "Only EY internal users are allowed"
                        });
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log(AppLogLevel.Error, $"Unable to invoke ValidateJwtToken {e.Message}", nameof(UserAuthorization), nameof(InvokeAsync), exception: e);
            }
            _logger.Log(AppLogLevel.Trace, "authorization failed", nameof(UserAuthorization), nameof(InvokeAsync));
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        private bool isInternalUser(ClaimsPrincipal response)
        {
            if (response.Identity != null && response.Identity.AuthenticationType == CEAuthExtension.SecretAuthenticationScheme)
                return true;
            else if (response.Claims.Any(claim => claim.Type == Constants.CustomClaimTypes.UserType) &&
                           response.Claims.First(claim => claim.Type == Constants.CustomClaimTypes.UserType).Value == Constants.UserTypeAllowed
                           )
                return true;
            return false;
        }
    }
}
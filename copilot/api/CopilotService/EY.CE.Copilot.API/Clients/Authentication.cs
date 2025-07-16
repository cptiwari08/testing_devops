using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.API.Common;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Services;
using EY.SaT.CapitalEdge.Extensions.Logging.Models;
using EY.CE.Copilot.API.Authorization;

namespace EY.CE.Copilot.API.Clients
{
    public class Authentication : IAuthentication
    {
        private readonly IConfiguration configuration;
        private static readonly HttpClient httpClient = new HttpClient();
        private static AuthToken TokenData = new AuthToken();
        private static string clientId;
        private static string clientSecret;
        private static string tenantid;
        private static string tokenEndpoint;
        private string workspaceregion;
        private string workspaceName;
        private static readonly SemaphoreSlim semaphoreSlimTokens = new SemaphoreSlim(1);
        private readonly ConcurrentDictionary<string, string> tokenCache = new ConcurrentDictionary<string, string>();
        private bool disposedValue;
        private AutoResetEvent tokenResetEvent = null;
        private static string authorityURL;
        private ILoggerHelperSingleton _logger;
        public Authentication(IConfiguration configuration, ILoggerHelperSingleton logger)
        {
            this.configuration = configuration;
            clientId = configuration[SharedKeyVault.AZURE_SPN_ID];
            clientSecret = configuration[SharedKeyVault.AZURE_SPN_SECRET];
            tenantid = configuration[SharedKeyVault.AZURE_TENANT_ID];
            tokenEndpoint = $@"https://login.microsoftonline.com/{tenantid}/oauth2/token";
            workspaceregion = configuration[ConfigMap.WORKSPACE_REGION];
            workspaceName = configuration[ConfigMap.WORKSPACE_NAME];
            authorityURL = $@"https://sts.windows.net/{tenantid}/oauth2/";
            _logger = logger;
        }

        internal class TokenWaitInfo
        {
            public RegisteredWaitHandle Handle = null;
        }

        public async Task<AuthToken> GetSSPAuthToken()
        {
            try
            {
                _logger.Log(AppLogLevel.Trace, "Getting SSP Auth Token", nameof(Authentication), nameof(GetSSPAuthToken));
                if (TokenData != null && TokenData.ExpiresOn > DateTimeOffset.UtcNow.AddMinutes(2))
                {
                    return new AuthToken
                    {
                        AccessToken = TokenData.AccessToken
                    };
                }
                else
                {
                    var ApplicationId = configuration[SharedKeyVault.AZURE_SPN_ID];
                    var ApplicationSecret = configuration[SharedKeyVault.AZURE_SPN_SECRET];

                    using (var httpClient = new HttpClient())
                    {
                        var content = new Dictionary<string, string>();
                        content[Constants.AuthToken.GrantType] = Constants.AuthToken.GrantTypeSecret;
                        content[Constants.AuthToken.Resource] = ApplicationId;
                        content[Constants.AuthToken.ClientId] = ApplicationId;
                        content[Constants.AuthToken.ClientSecret] = ApplicationSecret;
                        content[Constants.AuthToken.Authority] = authorityURL;

                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type: application/x-www-form-urlencoded", "application/json");

                        FormUrlEncodedContent encodedContent = new FormUrlEncodedContent(content);
                        HttpResponseMessage response = await httpClient.PostAsync(tokenEndpoint, encodedContent);
                        string result = response.Content.ReadAsStringAsync().Result;
                        var token = JsonConvert.DeserializeObject<AuthToken>(result);
                        TokenData.AccessToken = token.AccessToken;
                        TokenData.ExpiresOn = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn);
                        return token;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log(AppLogLevel.Error, $"Error in method GetSSPAuthToken - {e.Message}", nameof(Authentication), nameof(GetSSPAuthToken), exception: e);
                return null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (tokenResetEvent != null)
                    {
                        tokenResetEvent.Set();
                        tokenResetEvent.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
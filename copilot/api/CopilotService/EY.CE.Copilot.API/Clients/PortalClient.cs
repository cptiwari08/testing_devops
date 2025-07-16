using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Models.Portal;
using EY.CE.Copilot.API.Static;
using EY.CE.Copilot.Data.Models;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using static EY.CE.Copilot.API.Models.Chat;

namespace EY.CE.Copilot.API.Clients
{
    /// <summary>
    /// This class is responsible for making calls to the Self Service Portal (SSP) API.
    /// </summary>
    public class PortalClient : BaseClass, IPortalClient
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthentication _auth;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string? workspaceId;
        public PortalClient(
            IHttpClientFactory clientFactory,
            IConfiguration configuration,
            IAuthentication authentication,
            IAppLoggerService logger) : base(logger, nameof(PortalClient))
        {
            _configuration = configuration;
            _httpClientFactory = clientFactory;
            workspaceId = configuration[ConfigMap.WORKSPACE_ID];
            _auth = authentication;
        }

        #region Public methods

        /// <summary>
        /// Retrieves the project details.
        /// </summary>
        /// <returns>The project data.</returns>
        public async Task<ProjectData> GetProjectDetails()
        {
            try
            {
                Log(AppLogLevel.Trace, $"Getting project details", nameof(GetProjectDetails));
                var endPoint = string.Format(PortalEndPoints.GetProjectDetails, workspaceId);
                return await GetAsync<ProjectData>(endPoint);
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetProjectDetails - {e.Message}", nameof(GetProjectDetails), exception: e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the configuration value for the specified key.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The configuration value.</returns>
        public async Task<CopilotConfiguration> GetConfiguration(string key)
        {
            ProjectData project = await GetProjectDetails();

            if (string.Equals(key, Constants.IsAssistantEnabled, StringComparison.OrdinalIgnoreCase))
            {
                return new CopilotConfiguration
                {
                    Key = $"{Constants.PortalConfigurationPrefix}{Constants.IsAssistantEnabled}",
                    Value = project.Data.IsAssistantEnabled.ToString()
                };
            }

            throw new Exception($"Portal Configuration key {key} not found.");
        }

        /// <summary>
        /// Retrieves the user details.
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>

        public async Task<PortalUser> GetUserDetails(string emailId)
        {
            try
            {
                Log(AppLogLevel.Trace, $"Getting user details", nameof(GetUserDetails));
                var endPoint = string.Format(PortalEndPoints.GetUserDetails, emailId);
                return await GetAsync<PortalUser>(endPoint);
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in method GetUserDetails - {e.Message}", nameof(GetUserDetails), exception: e);
                throw;
            }
        }

        public async Task<Models.User> GetMe(string emailId)
        {
            Log(AppLogLevel.Trace, "Getting user details", nameof(GetMe));
            var endPoint = string.Format(PortalEndPoints.GetUserDetails, emailId);
            PortalUser portalUser = await GetAsync<PortalUser>(endPoint);
            Models.Data user = portalUser.Data?.FirstOrDefault();

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return new Models.User
            {
                GivenName = user.GivenName,
                Surname = user.Surname,
                Photo = user.Photo
            };
        }

        /// <summary>
        /// Retrieves the list of apps for which user has access to.
        /// </summary>
        /// <param name="token">The user token for authorization.</param>
        /// <returns>The list of apps.</returns>
        public async Task<List<ActivePOApp>> GetApps(string token)
        {
            Log(AppLogLevel.Trace, "Getting apps", nameof(GetApps));

            try
            {
                string endPoint = string.Format(PortalEndPoints.GetAppsList, workspaceId);
                Apps result = await GetAsync<Apps>(endPoint, token);
                var appPlatformClassIds = result.Data?.AppPlatformDetails
                    ?.Where(ad => ad.Name == "Program Office" || ad.Name == "SharePoint")
                    ?.Select(ad => ad.AppPlatformClassId)
                    ?.ToList() ?? new List<string>();

                var keyValuePairs = result.Data?.AppDetails
                    ?.Where(ad => appPlatformClassIds.Contains(ad.AppPlatformId))
                    ?.Select(ad => new KeyValuePair<string, string>(ad.Key, ad.AppClassName))
                    ?.ToList() ?? new List<KeyValuePair<string, string>>();

                List<ActivePOApp> activePOApps = keyValuePairs.Select(kvp => new ActivePOApp
                {
                    Key = kvp.Key,
                    Name = kvp.Value
                }).ToList();

                return activePOApps;
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error in getting App details - {e.Message}", nameof(GetApps), exception: e);
                throw;
            }
        }

        public async Task UpdateConfiguration(ConfigurationUpdate configuration)
        {
            Log(AppLogLevel.Trace, $"Updating portal configuration {configuration.Key}", nameof(UpdateConfiguration));

            if (string.Equals(configuration.Key, Constants.IsAssistantEnabled, StringComparison.OrdinalIgnoreCase))
            {
                var request = new
                {
                    CopilotDetails = new
                    {
                        IsActive = bool.Parse(configuration.Value)
                    }
                };

                var endPoint = string.Format(PortalEndPoints.PatchProjectDetails, workspaceId);
                await PatchAsync(endPoint, JsonContent.Create(request));
                return;
            }
            throw new Exception($"Portal Configuration key {configuration.Key} not found.");
        }

        public async Task UpdateConfiguration(CopilotConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends an HTTP GET request to the specified endpoint and deserializes the response to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="endPoint">The endpoint to send the request to.</param>
        /// <returns>The deserialized response of type T.</returns>
        private async Task<T> GetAsync<T>(string endPoint)
        {
            var token = await _auth.GetSSPAuthToken() ?? throw new Exception("Failed to get token from SSP.");
            using var httpClient = _httpClientFactory.CreateClient(Static.Constants.SSPHttpClient);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Static.Constants.Bearer, token.AccessToken);
            HttpResponseMessage response = await httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception($"Failed to make GET request. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            }
        }

        private async Task PatchAsync(string endPoint, HttpContent content)
        {
            var token = await _auth.GetSSPAuthToken() ?? throw new Exception("Failed to get token from SSP.");
            using var httpClient = _httpClientFactory.CreateClient(Static.Constants.SSPHttpClient);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Static.Constants.Bearer, token.AccessToken);
            HttpResponseMessage response = await httpClient.PatchAsync(endPoint, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to make PATCH request. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Sends an HTTP GET request to the specified endpoint and deserializes the response to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="endPoint">The endpoint to send the request to.</param>
        /// <param name="userToken"> Token for user context </param>
        /// <returns>The deserialized response of type T.</returns>
        private async Task<T> GetAsync<T>(string endPoint, string userToken)
        {
            using var httpClient = _httpClientFactory.CreateClient(Static.Constants.SSPHttpClient);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Static.Constants.Bearer, userToken);
            httpClient.DefaultRequestHeaders.Add("Request-Timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());
            HttpResponseMessage response = await httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception($"Failed to make GET request. Status code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            }
        }

        public async Task TriggerWorkflows(AsyncOperationRequest request, string userEmail, string instanceId)
        {
            try
            {
                var token = await _auth.GetSSPAuthToken() ?? throw new Exception("Failed to get token from SSP.");
                using var httpClient = _httpClientFactory.CreateClient(Static.Constants.SSPHttpClient);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Static.Constants.Bearer, token.AccessToken);
                var wfRequests = CreateWorkflowRequest(request, instanceId, Constants.SspEndpoint.AI_CONTENT_GENERATOR_WORKFLOW, userEmail);
                var payload = System.Text.Json.JsonSerializer.Serialize(wfRequests);
                var httpContent = ToJsonStringContent(StringToBase64(payload));
                var response = await httpClient.PostAsync($"{Constants.SspEndpoint.TriggerWorkflow}/{workspaceId}", httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Log(AppLogLevel.Error, $"Failed while trigering workflow(s): Status code : {response.StatusCode}. Error: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Failed while trigering workflow(s): {ex}", exception: ex);
            }
        }
        private List<StartWorkflowRequest> CreateWorkflowRequest(AsyncOperationRequest request, string correlationId, string workflowKey, string UserEmail)
        {
            var wfRequests = new List<StartWorkflowRequest>
            {
                new StartWorkflowRequest
                {
                    CorrelationId = correlationId,
                    RequesterUsername = UserEmail,
                    WorkflowKey = workflowKey,
                    WorkflowParameters = Serializer.SerializeToJson(request)
                }
            };
            return wfRequests;
        }
        private string StringToBase64(string plainText)
        {
            if (plainText == null) { return null; }
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
            return encodedString == "null" ? null : encodedString;
        }
        private StringContent ToJsonStringContent(string jsonString)
        {
            return new StringContent(jsonString, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
        #endregion Private methods
    }

}
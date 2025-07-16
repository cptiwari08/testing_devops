using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace EY.CE.Copilot.API.Clients
{
    public class OrchestratorClient : BaseClass, IOrchestratorClient
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;
        private string baseUrl = string.Empty;
        public OrchestratorClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IAppLoggerService logger) :
            base(logger, nameof(OrchestratorClient))
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
            baseUrl = configuration[SharedKeyVault.ORCHESTRATOR_BASE_URL];
        }

        public async Task<IActionResult> GetRequest(string token, string endpoint)
        {
            try
            {
                Log(AppLogLevel.Trace, $@"Orchestrator | Calling GetRequest");
                HttpClient client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(baseUrl);
                HttpResponseMessage response = await client.GetAsync(endpoint);
                return new ContentResult
                {
                    Content = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error from OrchestratorClient method GetRequest {e.Message}", exception: e);
                return new ContentResult
                {
                    Content = e.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<IActionResult> ParameterizedGetRequest(string token, string endpoint, Dictionary<string, string> queryParameters)
        {
            try
            {
                Log(AppLogLevel.Trace, $@"Orchestrator | Calling ParameterizedGetRequest");
                HttpClient client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(baseUrl);
                string queryString = string.Join("&", queryParameters.Select(x => $"{x.Key}={x.Value}"));
                HttpResponseMessage response = await client.GetAsync($"{endpoint}?{queryString}");
                return new ContentResult
                {
                    Content = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error from OrchestratorClient method ParameterizedGetRequest {e.Message}", exception: e);
                return new ContentResult
                {
                    Content = e.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<IActionResult> PostRequest(string token, string endpoint, string jsonPayload)
        {
            try
            {
                Log(AppLogLevel.Trace, $@"Orchestrator | Calling PostRequest");
                HttpClient client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(baseUrl);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                return new ContentResult
                {
                    Content = await response.Content.ReadAsStringAsync(),
                    StatusCode = (int)response.StatusCode
                };
            }
            catch (Exception e)
            {
                Log(AppLogLevel.Error, $"Error from OrchestratorClient method PostRequest {e.Message}", exception: e);
                return new ContentResult
                {
                    Content = e.Message,
                    StatusCode = 500
                };
            }
        }

    }
}
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EY.CapitalEdge.ChatOrchestrator.Activities
{
    public class HttpCallActivity
    {
        private readonly HttpClient _httpClient;
        private readonly Services.ILoggerProvider _loggerProvider;

        private const string ProjectFriendlyId = "projectFriendlyId";

        public HttpCallActivity(IHttpClientFactory httpClientFactory, Services.ILoggerProvider loggerProvider)
        {
            _httpClient = httpClientFactory.CreateClient();
            int httpClientTimeout = int.TryParse(Environment.GetEnvironmentVariable("HttpClientTimeout"), out int number) ? number : 100;
            _httpClient.Timeout = TimeSpan.FromSeconds(httpClientTimeout);
            _loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Generic HTTP call activity.
        /// </summary>
        /// <param name="serializedRequest">serializedRequest</param>
        /// <param name="executionContext">executionContext</param>
        /// <returns>SerializableHttpResponseMessage</returns>
        [Function(nameof(HttpCallActivity))]
        public async Task<SerializableHttpResponseMessage> Run([ActivityTrigger] HttpCallActivityInput input, FunctionContext executionContext)
        {
            ILogger log = _loggerProvider.GetLogger(executionContext, "HttpCallActivity");
            ILogger logger = new CustomLogger(log, $"[instanceId:{input.InstanceId}] [chatId:{input.ChatId}] [{ProjectFriendlyId}:{input.ProjectFriendlyId}]");
            HttpRequestMessage request = HttpRequestConverter.ConvertToHttpRequestMessage(input.SerializedRequest);
            logger.LogInformation("Calling url {Url}.", request.RequestUri);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error calling url {Url}.", request.RequestUri);
                return new SerializableHttpResponseMessage(request.Method, request.RequestUri?.ToString() ?? "", null, ex.Message, false, null);
            }

            var serializableResponse = new SerializableHttpResponseMessage(
                response.RequestMessage?.Method, 
                response.RequestMessage?.RequestUri?.ToString() ?? "",
                (int)response.StatusCode,
                response.ReasonPhrase,
                response.IsSuccessStatusCode,
                response.Content != null ? await response.Content.ReadAsStringAsync() : null);

            return serializableResponse;
        }
    }
}
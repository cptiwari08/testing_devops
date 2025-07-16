using DurableTask.Core.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Started.Activities
{
    public class HttpCallActivity
    {
        private readonly HttpClient _httpClient;

        public HttpCallActivity(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [Function(nameof(HttpCallActivity))]
        public async Task<string> Run([ActivityTrigger] string url, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("HttpCallActivity");
            logger.LogInformation("Calling url {url}.", url);

            var result = await _httpClient.GetAsync(url);
            if (!result.IsSuccessStatusCode)
            {
                throw new TaskFailedException($"Calling url {url} returned status code {result.StatusCode}");
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}
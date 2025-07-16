using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Started.Models;
using Started.Orchestrations;
using System.Net;
using System.Text.Json;

namespace Started.Clients
{
    public class CapitalEdgeChatStarterClient
    {
        [Function("CapitalEdgeChatStarterClient_HttpStart")]
        public async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("CapitalEdgeChatStarterClient_HttpStart");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QuestionInfo? questionInfo = JsonSerializer.Deserialize<QuestionInfo>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (questionInfo is null || string.IsNullOrWhiteSpace(questionInfo.Question) || string.IsNullOrWhiteSpace(questionInfo.UserContext))
                return req.CreateResponse(HttpStatusCode.BadRequest);

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(CapitalEdgeChatOrchestrator), questionInfo);

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}

using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Services;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text.Json;

namespace EY.CapitalEdge.ChatOrchestrator.Clients
{
    public class ChatStarterClient
    {
        private readonly Services.ILoggerProvider _loggerProvider;
        private readonly ICommon _common;
        private readonly IDurableTaskClientWrapper _clientWrapper;

        private const string EYGuidance = "ey-guidance";
        private const string Internet = "internet";
        private const string ProjectDoc = "project-docs";
        private const string ProjectData = "project-data";

        private const string User = "user";

        private static readonly JsonSerializerOptions _jsonSerializerOptionsDeserializeRequestBody = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ChatStarterClient(Services.ILoggerProvider loggerProvider, ICommon common, IDurableTaskClientWrapper clientWrapper)
        {
            _loggerProvider = loggerProvider;
            _common = common ?? throw new ArgumentNullException(nameof(common));
            _clientWrapper = clientWrapper;
        }

        /// <summary>
        /// Delegate the chat question to the orchestrator
        /// </summary>
        /// <param name="req">req</param>
        /// <param name="client">client</param>
        /// <param name="executionContext">executionContext</param>
        /// <returns>An HTTP 202 response with a Location header and a payload containing instance control urls</returns>
        [Function("Chat")]
        [OpenApiOperation(operationId: nameof(Chat), tags: ["Chat"], Description = "Manage the workflow to answer a question")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ChatQuestionInput), Required = true, Description = "Question payload: You can include either one or two sources in your request. The inclusion of the 'context' property is optional.")]
        [OpenApiResponseWithBody(HttpStatusCode.Accepted, contentType: "application/json", bodyType: typeof(ChatQuestionBodyResponse))]
        [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError)]
        public async Task<HttpResponseData> Chat(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "chat")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = _loggerProvider.GetLogger(executionContext, "Chat");

            var authorization = req.Headers.FirstOrDefault(f => f.Key.Equals("Authorization"));
            if (authorization.Value is null)
            {
                logger.LogError("Authorization token was not provided");
                return req.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var token = string.Join(",", authorization.Value);
            if (!ValidateToken(logger, token))
                return req.CreateResponse(HttpStatusCode.Unauthorized);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(requestBody))
            {
                logger.LogError("Body was not provided");
                return await BadRequest(req, "Body was not provided");
            }

            ChatQuestionInput? chatQuestionInput = DeserializeRequestBody(logger, requestBody);
            if (chatQuestionInput == null)
            {
                logger.LogError("Body format is not valid");
                return await BadRequest(req, "Body format is not valid");
            }

            if (chatQuestionInput.MessageId == 0)
            {
                logger.LogError("messageId should be greater than 0");
                return await BadRequest(req, "messageId should be greater than 0");
            }

            if (chatQuestionInput.ChatId == Guid.Empty)
            {
                logger.LogError("chatId is not a valid Guid");
                return await BadRequest(req, "chatId is not a valid Guid");
            }

            if (string.IsNullOrWhiteSpace(chatQuestionInput.Question))
            {
                logger.LogError("No question provided");
                return await BadRequest(req, "No question provided");
            }

            var sources = chatQuestionInput.Sources.ToArray();

            if (_common.IsThereAnySuggestedQuestion(chatQuestionInput.Context))
            {
                var suggested = _common.GetSuggestedQuestion(chatQuestionInput.Context);
                sources = GetSourcesFromSuggestions(suggested.Source);
            }

            foreach (var source in sources)
            {
                if (source != EYGuidance && source != Internet && source != ProjectDoc && source != ProjectData)
                {
                    logger.LogError("Invalid source provided, the valid ones are {EYGuidance}, {Internet}, {ProjectDoc} and {ProjectData}", EYGuidance, Internet, ProjectDoc, ProjectData);
                    return await BadRequest(req, $"Invalid source provided, the valid ones are {EYGuidance}, {Internet}, {ProjectDoc} and {ProjectData}");
                }
            }

            if (sources.Length != 1 && sources.Length != 2)
            {
                logger.LogError("Sources provided must be 1 or 2");
                return await BadRequest(req, "Sources provided must be 1 or 2");
            }

            Token tokenData = _common.GetTokenData(logger, token);
            sources = sources.Intersect(tokenData.Scope).ToArray();
            if (sources.Length == 0)
            {
                logger.LogError("User does not have access to the sources provided");
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            chatQuestionInput.Context ??= [];
            chatQuestionInput.Context.Remove(User);
            chatQuestionInput.Context.Add(User, new { email = tokenData.Email, familyName = tokenData.FamilyName, givenName = tokenData.GivenName });
            
            string? projectFriendlyId = tokenData.ProjectFriendlyId;
            if (string.IsNullOrEmpty(projectFriendlyId))
            {
                logger.LogError("ProjectFriendlyId is not provided");
                return await BadRequest(req, "ProjectFriendlyId is not provided");
            }
            
            RemoveAppInfoIfNoValid(chatQuestionInput);

            ChatQuestion chatQuestion = new()
            {
                MessageId = chatQuestionInput.MessageId,
                ChatId = chatQuestionInput.ChatId,
                ProjectFriendlyId = projectFriendlyId,
                Question = chatQuestionInput.Question,
                Sources = sources,
                InputSources = chatQuestionInput.Sources,
                Token = token,
                Context = chatQuestionInput.Context
            };

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(ChatOrchestrator), chatQuestion);

            logger.LogInformation("Started orchestration with ID = '{InstanceId}'.", instanceId);

            _clientWrapper.SetDurableTaskClient(client);
            return await _clientWrapper.CreateCheckStatusResponseAsync(req, instanceId, HttpStatusCode.Accepted, CancellationToken.None);
        }

        /// <summary>
        /// Deserialize request body
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        private static ChatQuestionInput? DeserializeRequestBody(ILogger logger, string requestBody)
        {
            try
            {
                return JsonSerializer.Deserialize<ChatQuestionInput>(requestBody, _jsonSerializerOptionsDeserializeRequestBody);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Deserialization error");
                return null;
            }
        }

        /// <summary>
        /// Validate token
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>Bolean</returns>
        private bool ValidateToken(ILogger logger, string token)
        {           
            string issuer = _common.GetEnvironmentVariable(logger, "Issuer");
            string publicKey = _common.GetEnvironmentVariable(logger, "PublicKey");

            return _common.ValidateToken(logger, token, issuer, publicKey);
        }

        /// <summary>
        /// Bad request response
        /// </summary>
        /// <param name="req">req</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        private static async Task<HttpResponseData> BadRequest(HttpRequestData req, string message)
        {
            var response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            await response.WriteStringAsync(message);
            return response;
        }

        /// <summary>
        /// REmove app info if no valid
        /// </summary>
        /// <param name="chatQuestionInput">chatQuestionInput</param>
        private void RemoveAppInfoIfNoValid(ChatQuestionInput? chatQuestionInput)
        {
            if (!_common.IsThereAnyAppInfo(chatQuestionInput?.Context))
                chatQuestionInput?.Context?.Remove("appInfo");
        }

        /// <summary>
        /// Get sources from suggestions
        /// </summary>
        /// <param name="source">Sources separated by coma</param>
        /// <returns></returns>
        private static string[] GetSourcesFromSuggestions(string? source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return Array.Empty<string>();

            string[] sources = source.Split(",");

            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = sources[i].Trim();
            }

            return sources;
        }
    }
}

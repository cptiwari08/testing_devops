using EY.CapitalEdge.ChatOrchestrator.Activities;
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Models.ChatHistoryService;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace EY.CapitalEdge.ChatOrchestrator.Orchestrators
{
    public class ChatOrchestrator
    {
        private readonly ICommon _common;

        // Compose frontend source names
        private const string EYGuidance = "ey-guidance"; // ey-guidance that comes as frontend input represent ey-ip + help-copilot backends

        private const string EYIPBackend = "ey-ip";
        private const string HelpCopilotBackend = "help-copilot";

        private const string EYIP = $"{EYGuidance}-{EYIPBackend}";
        private const string HelpCopilot = $"{EYGuidance}-{HelpCopilotBackend}";
        private const string Internet = "internet";
        private const string ProjectDoc = "project-docs";
        private const string ProjectData = "project-data";

        private readonly Dictionary<string, string> sourceNamesFrontend = new()
        {
            { EYGuidance, "EY Guidance" },
            { Internet, "Internet" },
            { ProjectDoc, "Project Docs" },
            { ProjectData, "Project Data" },
            { HelpCopilot, "Help Copilot" },
            { EYIP, "EY IP" }
        };

        // Status codes
        private const string Ok = "200";

        // Environment variables
        private const string ChatBotPython = "ChatBotPython";
        private const string ChatBotHelpCopilot = "ChatBotHelpCopilot";

        // Headers
        private const string Authorization = "Authorization";

        // Context keys
        private const string ProjectDescription = "projectDescription";
        private const string User = "user";
        private const string Documents = "documents";
        private const string Suggestion = "suggestion";
        private const string AppInfo = "appInfo";
        private const string ProjectFriendlyId = "projectFriendlyId";

        private const string MediaType = "application/json";
        private const string Encoding = "UTF8";

        public ChatOrchestrator(ICommon common)
        {
            _common = common ?? throw new ArgumentNullException(nameof(common));
        }

        /// <summary>
        /// Manage the workflow to answer a question, based on the sources provided in the context, 
        /// the orchestrator will call the corresponding backend services and if more than one source is provided, 
        /// will do a summarization and store the question and answer in the chat history
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>The answer and citing sources</returns>
        [Function(nameof(ChatOrchestrator))]
        public async Task<Message> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var createdTime = context.CurrentUtcDateTime;

            ILogger log = context.CreateReplaySafeLogger<ChatOrchestrator>();

            ChatQuestion input = GetChatQuestion(context.GetInput<ChatQuestion>());

            ILogger logger = new CustomLogger(log, $"[instanceId:{context.InstanceId}] [chatId:{input.ChatId}] [{ProjectFriendlyId}:{input.ProjectFriendlyId}]");
            logger.LogInformation("Orchestrator is running.");

            var message = new Message
            {
                Role = "assistant",
                Content = string.Empty,
                Sources = input.Sources,
                InputSources = input.InputSources,
                Context = [],
                CreatedTime = createdTime
            };

            if (input.Context is not null)
                message.Context = new Dictionary<string, object?>(input.Context);

            message.Context ??= [];
            message.Context.Add("isMessageLiked", null);

            message.Context.Remove(User);
            message.Context.Remove(Documents);

            input.Sources = SplitEYGuidance(input);
            input.Context ??= [];
            input.Context.Add(ProjectDescription, await GetProjectDescription(context, logger, input));

            List<Message> chatHistoryFromCache = await GetChatHistoryActivity(context, logger, input);

            List<ChatHistory> chatHistory = MapChatHistoryForBackends(chatHistoryFromCache);

            Task<List<FollowUpSuggestionsResponse>> followUpSuggestions = GetFollowUpSuggestions(context, input, chatHistory);

            List<(int Index, string Backend, Task<SerializableHttpResponseMessage> Task)> parallelTasks = new List<(int, string, Task<SerializableHttpResponseMessage>)>();

            if (input.Sources.Contains(HelpCopilot))
            {
                SerializableHttpResponseMessage conversation = await StartHelpCopilotConversation(context, logger, input);
                AddParallelTaskToGetHelpCopilotAnswer(context, input, chatHistory, conversation, parallelTasks);
            }

            if (input.Sources.Contains(Internet))
                AddParallelTaskToGetInternetAnswer(context, input, chatHistory, parallelTasks);

            if (input.Sources.Contains(EYIP))
                AddParallelTaskToGetEYIPAnswer(context, input, chatHistory, parallelTasks);

            if (input.Sources.Contains(ProjectDoc))
                AddParallelTaskToGetProjectDocAnswer(context, input, chatHistory, parallelTasks);

            if (input.Sources.Contains(ProjectData))
                AddParallelTaskToGetProjectDataAnswer(context, input, chatHistory, parallelTasks);

            await Task.WhenAll(parallelTasks.Select(t => t.Task));

            List<BackendResponse> backends = GetBackendResponses(logger, parallelTasks.OrderBy(o => o.Index).ToList());

            if (input.Sources.Contains(HelpCopilot) && input.Sources.Contains(EYIP))
            {
                BackendResponse eyGuidance = await GetEYGuidance(context, logger, backends, input);
                int position = backends.FindIndex(f => f.Backend == HelpCopilot);
                backends.Insert(position, eyGuidance);
            }

            List<string> excludedBackends = [HelpCopilot, EYIP];
            List<BackendResponse> filteredBackends = backends.Where(w => !excludedBackends.Contains(w.Backend) && w.Status == Ok).ToList();
            string summarize = string.Empty;
            if (filteredBackends.Count == 0)
                summarize = "No backend responses available for summarization.";
            else
            {
                string task = await GetSummarizationPromptAsync(context, filteredBackends, input);
                summarize = await GetSummarization(context, logger, input, task);
            }

            message.Content = GetDetailAnswer(backends, summarize);

            backends.Insert(0, new BackendResponse { Backend = "summary", Response = summarize, Status = summarize.Contains("error") ? "" : Ok, CitingSources = null });

            AddSourcesToMessage(logger, backends, message);

            message.MessageId = input.MessageId + 1;
            message.LastUpdatedTime = context.CurrentUtcDateTime;
            message.FollowUpSuggestions = await followUpSuggestions;
            if (input.Sources.Contains(ProjectDoc))
            {
                backends.ForEach(f => f.FollowUpSuggestions?.ForEach(s => message.FollowUpSuggestions.Add((new FollowUpSuggestionsResponse { Id = s.Id, SuggestionText = s.SuggestionText }))));
            }

            return message;
        }

        /// <summary>
        /// Get follow up suggestions
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="input">input</param>
        /// <param name="chatHistory">chatHistory</param>
        /// <returns>List of follow up suggestions</returns>
        private static Task<List<FollowUpSuggestionsResponse>> GetFollowUpSuggestions(TaskOrchestrationContext context, ChatQuestion input, List<ChatHistory> chatHistory)
        {
            FollowUpSuggestionInput followUpSuggestionInput = new()
            {
                InstanceId = context.InstanceId,
                ChatId = input.ChatId,
                Input = input,
                ChatHistory = chatHistory,
                ProjectFriendlyId = input.ProjectFriendlyId
            };
            return context.CallActivityAsync<List<FollowUpSuggestionsResponse>>(nameof(FollowUpSuggestionActivity), followUpSuggestionInput);
        }

        /// <summary>
        /// Validate input with bussiness rules
        /// </summary>
        /// <param name="input">input</param>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        private static ChatQuestion GetChatQuestion(ChatQuestion? input)
        {
            if (input is null
                || input.ChatId == Guid.Empty
                || string.IsNullOrWhiteSpace(input.Question)
                || input.MessageId == 0)
                throw new ArgumentNullException(nameof(input), "ChatQuestion was not provided correctly");

            input.Sources = input.Sources.Distinct().ToArray();
            foreach (var source in input.Sources)
            {
                if (source != EYGuidance && source != Internet && source != ProjectDoc && source != ProjectData)
                    throw new ArgumentNullException(nameof(input), $"Invalid source provided, the valid ones a are {EYGuidance}, {Internet}, {ProjectDoc} and {ProjectData}");
            }

            if (input.Sources.Length != 1 && input.Sources.Length != 2)
                throw new ArgumentNullException(nameof(input), "Sources provided must be 1 or 2");

            ChatQuestion chatQuestion = new()
            {
                ChatId = input.ChatId,
                ProjectFriendlyId = input.ProjectFriendlyId,
                Question = input.Question,
                MessageId = input.MessageId,
                Sources = input.Sources,
                InputSources = input.InputSources,
                Context = input.Context ?? [],
                Token = input.Token
            };

            return chatQuestion;
        }

        /// <summary>
        /// If EYGuidance exist, split it into EYIP and HelpCopilot
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>input</returns>
        private static string[] SplitEYGuidance(ChatQuestion input)
        {
            if (input.Sources.Contains(EYGuidance))
            {
                input.Sources = input.Sources.Where(s => s != EYGuidance).ToArray();
                input.Sources = [.. input.Sources, EYIP, HelpCopilot];
            }

            return input.Sources;
        }

        /// <summary>
        /// Get project description from copilot api
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="logger">logger</param>
        /// <param name="input">input</param>
        /// <returns>Project description</returns>
        private async Task<string> GetProjectDescription(TaskOrchestrationContext context, ILogger logger, ChatQuestion input)
        {
            string projectDescription = string.Empty;
            try
            {
                Token token = _common.GetTokenData(logger, input.Token);
                string? copilotApiUrl = token?.CopilotApiUrl;
                if (string.IsNullOrWhiteSpace(copilotApiUrl) || !_common.IsValidUrl(copilotApiUrl))
                    throw new ArgumentException($"CopilotApiUrl: {copilotApiUrl} is not valid url.");

                var request = new SerializableHttpRequestMessage(HttpMethod.Get,
                    $"{copilotApiUrl}/configuration/PROJECT_CONTEXT",
                    new Dictionary<string, string> { { Authorization, input.Token ?? "" } });
                var result = await context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request });
                IfNoSuccessStatusCodeThrowHttpRequestException(logger, result);

                if (result.StatusCode == 204)
                    return projectDescription;

                var projectContext = _common.Deserialize<ProjectContextResponse>(result.Response);
                return projectContext.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return projectDescription;
            }
        }

        /// <summary>
        /// Get chat history from Copilot API if exeption occurs return empty list
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="logger">logger</param>
        /// <param name="input">input</param>
        /// <returns>Chat history list</returns>
        private async Task<List<Message>> GetChatHistoryActivity(TaskOrchestrationContext context, ILogger logger, ChatQuestion input)
        {
            List<Message> chatHistory = new List<Message>();
            try
            {
                Token token = _common.GetTokenData(logger, input.Token);
                string? copilotApiUrl = token?.CopilotApiUrl;
                if (string.IsNullOrWhiteSpace(copilotApiUrl) || !_common.IsValidUrl(copilotApiUrl))
                    throw new ArgumentException($"CopilotApiUrl: {copilotApiUrl} is not valid url.");

                var request = new SerializableHttpRequestMessage(HttpMethod.Get,
                    $"{copilotApiUrl}/user/chat/{input.ChatId}",
                    new Dictionary<string, string> { { Authorization, input.Token ?? "" } });
                var result = await context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request });
                IfNoSuccessStatusCodeThrowHttpRequestException(logger, result);

                if (result.StatusCode == 204)
                    return chatHistory;

                ChatHistoryResponse chat = _common.Deserialize<ChatHistoryResponse>(result.Response);
                // As frontend add chat history pair for current question with default answer, because it is waiting for orchestrator response,
                // removing that from the chat history, because it is not relevant
                List<Message> response = chat.Content.Where(w => w.MessageId < (input.MessageId -2)).ToList();
                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return chatHistory;
            }
        }

        /// <summary>
        /// Map chat history for backends
        /// </summary>
        /// <param name="chatHistory">chatHistory</param>
        /// <returns>ChatHistory list</returns>
        private static List<ChatHistory> MapChatHistoryForBackends(List<Message> chatHistory)
        {
            var chatHistoryForBackends = new List<ChatHistory>();

            if (chatHistory is null)
                return chatHistoryForBackends;

            foreach (var message in chatHistory)
            {
                chatHistoryForBackends.Add(new ChatHistory
                {
                    MessageId = message.MessageId,
                    Role = message.Role,
                    Content = message.Content
                });
            }

            return chatHistoryForBackends;
        }

        /// <summary>
        /// Start help copilot conversation
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="logger">logger</param>
        /// <param name="input">input</param>
        /// <returns>Conversation response with conversation reference id</returns>
        private async Task<SerializableHttpResponseMessage> StartHelpCopilotConversation(TaskOrchestrationContext context, ILogger logger, ChatQuestion input)
        {
            SerializableHttpResponseMessage result;
            var request = new SerializableHttpRequestMessage(HttpMethod.Post, requestUri: $"{Environment.GetEnvironmentVariable(ChatBotHelpCopilot)}/api/v1/help-copilot/chat/start", new Dictionary<string, string> { { Authorization, input.Token ?? "" } });

            try
            {
                request.Content = new StrContent(JsonSerializer.Serialize(new BackendInput
                {
                    ChatId = input.ChatId,
                    InstanceId = context.InstanceId,
                    ProjectFriendlyId = input.ProjectFriendlyId
                }), MediaType, Encoding);
                result = await context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request });
                if (!result.IsSuccessStatusCode)
                {
                    logger.LogError("Error calling backend service: {Url}, Status code: {StatusCode}, Reason: {ReasonPhrase}", result.RequestUri, result.StatusCode, result.ReasonPhrase);
                    throw new HttpRequestException($"Error calling backend service: {result.RequestUri}, Status code: {result.StatusCode}, Reason: {result.ReasonPhrase}", null, result.StatusCode == null ? HttpStatusCode.InternalServerError : (HttpStatusCode)result.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, ex.Message);
                return new SerializableHttpResponseMessage(request.Method, request.RequestUri, ex.StatusCode == null ? 500 : (int)ex.StatusCode, ex.Message, false, null);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new SerializableHttpResponseMessage(request.Method, request.RequestUri, null, $"Error calling backend service: {request.RequestUri}", false, null);
            }

            return result;
        }

        /// <summary>
        /// Add parallel task to get the answer from the helpcopilot backend
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="logger">logger</param>
        /// <param name="input">input</param>
        /// <param name="chatHistory">chatHistory</param>
        /// <param name="parallelTasks">parallelTasks</param>
        private void AddParallelTaskToGetHelpCopilotAnswer(TaskOrchestrationContext context, ChatQuestion input, List<ChatHistory> chatHistory, SerializableHttpResponseMessage conversation, List<(int Index, string Backend, Task<SerializableHttpResponseMessage> Task)> parallelTasks)
        {
            if (!conversation.IsSuccessStatusCode)
            {
                parallelTasks.Add((5, HelpCopilot, Task.FromResult(conversation)));
                return;
            }

            BackendResponse backendResponse = _common.Deserialize<BackendResponse>(conversation.Response);
            ConversationResponse conversationResponse = _common.Deserialize<ConversationResponse>(backendResponse.Response.ToString());

            var request = new SerializableHttpRequestMessage(HttpMethod.Post, $"{Environment.GetEnvironmentVariable(ChatBotHelpCopilot)}/api/v1/help-copilot/chat/sensitive-data-support", new Dictionary<string, string> { { Authorization, input.Token ?? "" } });
            input.Context.Add("conversationReferenceId", conversationResponse.ConversationReferenceId);
            request.Content = new StrContent(JsonSerializer.Serialize(new BackendInput
            {
                ChatId = input.ChatId,
                InstanceId = context.InstanceId,
                ProjectFriendlyId = input.ProjectFriendlyId,
                Question = input.Question,
                ChatHistory = chatHistory,
                Context = FilterContext(input.Context, ["conversationReferenceId"])
            }), MediaType, Encoding);
            parallelTasks.Add((5, HelpCopilot, context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request })));
        }

        /// <summary>
        /// Add parallel task to get the answer from the internet search backend
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="input">input</param>
        /// <param name="chatHistory">chatHistory</param>
        /// <param name="parallelTasks">parallelTasks</param>
        /// 
        private void AddParallelTaskToGetInternetAnswer(TaskOrchestrationContext context, ChatQuestion input, List<ChatHistory> chatHistory, List<(int Index, string Backend, Task<SerializableHttpResponseMessage> Task)> parallelTasks)
        {
            var body = new BackendInput
            {
                ChatId = input.ChatId,
                InstanceId = context.InstanceId,
                ProjectFriendlyId = input.ProjectFriendlyId,
                Question = input.Question,
                ChatHistory = chatHistory,
                Context = FilterContext(input.Context, [ProjectDescription, User])
            };
            var request = new SerializableHttpRequestMessage(HttpMethod.Post, $"{Environment.GetEnvironmentVariable(ChatBotPython)}/internet", new Dictionary<string, string> { { Authorization, input.Token ?? "" } });
            request.Content = new StrContent(_common.SerializeToCamelCase(body), MediaType, Encoding);
            parallelTasks.Add((3, Internet, context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request })));
        }

        /// <summary>
        /// Add parallel task to get the answer from the eyip backend
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="input">input</param>
        /// <param name="chatHistory">chatHistory</param>
        /// <param name="parallelTasks">parallelTasks</param>
        /// 
        private void AddParallelTaskToGetEYIPAnswer(TaskOrchestrationContext context, ChatQuestion input, List<ChatHistory> chatHistory, List<(int Index, string Backend, Task<SerializableHttpResponseMessage> Task)> parallelTasks)
        {
            var body = new BackendInput
            {
                ChatId = input.ChatId,
                InstanceId = context.InstanceId,
                ProjectFriendlyId = input.ProjectFriendlyId,
                Question = input.Question,
                ChatHistory = chatHistory,
                Context = FilterContext(input.Context, [ProjectDescription, Suggestion, User])
            };
            var request = new SerializableHttpRequestMessage(HttpMethod.Post, $"{Environment.GetEnvironmentVariable(ChatBotPython)}/ey-ip", new Dictionary<string, string> { { Authorization, input.Token ?? "" } });
            request.Content = new StrContent(_common.SerializeToCamelCase(body), MediaType, Encoding);
            parallelTasks.Add((6, EYIP, context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request })));
        }

        /// <summary>
        /// Add parallel task to get the answer from the projectdoc backend
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="input">input</param>
        /// <param name="chatHistory">chatHistory</param>
        /// <param name="parallelTasks">parallelTasks</param>
        /// 
        private void AddParallelTaskToGetProjectDocAnswer(TaskOrchestrationContext context, ChatQuestion input, List<ChatHistory> chatHistory, List<(int Index, string Backend, Task<SerializableHttpResponseMessage> Task)> parallelTasks)
        {
            var body = new BackendInput
            {
                ChatId = input.ChatId,
                InstanceId = context.InstanceId,
                ProjectFriendlyId = input.ProjectFriendlyId,
                Question = input.Question,
                ChatHistory = chatHistory,
                Context = FilterContext(input.Context, [ProjectDescription, User, Documents])
            };
            var request = new SerializableHttpRequestMessage(HttpMethod.Post, $"{Environment.GetEnvironmentVariable(ChatBotPython)}/project-docs", new Dictionary<string, string> { { Authorization, input.Token ?? "" } });
            request.Content = new StrContent(_common.SerializeToCamelCase(body), MediaType, Encoding);
            parallelTasks.Add((2, ProjectDoc, context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request })));
        }

        /// <summary>
        /// Add parallel task to get the answer from the projectdata backend
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="input">input</param>
        /// <param name="chatHistory">chatHistory</param>
        /// <param name="parallelTasks">parallelTasks</param>
        /// 
        private void AddParallelTaskToGetProjectDataAnswer(TaskOrchestrationContext context, ChatQuestion input, List<ChatHistory> chatHistory, List<(int Index, string Backend, Task<SerializableHttpResponseMessage> Task)> parallelTasks)
        {
            var body = new BackendInput
            {
                ChatId = input.ChatId,
                InstanceId = context.InstanceId,
                ProjectFriendlyId = input.ProjectFriendlyId,
                Question = input.Question,
                ChatHistory = chatHistory,
                Context = FilterContext(input.Context, [Suggestion, User, AppInfo])
            };
            var request = new SerializableHttpRequestMessage(HttpMethod.Post, $"{Environment.GetEnvironmentVariable(ChatBotPython)}/project-data", new Dictionary<string, string> { { Authorization, input.Token ?? "" } });
            request.Content = new StrContent(_common.SerializeToCamelCase(body), MediaType, Encoding);
            parallelTasks.Add((1, ProjectData, context.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                new HttpCallActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, SerializedRequest = request })));
        }

        /// <summary>
        /// Get backend responses from the parallel tasks
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="parallelTasks">parallelTasks</param>
        /// <returns>BackendReponse list</returns>
        private List<BackendResponse> GetBackendResponses(ILogger logger, List<(int Index, string Backend, Task<SerializableHttpResponseMessage> Task)> parallelTasks)
        {
            var answers = new List<BackendResponse>();
            foreach (var task in parallelTasks)
            {
                SerializableHttpResponseMessage result = task.Task.Result;
                string statusCode = result.StatusCode?.ToString() ?? "";

                BackendResponse backendResponse;
                try
                {
                    backendResponse = _common.Deserialize<BackendResponse>(result.Response);
                    backendResponse.Backend = task.Backend;
                    backendResponse.Status = statusCode;
                    backendResponse.CitingSources?.ForEach(f => f.SourceName = task.Backend);
                    backendResponse.FollowUpSuggestions?.Select(f => f.SuggestionText).ToList();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Result: {Result}", JsonSerializer.Serialize(result));
                    backendResponse = new BackendResponse
                    {
                        Backend = task.Backend,
                        Status = statusCode,
                        Response = result.ReasonPhrase ?? ""
                    };
                }

                if (!result.IsSuccessStatusCode)
                    logger.LogError("Error calling backend service: {Url}, Status code: {StatusCode}, Reason: {ReasonPhrase}", result.RequestUri, result.StatusCode, result.ReasonPhrase);

                answers.Add(backendResponse);
            }

            return answers;
        }

        /// <summary>
        /// If the status code is not success, throw an HttpRequestException
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="result">result</param>
        /// <exception cref="HttpRequestException"></exception>
        private static void IfNoSuccessStatusCodeThrowHttpRequestException(ILogger logger, SerializableHttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                logger.LogError("Error calling backend service: {Url}, Status code: {StatusCode}, Reason: {ReasonPhrase}", result.RequestUri, result.StatusCode, result.ReasonPhrase);
                throw new HttpRequestException($"Error calling backend service: {result.RequestUri}, Status code: {result.StatusCode}, Reason: {result.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Get EY Guidance which is the combination of EYIP and HelpCopilot
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="logger">logger</param>
        /// <param name="backendResponses">backendResponses</param>
        /// <param name="input">input</param>
        /// <returns>List BackendResponses</returns>
        private async Task<BackendResponse> GetEYGuidance(TaskOrchestrationContext context, ILogger logger, List<BackendResponse> backendResponses, ChatQuestion input)
        {
            List<string> backendsToCheck = new List<string> { HelpCopilot, EYIP };
            List<BackendResponse> successBackends = backendResponses.Where(w => backendsToCheck.Contains(w.Backend) && w.Status == Ok).ToList();

            string summarize = string.Empty;
            string status = string.Empty;
            if (successBackends.Count > 1)
            {
                string task = await GetSummarizationPromptAsync(context, successBackends, input);
                summarize = await GetSummarization(context, logger, input, task);
                status = Ok;
            }
            else if (successBackends.Count == 1)
            {
                summarize = successBackends[0].Response?.ToString() ?? "No response";
                status = Ok;
            }
            else if (successBackends.Count == 0)
            {
                summarize = "Backend responses fail";
                status = "";
            }

            return new BackendResponse { Backend = EYGuidance, Response = summarize, Status = status, CitingSources = null, Sql = null };
        }

        /// <summary>
        /// Get summarization prompt based on combination of backends
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="backendResponses">backendResponses</param>
        /// <param name="input">input</param>
        /// <returns>Summarization prompt</returns>
        private static async Task<string> GetSummarizationPromptAsync(TaskOrchestrationContext context, List<BackendResponse> backendResponses, ChatQuestion input)
        {
            var task = new StringBuilder();
            string prompt = string.Empty;

            // Add prompt based on the combination of backends
            var backends = backendResponses.Select(b => b.Backend).ToArray();

            // Add backend combination prompt
            if (backends.Contains(EYIP) && backends.Contains(HelpCopilot))
            {
                prompt = await context.CallActivityAsync<string>(nameof(ReadPromptFromFileActivity),
                    new ReadPromptFromFileActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, FileName = $"{EYGuidance}.txt" });
            }

            // Generic prompt for more than one source
            if (string.IsNullOrEmpty(prompt) && backends.Length > 1)
            {
                prompt = await context.CallActivityAsync<string>(nameof(ReadPromptFromFileActivity),
                    new ReadPromptFromFileActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, FileName = $"generic-prompt-n-sources.txt" });
            }
            // Generic prompt for one source
            else if (string.IsNullOrEmpty(prompt) && backends.Length == 1)
            {
                prompt = await context.CallActivityAsync<string>(nameof(ReadPromptFromFileActivity),
                    new ReadPromptFromFileActivityInput { InstanceId = context.InstanceId, ChatId = input.ChatId, ProjectFriendlyId = input.ProjectFriendlyId, FileName = $"generic-prompt-one-source.txt" });
            }

            task.AppendLine(prompt);

            // Add question
            task.AppendLine($@"Question: ""{input.Question}""");

            // Add sources responses
            for (int i = 0; i < backendResponses.Count; i++)
            {
                task.AppendLine($"{backendResponses[i].Backend}:");

                task.AppendLine(@$"         ''' {backendResponses[i].Response.ToString()} '''");
            }

            if (backendResponses.Count > 1)
                task.AppendLine($"Rewritten Response:");

            return task.ToString();
        }

        /// <summary>
        /// Get summarization of the texts provided, using the openai service
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="logger">logger</param>
        /// <param name="input"></param>
        /// <returns>Summarization string</returns>
        /// <param name="task">task</param>
        private async Task<string> GetSummarization(TaskOrchestrationContext context, ILogger logger, ChatQuestion input, string task)
        {
            try
            {
                return await context.CallActivityAsync<string>(nameof(SummarizeActivity),
                    new SummarizeActivityInput { 
                        InstanceId = context.InstanceId, 
                        ChatId = input.ChatId,
                        ProjectFriendlyId = input.ProjectFriendlyId,
                        Task = task });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Summarization error");
                throw new ChatOrchestratorException("Summarization error", ex);
            }
        }

        /// <summary>
        /// Format the answer with the summary and the backend responses
        /// </summary>
        /// <param name="backendResponses">backendResponses</param>
        /// <param name="summarize">summarize</param>
        /// <returns>answer</returns>
        private string GetDetailAnswer(List<BackendResponse> backendResponses, string summarize)
        {
            var str = new StringBuilder();
            if (backendResponses.Count > 1 && !string.IsNullOrWhiteSpace(summarize))
            {
                str.AppendLine($"Summary");
                str.AppendLine(summarize);
                str.AppendLine($"{Environment.NewLine}");
            }

            foreach (var backend in backendResponses)
            {
                str.AppendLine($"{sourceNamesFrontend[backend.Backend]}");
                str.AppendLine(backend.Response.ToString());
                str.AppendLine($"{Environment.NewLine}");
            }
            return str.ToString();
        }

        /// <summary>
        /// Return context filter by allowed keys provided
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="allowedKeys">allowedKeys</param>
        /// <returns>Filter context</returns>
        private static Dictionary<string, object> FilterContext(Dictionary<string, object?>? context, string[] allowedKeys)
        {
            var result = new Dictionary<string, object>();
            if (context is null)
                return result;

            context = context.Where(w => allowedKeys.Contains(w.Key) && w.Value != null).ToDictionary(k => k.Key, v => v.Value);

            return context.ToDictionary(k => k.Key, v => v.Value!);
        }

        /// <summary>
        /// Add sources to the response message from backends
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="backends"></param>
        /// <param name="message"></param>
        private void AddSourcesToMessage(ILogger logger, List<BackendResponse> backends, Message message)
        {
            bool projectDataReturnSql = bool.Parse(_common.GetEnvironmentVariable(logger, "ProjectDataReturnSql"));
            foreach (var backend in backends)
            {
                message.Response.Add(new SourceDto
                {
                    SourceName = backend.Backend,
                    Content = backend.Response.ToString(),
                    Status = backend.Status,
                    SqlQuery = projectDataReturnSql ? backend.Sql : null,
                    CitingSources = backend.CitingSources
                });
            }
        }
    }
}

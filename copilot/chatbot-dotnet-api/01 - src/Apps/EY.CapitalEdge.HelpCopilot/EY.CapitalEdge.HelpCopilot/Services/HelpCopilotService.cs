using EY.CapitalEdge.HelpCopilot.Models;
using EY.CapitalEdge.HelpCopilot.Models.Chat;
using EY.CapitalEdge.HelpCopilot.Models.ChatSensitiveDataSupport;
using EY.CapitalEdge.HelpCopilot.Static;
using System.Text.Json;

namespace EY.CapitalEdge.HelpCopilot.Services
{
    public class HelpCopilotService : IHelpCopilotService
    {
        private readonly ILogger<HelpCopilotService> _logger;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public HelpCopilotService(ILogger<HelpCopilotService> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri($"{configuration[SaTKnowledgeAssistant.BaseAddress]}");
        }

        public async Task<SessionResponse> SessionAsync(RequestContext context)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, SaTKnowledgeAssistant.Session);
                request.Headers.Add(Constants.Authorization, context.Token);
                request.Headers.Add(Constants.CEAuthHeader, Constants.CEAuthHeaderValue);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<SessionResponse>(responseStream, _jsonSerializerOptions);

                if (result is null)
                    return new SessionResponse();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the SessionAsync request.", context.InstanceId, context.ChatId, context.ProjectFriendlyId);
                throw new HelpCopilotServiceException($"An error occurred while processing the SessionAsync request for instanceId: {context.InstanceId}, chatId: {context.ChatId}, projectFriendlyId: {context.ProjectFriendlyId}", ex);
            }
        }

        public async Task<ChatResponse> ChatAsync(RequestContext context, ChatInput input)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, SaTKnowledgeAssistant.Chat);
                request.Headers.Add(Constants.Authorization, context.Token);
                request.Headers.Add(Constants.CEAuthHeader, Constants.CEAuthHeaderValue);

                request.Content = new StringContent(JsonSerializer.Serialize(input), null, ContentType.ApplicationJson);
                
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ChatResponse>(responseStream, _jsonSerializerOptions);

                if (result is null)
                    return new ChatResponse();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the ChatAsync request.", context.InstanceId, context.ChatId, context.ProjectFriendlyId);
                throw new HelpCopilotServiceException($"An error occurred while processing the ChatAsync request for instanceId: {context.InstanceId}, chatId: {context.ChatId}, projectFriendlyId: {context.ProjectFriendlyId}", ex);
            }
        }

        public async Task<ChatResponse> ChatSensitiveDataSupportAsync(RequestContext context, ChatSensitiveDataSupportInput input)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, SaTKnowledgeAssistant.ChatSensitiveDataSupport);
                request.Headers.Add(Constants.Authorization, context.Token);
                request.Headers.Add(Constants.CEAuthHeader, Constants.CEAuthHeaderValue);

                request.Content = new StringContent(JsonSerializer.Serialize(input), null, ContentType.ApplicationJson);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ChatResponse>(responseStream, _jsonSerializerOptions);

                if (result is null)
                    return new ChatResponse();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the ChatSensitiveDataSupportAsync request.", context.InstanceId, context.ChatId, context.ProjectFriendlyId);
                throw new HelpCopilotServiceException($"An error occurred while processing the ChatSensitiveDataSupportAsync request for instanceId: {context.InstanceId}, chatId: {context.ChatId}, projectFriendlyId: {context.ProjectFriendlyId}", ex);
            }
        }

        public async Task<ConversationResponse> StartConversationAsync(RequestContext context)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{SaTKnowledgeAssistant.StartConversation}?knowledgeAssistantTag=CE");
                request.Headers.Add(Constants.Authorization, context.Token);
                request.Headers.Add(Constants.CEAuthHeader, Constants.CEAuthHeaderValue);

                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ConversationResponse>(responseStream, _jsonSerializerOptions);

                if (result is null)
                    return new ConversationResponse() { ConversationReferenceId = string.Empty, WelcomeTextMessage = string.Empty };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the StartConversationAsync request.", context.InstanceId, context.ChatId, context.ProjectFriendlyId);
                throw new HelpCopilotServiceException($"An error occurred while processing the StartConversationAsync request for instanceId: {context.InstanceId}, chatId: {context.ChatId}, projectFriendlyId: {context.ProjectFriendlyId}", ex);
            }
        }
    }
}

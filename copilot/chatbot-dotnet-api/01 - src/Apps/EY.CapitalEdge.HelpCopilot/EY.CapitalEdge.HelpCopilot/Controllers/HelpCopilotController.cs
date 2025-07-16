using EY.CapitalEdge.HelpCopilot.Models;
using EY.CapitalEdge.HelpCopilot.Models.Chat;
using EY.CapitalEdge.HelpCopilot.Models.ChatSensitiveDataSupport;
using EY.CapitalEdge.HelpCopilot.Services;
using EY.CapitalEdge.HelpCopilot.Utils;
using EY.CapitalEdge.HelpCopilot.Utils.Models;
using Microsoft.AspNetCore.Mvc;
using Polly.Timeout;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EY.CapitalEdge.HelpCopilot.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v1/help-copilot")]
    public class HelpCopilotController : ControllerBase
    {
        private readonly ILogger<HelpCopilotController> _logger;
        private readonly IHelpCopilotService _helpCopilotService;
        private readonly ICommon _common;

        public HelpCopilotController(
            ILogger<HelpCopilotController> logger,
            IHelpCopilotService helpCopilotService,
            ICommon common)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _helpCopilotService = helpCopilotService ?? throw new ArgumentNullException(nameof(helpCopilotService));
            _common = common ?? throw new ArgumentNullException(nameof(common));
        }

        [HttpPost("user/session", Name = "Session")]
        public async Task<IActionResult> Session([FromHeader(Name = "Authorization")] string authenticationToken, [FromBody] BackendInput input)
        {
            if (!IsBackendInputValid(input)) return BadRequest();

            _logger.LogInformation("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] Session request received!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);

            if (!IsAuthHeaderValidFormat(authenticationToken, input)) return Unauthorized();

            if (!_common.ValidateToken(authenticationToken, input))
                return Unauthorized();

            try
            {
                var result = await _helpCopilotService.SessionAsync(GetRequestContext(input, authenticationToken));
                return new OkObjectResult(new BackendResponse() { Response = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the Session request.", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return GetStatusCode(ex);
            }
        }

        [HttpPost("chat/start", Name = "StartConversation")]
        public async Task<IActionResult> StartConversation([FromHeader(Name = "Authorization")] string authenticationToken, [FromBody] BackendInput input)
        {
            if (!IsBackendInputValid(input)) return BadRequest();

            _logger.LogInformation("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] StartConversation request received!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);

            if (!IsAuthHeaderValidFormat(authenticationToken, input)) return Unauthorized();

            if (!_common.ValidateToken(authenticationToken, input))
                return Unauthorized();

            try
            {
                var result = await _helpCopilotService.StartConversationAsync(GetRequestContext(input, authenticationToken));
                return new OkObjectResult(new BackendResponse() { Response = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the StartConversation request.", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return GetStatusCode(ex);
            }
        }

        [HttpPost("chat", Name = "Chat")]
        public async Task<IActionResult> Chat([FromHeader(Name = "Authorization")] string authenticationToken, [FromBody] BackendInput input)
        {
            if (!IsBackendInputValid(input)) return BadRequest();

            _logger.LogInformation("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] Chat request received!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);

            if (!IsAuthHeaderValidFormat(authenticationToken, input)) return Unauthorized();

            if (!_common.ValidateToken(authenticationToken, input))
                return Unauthorized();

            string? query = input.Question;
            if (string.IsNullOrWhiteSpace(query))
            {
                _logger.LogError("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] UserPrompt is not valid!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return BadRequest();
            }

            string? conversationReferenceId = _common.ExtractValueFromContext(input.Context, "conversationReferenceId");
            if (string.IsNullOrWhiteSpace(conversationReferenceId))
            {
                _logger.LogError("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] ConversationReferenceId is not valid!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return BadRequest();
            }

            string? projectDescription = _common.ExtractValueFromContext(input.Context, "projectDescription");
            if (!string.IsNullOrWhiteSpace(projectDescription))
            {
                query = $"Given the context: {projectDescription} \n {query}";
            }

            ChatInput chatInput = new()
            {
                Query = query,
                UseOpenai = true,
                ConversationReferenceId = conversationReferenceId,
                ResponseType = "Question"
            };
            try
            {
                ChatResponse result = await _helpCopilotService.ChatAsync(GetRequestContext(input, authenticationToken), chatInput);
                BackendResponse response = new()
                {
                    Response = result.Messages?.FirstOrDefault()?.MessageText ?? "HelpCopilot API did not return any data",
                    CitingSources = new List<CitingSource>()
                    {
                        new CitingSource()
                        {
                            SourceValue = result.Messages?.FirstOrDefault()?.Documents
                        }
                    },
                    RawReponse = result
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the Chat request.", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return GetStatusCode(ex);
            }
        }

        /// <summary>
        /// Wrapper endpoint for Chat API to support sensitive data, this endpoint will not store any data in the database
        /// </summary>
        /// <param name="authenticationToken">authenticationToken</param>
        /// <param name="input">input</param>
        /// <returns>Chat response</returns>
        [HttpPost("chat/sensitive-data-support", Name = "ChatSensitiveDataSupport")]
        public async Task<IActionResult> ChatSensitiveDataSupport([FromHeader(Name = "Authorization")] string authenticationToken, [FromBody] BackendInput input)
        {
            if (!IsBackendInputValid(input)) return BadRequest();

            _logger.LogInformation("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] ChatSensitiveDataSupport request received!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);

            if (!IsAuthHeaderValidFormat(authenticationToken, input)) return Unauthorized();

            if (!_common.ValidateToken(authenticationToken, input))
                return Unauthorized();

            string? query = input.Question;
            if (string.IsNullOrWhiteSpace(query))
            {
                _logger.LogError("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}]  UserPrompt is not valid!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return BadRequest();
            }

            string? conversationReferenceId = _common.ExtractValueFromContext(input.Context, "conversationReferenceId");
            if (string.IsNullOrWhiteSpace(conversationReferenceId))
            {
                _logger.LogError("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}]  ConversationReferenceId is not valid!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return BadRequest();
            }

            string? projectDescription = _common.ExtractValueFromContext(input.Context, "projectDescription");
            if (!string.IsNullOrWhiteSpace(projectDescription))
            {
                query = $"Given the context: {projectDescription} \n {query}";
            }

            ChatSensitiveDataSupportInput chatInput = new()
            {
                Query = query,
                ConversationReferenceId = conversationReferenceId,
                IsSensitiveInfo = true,
                ExternalQuestionId = input.InstanceId,
                ChatHistory = MapChatHistory(input.ChatHistory),
                ResponseType = "Question"
            };
            try
            {
                ChatResponse result = await _helpCopilotService.ChatSensitiveDataSupportAsync(GetRequestContext(input, authenticationToken), chatInput);
                BackendResponse response = new()
                {
                    Response = result.Messages?.FirstOrDefault()?.MessageText ?? "SAT Knowledge assistant API did not return any data",
                    CitingSources = new List<CitingSource>()
                    {
                        new CitingSource()
                        {
                            SourceValue = result.Messages?.FirstOrDefault()?.Documents
                        }
                    },
                    RawReponse = result
                };
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] An error occurred while processing the ChatSensitiveDataSupport request.", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return GetStatusCode(ex);
            }
        }

        [ExcludeFromCodeCoverage]
        private bool IsBackendInputValid(BackendInput input)
        {
            if (input.InstanceId.Length != 32 || !Guid.TryParseExact(input.InstanceId, "N", out _))
            {
                _logger.LogError("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] InstanceId is not valid!", input.ChatId, input.ChatId, input.ProjectFriendlyId);
                return false;
            }

            return true;
        }

        [ExcludeFromCodeCoverage]
        private bool IsAuthHeaderValidFormat(string authHeader, BackendInput input)
        {
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                _logger.LogError("[instanceId:{InstanceId}] [chatId:{ChatId}] [projectFriendlyId:{ProjectFriendlyId}] Authorization header is not valid!", input.InstanceId, input.ChatId, input.ProjectFriendlyId);
                return false;
            }

            return true;
        }

        [ExcludeFromCodeCoverage]
        private static RequestContext GetRequestContext(BackendInput input, string authenticationToken)
        {
            return new RequestContext
            {
                ChatId = input.ChatId,
                InstanceId = input.InstanceId,
                ProjectFriendlyId = input.ProjectFriendlyId,
                Token = authenticationToken
            };
        }

        [ExcludeFromCodeCoverage]
        private StatusCodeResult GetStatusCode(Exception ex)
        {
            if (ex is HttpRequestException)
            {
                HttpRequestException? httpEx = ex as HttpRequestException;
                return StatusCode(GetStatusCodeNumber(httpEx?.StatusCode));
            }
            else if (ex is TimeoutRejectedException)
            {
                return StatusCode(StatusCodes.Status408RequestTimeout);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [ExcludeFromCodeCoverage]
        private static int GetStatusCodeNumber(HttpStatusCode? statusCode)
        {
            return statusCode == null ? 500 : (int)statusCode;
        }

        /// <summary>
        /// Map the input to ChatHistory object
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>Mapped chat history to be send to sat knowledge assistant api</returns>
        [ExcludeFromCodeCoverage]
        private static ChatHistory[] MapChatHistory(List<Utils.Models.Message> chatHistory)
        {
            List<ChatHistory> result = [];
            for (int i = 1; i < chatHistory.Count; i += 2)
            {
                result.Add(new ChatHistory
                {
                    Question = chatHistory[i-1].Content,
                    Answers = [
                        new AnswerDto
                        {
                            Answer = chatHistory[i].Content,
                            Documents = []
                        }
                    ]
                });
            }

            return result.ToArray();
        }
    }
}

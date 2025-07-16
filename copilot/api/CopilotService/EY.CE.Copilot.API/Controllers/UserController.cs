using EY.CE.Copilot.API.Clients;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using static EY.CE.Copilot.API.Static.Constants;

namespace EY.CE.Copilot.API.Controllers
{
    [Authorize(Policy = AuthenticationPolicy.CE_USER_POLICY)]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string UserEmail;
        private readonly ISessionClient RedisClient;
        private readonly IDocumentsClient DocumentsClient;
        private readonly IPortalClient PortalClient;
        private readonly IChatClient _chatClient;

        public UserController(IHttpContextAccessor httpContextAccessor,
            ISessionClient redisClient,
            IDocumentsClient documentsClient,
            IPortalClient portalClient,
            IChatClient chatClient)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext.Items.TryGetValue(Constants.UserMail, out object? mail))
                UserEmail = mail.ToString();

            RedisClient = redisClient;
            DocumentsClient = documentsClient;
            PortalClient = portalClient;
            _chatClient = chatClient;
        }

        [HttpPost("chat/{key}")]
        public async Task<IActionResult> Post([FromBody] Conversation chat)
        {
            string key = chat.Key.ToString();
            string value = JsonSerializer.Serialize(chat.Content);

            try
            {
                string result = await RedisClient.SetRedisCache(key, value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetChat(string chatId)
        {
            try
            {
                string key = string.Format(Constants.Redis.Keys.MessageKey, UserEmail, chatId);

                string value = await _chatClient.GetDataFromCacheOrDatabase(key);
                Console.WriteLine($@"Chat | redis response | {value} ");
                if (value.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status200OK, null);
                }

                OutputConversation chat = new OutputConversation
                {
                    Key = key,
                    Content = JsonDocument.Parse(value)
                };
                return Ok(chat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("chat/{chatId}")]
        public async Task<IActionResult> DeleteChat(string chatId)
        {
            try
            {
                string key = string.Format(Constants.Redis.Keys.MessageKey, UserEmail, chatId);
                string result = await RedisClient.DeleteRedisCache(key);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("chat/{chatId}/{qCount}")]
        public async Task<IActionResult> GetChat(string chatId, int qCount)
        {
            try
            {
                string key = string.Format(Constants.Redis.Keys.MessageKey, UserEmail, chatId);

                string value = await _chatClient.GetDataFromCacheOrDatabase(key);
                Console.WriteLine($@"Chat | redis response | {value} ");

                if (!value.IsNullOrEmpty())
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Chat session not found");
                }

                var chat = JsonSerializer.Deserialize<List<Chat.Message>>(value);

                // get latest n*2 array from content array and if count is less than n*2 return all else return last n*2

                if (chat != null && chat.Count > qCount * 2)
                {
                    chat = chat.GetRange(chat.Count - qCount * 2, qCount * 2);
                }
                else
                {
                    chat = chat.GetRange(0, chat.Count);
                }

                var conversation = new Conversation
                {
                    Key = key,
                    Content = chat
                };
                return Ok(conversation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("projectdocs/details")]
        public async Task<IActionResult> ProjectDocsDetails(
            [FromQuery(Name = "filter")] string? filter,
            [FromBody] IEnumerable<string>? filterByDocIds )
        {
            try
            { 
                var projectDocs = await DocumentsClient.GetProjectDocs(new DocsRequest { Filter = filter, GeneratorType = GeneratorType.ChatRequest, User = UserEmail }, true);
                if (filterByDocIds != null && filterByDocIds.Any())
                {
                    projectDocs = projectDocs.Where(doc => filterByDocIds.Contains(doc.ID));
                }
                return Ok(projectDocs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("projectdocs")]
        public async Task<IActionResult> ProjectDocs(
            [FromQuery(Name = "filter")] string? filter
        )
        {
            try
            {
                var projectDocs = await DocumentsClient.GetProjectDocs(new DocsRequest { Filter = filter, GeneratorType = GeneratorType.ChatRequest, User = UserEmail }, true);
                return Ok(projectDocs.Select(doc => doc.ID));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("projectdocs")]
        public async Task<IActionResult> ClearProjectDocs()
        {
            try
            {
                string projectDocsKey = string.Format(Redis.Keys.ProjectDocs, UserEmail, GeneratorType.ChatRequest);
                string result = await RedisClient.DeleteRedisCache(projectDocsKey);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            try
            {
                PortalUser result = await PortalClient.GetUserDetails(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("apps")]
        public async Task<IActionResult> GetApps()
        {
            try
            {
                string token = Request.Headers[Constants.IDPAuthorizationHeader];
                var result = await PortalClient.GetApps(token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Return List of Assistant Documents list for which user has access to in the project
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onbehalfOfUser"> This should to be set to false if request is initiated from Program office Platform</param>
        /// <returns></returns>
        [Authorize(Policy = AuthenticationPolicy.CE_USER_OR_APISECRET)]
        [HttpPost("assistant-docs")]
        public async Task<IActionResult> GetAssistantDocs([FromBody] DocsRequest request, [FromQuery] bool onbehalfOfUser = false)
        {
            try
            {
                var result = await DocumentsClient.GetAssistantDocs(request, onbehalfOfUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

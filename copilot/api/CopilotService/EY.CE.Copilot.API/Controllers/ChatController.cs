using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EY.CE.Copilot.API.Controllers
{
    [Authorize(Policy = AuthenticationPolicy.CE_USER_POLICY)]
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatClient _chatClient;

        public ChatController(IChatClient client)
        {
            _chatClient = client;
        }

        /// <summary>
        /// Starts a new chat session.
        /// </summary>
        /// <returns>The response from the chat client.</returns>
        [HttpGet("start")]
        public async Task<IActionResult> StartChat()
        {
            try
            {
                var response = await _chatClient.StartChat();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return GetExceptionContentResult(ex);
            }
        }

        /// <summary>
        /// Posts a question to the chat session.
        /// </summary>
        /// <param name="question">The question to be posted.</param>
        /// <returns>The response from the chat client.</returns>
        [HttpPost]
        public async Task<IActionResult> PostQuestion([FromBody] Chat.Input question)
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"];
                string token = authorizationHeader.ToString().Replace("Bearer ", "");
                var response = await _chatClient.PostChat(token, question);

                return response;

            }
            catch (Exception ex)
            {
                return GetExceptionContentResult(ex);
            }
        }

        /// <summary>
        /// Gets the status of the chat session.
        /// </summary>
        /// <param name="chatid">The ID of the chat session.</param>
        /// <param name="endpoint">The endpoint of the chat session.</param>
        /// <returns>The response from the chat client.</returns>
        [HttpPost("status/{chatid}")]
        public async Task<IActionResult> GetChatStatus(string chatid, [FromBody] Chat.Status input)
        {
            try
            {
                var authorizationHeader = Request.Headers["Authorization"];
                string token = authorizationHeader.ToString().Replace("Bearer ", "");
                var response = await _chatClient.GetStatus(token, input, chatid);
                return response;
            }
            catch (Exception ex)
            {
                return GetExceptionContentResult(ex);
            }
        }

        /// <summary>
        /// Posts feedback for a chat message.
        /// </summary>
        /// <param name="feedback">The feedback to be posted.</param>
        /// <returns>The response from the chat client.</returns>
        [HttpPost("feedback")]
        public async Task<IActionResult> PostFeedback([FromBody] Chat.MessageFeedback feedback)
        {
            try
            {
                await _chatClient.PostFeedback(feedback);
                return Ok();
            }
            catch (Exception ex)
            {
                return GetExceptionContentResult(ex);
            }
        }

        /// <summary>
        /// Posts feedback for the Copilot functionality.
        /// </summary>
        /// <param name="feedback">The feedback to be posted.</param>
        /// <returns>The response from the chat client.</returns>
        [HttpPost("copilot-feedback")]
        public async Task<IActionResult> PostCopilotFeedback([FromBody] Chat.CopilotFeedback feedback)
        {
            try
            {
                await _chatClient.PostCopilotFeedback(feedback);
                return Ok();
            }
            catch (Exception ex)
            {
                return GetExceptionContentResult(ex);
            }
        }

        /// <summary>
        /// Exports the chat history as a file.
        /// </summary>
        /// <param name="chatId">The ID of the chat session.</param>
        /// <param name="messageId">The ID of the message.</param>
        /// <param name="fileType">The type of the file to export (PDF, WORD).</param>
        /// <returns>The exported file.</returns>
        [HttpGet("{chatId}/messages/{messageId}/history/export/{fileType}")]
        public async Task<ActionResult> ExportChatHistory(string chatId, int messageId, string fileType)
        {
            try
            {
                return await _chatClient.GetExportFile(chatId, messageId, fileType);
            }
            catch (Exception ex)
            {
                return GetExceptionContentResult(ex);
            }
        }

        private static ContentResult GetExceptionContentResult(Exception ex)
        {
            return new ContentResult
            {
                Content = ex.Message,
                ContentType = Constants.ContentType.TextPlain,
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
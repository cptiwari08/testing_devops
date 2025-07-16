using EY.CapitalEdge.HelpCopilot.Controllers;
using EY.CapitalEdge.HelpCopilot.Models;
using EY.CapitalEdge.HelpCopilot.Models.Chat;
using EY.CapitalEdge.HelpCopilot.Models.ChatSensitiveDataSupport;
using EY.CapitalEdge.HelpCopilot.Services;
using EY.CapitalEdge.HelpCopilot.Utils;
using EY.CapitalEdge.HelpCopilot.Utils.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EY.CapitalEdge.HelpCopilot.Tests
{
    public class HelpCopilotControllerTests
    {
        private readonly Mock<ILogger<HelpCopilotController>> _logger = new Mock<ILogger<HelpCopilotController>>();
        private readonly Mock<IHelpCopilotService> _helpCopilotServiceMock = new Mock<IHelpCopilotService>();
        private readonly Mock<ICommon> _commonMock = new Mock<ICommon>();

        [Fact]
        public async Task Session_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer xyz";
            _helpCopilotServiceMock.Setup(s => s.SessionAsync(It.IsAny<RequestContext>())).ReturnsAsync(new SessionResponse());
            _commonMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<BackendInput>())).Returns(true);
            BackendInput input = new BackendInput() { InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), ProjectFriendlyId = "ai-ceassistbench" };
            string authenticationToken = "Bearer xyz";

            // Act
            var result = await controller.Session(authenticationToken, input) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Session_WhenCalled_ReturnUnauthorized(string? authHeader)
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = authHeader;
            BackendInput input = new BackendInput() { InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), ProjectFriendlyId = "ai-ceassistbench" };

            // Act
            var result = await controller.Session(It.IsAny<string>(), input) as UnauthorizedResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task StartConversation_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer xyz";
            _helpCopilotServiceMock.Setup(s => s.StartConversationAsync(It.IsAny<RequestContext>())).ReturnsAsync(
                new ConversationResponse() { ConversationReferenceId = "35b22a3c-6bb3-4faa-8923-533754c36be2", WelcomeTextMessage = "Hello!" });
            _commonMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<BackendInput>())).Returns(true);
            BackendInput input = new BackendInput() { InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), ProjectFriendlyId = "ai-ceassistbench" };
            string authenticationToken = "Bearer xyz";

            // Act
            var result = await controller.StartConversation(authenticationToken, input) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task StartConversation_WhenCalled_ReturnUnauthorized(string? authHeader)
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = authHeader;
            BackendInput input = new BackendInput() { InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), ProjectFriendlyId = "ai-ceassistbench" };

            // Act
            var result = await controller.StartConversation(It.IsAny<string>(), input) as UnauthorizedResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task Chat_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer xyz";
            var chatResponse = new ChatResponse()
            {
                IsSuccess = true,
                Messages = [ 
                    new Models.Chat.Message()
                    {
                        MessageText = "HelpCopilot test response"
                    } 
                ]
            };
            _helpCopilotServiceMock.Setup(s => s.ChatAsync(It.IsAny<RequestContext>(), It.IsAny<ChatInput>())).ReturnsAsync(chatResponse);
            _commonMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<BackendInput>())).Returns(true);
            _commonMock.Setup(s => s.ExtractValueFromContext(It.IsAny<IDictionary<string, object>>(), It.IsAny<string>())).Returns("eb9995f6-99c9-4908-935a-b052705b71a9");
            BackendInput input = new BackendInput()
            {
                InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b",
                ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "Do i have to authenticate every time i use a project which has MFA enabled on it",
                Context = new Dictionary<string, object>() { { "conversationReferenceId", "eb9995f6-99c9-4908-935a-b052705b71a9" }, { "projectDescription", "Project description" } }
            };
            string authenticationToken = "Bearer xyz";

            // Act
            var result = await controller.Chat(authenticationToken, input) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Chat_WhenCalled_ReturnUnauthorized(string? authHeader)
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = authHeader;
            BackendInput input = new BackendInput()
            {
                InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b",
                ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "Do i have to authenticate every time i use a project which has MFA enabled on it",
                Context = new Dictionary<string, object>() { { "conversationReferenceId", "eb9995f6-99c9-4908-935a-b052705b71a9" } }
            };

            // Act
            var result = await controller.Chat(It.IsAny<string>(), input) as UnauthorizedResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task ChatSensitiveDataSupport_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer xyz";
            var chatResponse = new ChatResponse()
            {
                IsSuccess = true,
                Messages = [
                    new Models.Chat.Message()
                    {
                        MessageText = "HelpCopilot test response"
                    }
                ]
            };
            _helpCopilotServiceMock.Setup(s => s.ChatSensitiveDataSupportAsync(It.IsAny<RequestContext>(), It.IsAny<ChatSensitiveDataSupportInput>())).ReturnsAsync(chatResponse);
            _commonMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<BackendInput>())).Returns(true);
            _commonMock.Setup(s => s.ExtractValueFromContext(It.IsAny<IDictionary<string, object>>(), It.IsAny<string>())).Returns("eb9995f6-99c9-4908-935a-b052705b71a9");
            BackendInput input = new BackendInput()
            {
                InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b",
                ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "Do i have to authenticate every time i use a project which has MFA enabled on it",
                Context = new Dictionary<string, object>() { { "conversationReferenceId", "eb9995f6-99c9-4908-935a-b052705b71a9" }, { "projectDescription", "Project description" } }
            };
            string authenticationToken = "Bearer xyz";

            // Act
            var result = await controller.ChatSensitiveDataSupport(authenticationToken, input) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task ChatSensitiveDataSupport_WhenCalled_ReturnUnauthorized(string? authHeader)
        {
            // Arrange
            var controller = new HelpCopilotController(_logger.Object, _helpCopilotServiceMock.Object, _commonMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = authHeader;
            BackendInput input = new BackendInput()
            {
                InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b",
                ChatId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "Do i have to authenticate every time i use a project which has MFA enabled on it",
                Context = new Dictionary<string, object>() { { "conversationReferenceId", "eb9995f6-99c9-4908-935a-b052705b71a9" } }
            };

            // Act
            var result = await controller.ChatSensitiveDataSupport(It.IsAny<string>(), input) as UnauthorizedResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }
    }
}
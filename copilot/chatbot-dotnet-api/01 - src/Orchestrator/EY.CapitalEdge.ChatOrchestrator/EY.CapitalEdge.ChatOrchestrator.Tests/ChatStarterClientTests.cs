using Moq;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Text;
using EY.CapitalEdge.ChatOrchestrator.Clients;
using System.Net;
using Microsoft.Extensions.Logging;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Tests.Fakes;
using System.Text.Json;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Services;

namespace EY.CapitalEdge.ChatOrchestrator.Tests
{
    public class ChatStarterClientTests
    {
        [Fact]
        public async Task Chat_WithoutAuthorizationToken_ThrowUnauthorized()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes(""));
            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            var mockCommon = new Mock<ICommon>();

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task Chat_WithInvalidAuthorizationToken_ThrowUnauthorized()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes(""));
            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<Common> { CallBase = true };
            mockCommon.Setup(c => c.GetEnvironmentVariable(It.IsAny<ILogger>(), "Issuer")).Returns("https://eycapitaledge-dev.ey.com");
            mockCommon.Setup(c => c.GetEnvironmentVariable(It.IsAny<ILogger>(), "PublicKey")).Returns("VGVzdCBwdWJsaWMga2V5");

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task Chat_WithoutBody_BadRequest()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes(""));
            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            result.Body.Position = 0;
            using var reader = new StreamReader(result.Body);
            var message = await reader.ReadToEndAsync();
            Assert.Equal("Body was not provided", message);
        }

        [Fact]
        public async Task Chat_WithInvalidBodyFormat_BadRequest()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes("{"));
            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Chat_WithNullChatQuestionInput_BadRequest()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes("null"));
            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            result.Body.Position = 0;
            using var reader = new StreamReader(result.Body);
            var message = await reader.ReadToEndAsync();
            Assert.Equal("Body format is not valid", message);
        }

        [Fact]
        public async Task Chat_WithMessageId0_BadRequest()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"messageId\":0,\"chatId\":\"89da0749-e1af-40ff-9bc5-11d7a5c54164\",\"question\":\"How to export report to PPT?\",\"sources\":[\"internet\"],\"context\":{}}"));
            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            result.Body.Position = 0;
            using var reader = new StreamReader(result.Body);
            var message = await reader.ReadToEndAsync();
            Assert.Equal("messageId should be greater than 0", message);
        }

        [Fact]
        public async Task Chat_WithEmptyChatId_BadRequest()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"messageId\":1,\"chatId\":\"00000000-0000-0000-0000-000000000000\",\"question\":\"How to export report to PPT?\",\"sources\":[\"internet\"],\"context\":{}}"));
            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            result.Body.Position = 0;
            using var reader = new StreamReader(result.Body);
            var message = await reader.ReadToEndAsync();
            Assert.Equal("chatId is not a valid Guid", message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("")]
        public async Task Chat_WithQuestionIsNullOrWhiteSpace_BadRequest(string question)
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"messageId\":1,\"chatId\":\"89da0749-e1af-40ff-9bc5-11d7a5c54164\",\"question\":\"" + question + "\",\"sources\":[\"internet\"],\"context\":{}}"));

            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            result.Body.Position = 0;
            using var reader = new StreamReader(result.Body);
            var message = await reader.ReadToEndAsync();
            Assert.Equal("No question provided", message);
        }

        [Theory]
        [InlineData("internet", "invalid-source")]
        [InlineData("invalid-source")]
        public async Task Chat_WithInvalidSources_BadRequest(params string[] sources)
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var sourcesJson = JsonSerializer.Serialize(sources);
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"messageId\":1,\"chatId\":\"89da0749-e1af-40ff-9bc5-11d7a5c54164\",\"question\":\"How to export report to PPT?\",\"sources\":" + sourcesJson + ",\"context\":{}}"));

            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            result.Body.Position = 0;
            using var reader = new StreamReader(result.Body);
            var message = await reader.ReadToEndAsync();
            Assert.Contains("Invalid source provided, the valid ones are", message);
        }

        [Theory]
        [InlineData()]
        [InlineData("internet", "project-data", "project-docs")]
        public async Task Chat_WithInvalidNumberOfSources_BadRequest(params string[] sources)
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var sourcesJson = JsonSerializer.Serialize(sources);
            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"messageId\":1,\"chatId\":\"89da0749-e1af-40ff-9bc5-11d7a5c54164\",\"question\":\"How to export report to PPT?\",\"sources\":" + sourcesJson + ",\"context\":{}}"));

            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            result.Body.Position = 0;
            using var reader = new StreamReader(result.Body);
            var message = await reader.ReadToEndAsync();
            Assert.Contains("Sources provided must be 1 or 2", message);
        }

        [Fact]
        public async Task Chat_WhenTokenSourceNotMatchInputSource_Forbidden()
        {
            // Arrange
            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"messageId\":1,\"chatId\":\"89da0749-e1af-40ff-9bc5-11d7a5c54164\",\"question\":\"How to export report to PPT?\",\"sources\":[\"internet\"],\"context\":{}}"));

            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            mockCommon.Setup(c => c.GetTokenData(It.IsAny<ILogger>(), It.IsAny<string>())).Returns(
                new Token { Scope = new List<string> { "project-data" }}
            );

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task Chat_WithValidInput_StartsOrchestration()
        {
            // Arrange
            var mockClient = new Mock<FakeDurableTaskClient>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "Chat")).Returns(mockLogger.Object);

            var body = new MemoryStream(Encoding.ASCII.GetBytes("{\"messageId\":1,\"chatId\":\"89da0749-e1af-40ff-9bc5-11d7a5c54164\",\"question\":\"How to export report to PPT?\",\"sources\":[\"internet\"],\"context\":{}}"));

            var mockContext = new Mock<FunctionContext>();
            var request = new FakeHttpRequestData(mockContext.Object, new Uri("https://api-manager/api/chat"), body);
            request.Headers.Add("Authorization", "Bearer test");
            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.ValidateToken(It.IsAny<ILogger>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            mockCommon.Setup(c => c.GetTokenData(It.IsAny<ILogger>(), It.IsAny<string>())).Returns(
                new Token { Scope = new List<string> { "internet" }, ProjectFriendlyId = "ai-ceassistbench" }
            );

            var mockClientWrapper = new Mock<IDurableTaskClientWrapper>();
            mockClientWrapper.Setup(c => c.CreateCheckStatusResponseAsync(It.IsAny<HttpRequestData>(), It.IsAny<string>(), It.IsAny<HttpStatusCode>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FakeHttpResponseData(mockContext.Object));

            // Act
            var chatStarterClient = new ChatStarterClient(mockLoggerProvider.Object, mockCommon.Object, mockClientWrapper.Object);
            var result = await chatStarterClient.Chat(request, mockClient.Object, mockContext.Object);

            // Assert
            Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
            mockClientWrapper.Verify(c => c.CreateCheckStatusResponseAsync(It.IsAny<HttpRequestData>(), It.IsAny<string>(), It.IsAny<HttpStatusCode>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
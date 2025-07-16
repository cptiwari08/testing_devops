using EY.CapitalEdge.ChatOrchestrator.Activities;
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace EY.CapitalEdge.ChatOrchestrator.Tests
{
    public class HttpCallActivityTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new Mock<IHttpClientFactory>();

        [Fact]
        public async Task Run_ValidRequest_ReturnsSerializableHttpResponseMessage()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "HttpCallActivity")).Returns(mockLogger.Object);
            var mockFunctionContext = new Mock<FunctionContext>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"backend\":\"internet\",\"sql\":null,\"response\":\"To export a report to PowerPoint (PPT)....\",\"citingSources\":[{\"sourceName\":\"internet\",\"sourceType\":\"internet\",\"sourceValue\":[]}],\"rawResponse\":{\"response\":\"Raw response\",\"sources\":[],\"source_nodes\":[]}}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            string instanceId = "05cecd8a143545bc9d05cb6dd69ada1c";
            Guid chatId = Guid.NewGuid();
            var serializedRequest = new SerializableHttpRequestMessage(HttpMethod.Post, $"http://test.com", new Dictionary<string, string> { { "Authorization", "Bearer xyz" } });
            serializedRequest.Content = new StrContent(JsonSerializer.Serialize(new BackendInput
            {
                ChatId = chatId,
                InstanceId = instanceId,
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "How to export report to PPT?",
                Context = []
            }), "application/json", "UTF8");

            var underTest = new HttpCallActivity(_httpClientFactoryMock.Object, mockLoggerProvider.Object);

            // Act
            var result = await underTest.Run(
                new HttpCallActivityInput { InstanceId = instanceId, ChatId = chatId, SerializedRequest = serializedRequest }, mockFunctionContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Run_WhenException_ReturnsSerializableHttpResponseMessageWithUnsuccessfulStatus()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "HttpCallActivity")).Returns(mockLogger.Object);
            var mockFunctionContext = new Mock<FunctionContext>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException());

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            string instanceId = "05cecd8a143545bc9d05cb6dd69ada1c";
            Guid chatId = Guid.NewGuid();
            var serializedRequest = new SerializableHttpRequestMessage(HttpMethod.Post, $"http://test.com", new Dictionary<string, string> { { "Authorization", "Bearer xyz" } });
            serializedRequest.Content = new StrContent(JsonSerializer.Serialize(new BackendInput
            {
                ChatId = chatId,
                InstanceId = instanceId,
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "How to export report to PPT?",
                Context = []
            }), "application/json", "UTF8");

            var underTest = new HttpCallActivity(_httpClientFactoryMock.Object, mockLoggerProvider.Object);

            // Act
            var result = await underTest.Run(
                new HttpCallActivityInput { InstanceId = instanceId, ChatId = chatId, SerializedRequest = serializedRequest }, mockFunctionContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccessStatusCode);
        }
    }
}

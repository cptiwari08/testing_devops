using EY.CapitalEdge.HelpCopilot.Models;
using EY.CapitalEdge.HelpCopilot.Models.Chat;
using EY.CapitalEdge.HelpCopilot.Models.ChatSensitiveDataSupport;
using EY.CapitalEdge.HelpCopilot.Services;
using EY.CapitalEdge.HelpCopilot.Static;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;

namespace EY.CapitalEdge.HelpCopilot.Tests
{
    public class HelpCopilotServiceTests
    {
        private readonly Mock<ILogger<HelpCopilotService>> _logger = new Mock<ILogger<HelpCopilotService>>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new Mock<IHttpClientFactory>();

        public HelpCopilotServiceTests()
        {
            _configuration.Setup(x => x[SaTKnowledgeAssistant.BaseAddress]).Returns("https://eycapitaledge-dev.ey.com");
        }

        [Fact]
        public async Task SessionAsync_WhenCalled_ReturnsSessionResponse()
        {
            // Arrange
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
                    Content = new StringContent(@"{""isSuccess"":true,""userRoles"":[""Client""],""guid"":""1b8b3c2d-8274-47c1-9c07-524591c4b721"",""emailId"":""Daniel.Rivera@ey.com"",""displayName"":""Daniel Rivera""}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);

            // Act
            var result = await service.SessionAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz"});

            //Assert
            Assert.NotNull(result);
            Assert.IsType<SessionResponse>(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task SessionAsync_WhenCalled_ReturnsHttpRequestException(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);

            // Act and Assert
            await Assert.ThrowsAsync<HelpCopilotServiceException>(() => service.SessionAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz" }));
        }

        [Fact]
        public async Task StartConversationAsync_WhenCalled_ReturnsSessionResponse()
        {
            // Arrange
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
                    Content = new StringContent(@"{""conversationReferenceId"":""4fb202d2-68f6-4bac-ae0d-c1b2634392b3"",""welcomeTextMessage"":""Hello! I'm here to assist you with any acquisition and divestiture queries related to Capital Edge you might have.""}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);

            // Act
            var result = await service.StartConversationAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz" });

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ConversationResponse>(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task StartConversationAsync_WhenCalled_ReturnsHttpRequestException(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);

            // Act and Assert
            await Assert.ThrowsAsync<HelpCopilotServiceException>(() => service.StartConversationAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz" }));
        }

        [Fact]
        public async Task ChatAsync_WhenCalled_ReturnsSessionResponse()
        {
            // Arrange
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
                    Content = new StringContent(@"{""isSuccess"":false,""messages"":[{""conversationGuid"":""37e533b7-71a6-4e96-ba55-ca8e5357d06b"",""conversationStatusId"":2,""messageGuid"":""98b2061d-bd52-4fd7-8a02-448a0c589c74"",""messageTypeId"":1,""messageText"":""Hello! How can I assist you today?\n Related Video : <a target=\""_blank\"" href=\""https://eygb.sharepoint.com/sites/CapitalEdge/_layouts/15/stream.aspx?id=%2Fsites%2FCapitalEdge%2FShared%20Documents%2FOP%20model%20docs%2FEY%2DP%20Skill%20Builder%20%2D%20Capital%20Edge%20%2D%20Operating%20Model%20App%2002%2D24%2D2023%2Emp4&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview\"">EY-P Skill Builder - Capital Edge - Operating Model App 02-24-2023-en-US</a>"",""messageCreatedDate"":""2024-02-22T16:17:09.282343Z"",""user"":{""guid"":""1b8b3c2d-8274-47c1-9c07-524591c4b721"",""displayName"":""Daniel Rivera"",""emailId"":""Daniel.Rivera@ey.com"",""createdDate"":""0001-01-01T00:00:00"",""userIdentity"":null,""roles"":null},""additionalInfo"":null,""isMessageLiked"":null,""score"":77.1,""showFeedbackOptions"":true,""showTypingEffect"":true,""chatResponseGuid"":""98b2061d-bd52-4fd7-8a02-448a0c589c74"",""currentResponseCount"":1,""totalResponseCount"":1,""documents"":[{""documentGuid"":""64fd61be-0aa3-4926-8c92-197e1d614966"",""documentName"":""EY-P Skill Builder - Capital Edge - Operating Model App 02-24-2023-en-US.pdf"",""pages"":[{""pageNumber"":19,""metaData"":null}]}]}],""status"":0,""code"":0,""message"":null}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);
            ChatInput input = new ChatInput() { ConversationReferenceId = "37e533b7-71a6-4e96-ba55-ca8e5357d06b", Query = "Hello! How can I assist you today?", ResponseType = "Question", UseOpenai = true };

            // Act
            var result = await service.ChatAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz" }, input);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ChatResponse>(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task ChatAsync_WhenCalled_ReturnsHttpRequestException(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);
            ChatInput input = new ChatInput() { ConversationReferenceId = "37e533b7-71a6-4e96-ba55-ca8e5357d06b", Query = "Hello! How can I assist you today?", ResponseType = "Question", UseOpenai = true };

            // Act and Assert
            await Assert.ThrowsAsync<HelpCopilotServiceException>(() => service.ChatAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz" }, input));
        }

        [Fact]
        public async Task ChatSensitiveDataSupportAsync_WhenCalled_ReturnsSessionResponse()
        {
            // Arrange
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
                    Content = new StringContent(@"{""isSuccess"":false,""messages"":[{""conversationGuid"":""37e533b7-71a6-4e96-ba55-ca8e5357d06b"",""conversationStatusId"":2,""messageGuid"":""98b2061d-bd52-4fd7-8a02-448a0c589c74"",""messageTypeId"":1,""messageText"":""Hello! How can I assist you today?\n Related Video : <a target=\""_blank\"" href=\""https://eygb.sharepoint.com/sites/CapitalEdge/_layouts/15/stream.aspx?id=%2Fsites%2FCapitalEdge%2FShared%20Documents%2FOP%20model%20docs%2FEY%2DP%20Skill%20Builder%20%2D%20Capital%20Edge%20%2D%20Operating%20Model%20App%2002%2D24%2D2023%2Emp4&referrer=StreamWebApp%2EWeb&referrerScenario=AddressBarCopied%2Eview\"">EY-P Skill Builder - Capital Edge - Operating Model App 02-24-2023-en-US</a>"",""messageCreatedDate"":""2024-02-22T16:17:09.282343Z"",""user"":{""guid"":""1b8b3c2d-8274-47c1-9c07-524591c4b721"",""displayName"":""Daniel Rivera"",""emailId"":""Daniel.Rivera@ey.com"",""createdDate"":""0001-01-01T00:00:00"",""userIdentity"":null,""roles"":null},""additionalInfo"":null,""isMessageLiked"":null,""score"":77.1,""showFeedbackOptions"":true,""showTypingEffect"":true,""chatResponseGuid"":""98b2061d-bd52-4fd7-8a02-448a0c589c74"",""currentResponseCount"":1,""totalResponseCount"":1,""documents"":[{""documentGuid"":""64fd61be-0aa3-4926-8c92-197e1d614966"",""documentName"":""EY-P Skill Builder - Capital Edge - Operating Model App 02-24-2023-en-US.pdf"",""pages"":[{""pageNumber"":19,""metaData"":null}]}]}],""status"":0,""code"":0,""message"":null}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);
            ChatSensitiveDataSupportInput input = new ChatSensitiveDataSupportInput() { ConversationReferenceId = "37e533b7-71a6-4e96-ba55-ca8e5357d06b", Query = "Hello! How can I assist you today?", ResponseType = "Question", IsSensitiveInfo = true, ExternalQuestionId = "86fe51fe7efa4b7eb839b236e3b05b7b", ChatHistory = [] };

            // Act
            var result = await service.ChatSensitiveDataSupportAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz" }, input);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ChatResponse>(result);
        }

        [Theory]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task ChatSensitiveDataSupportAsync_WhenCalled_ReturnsHttpRequestException(HttpStatusCode httpStatusCode)
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new HelpCopilotService(_logger.Object, httpClient, _configuration.Object);
            ChatSensitiveDataSupportInput input = new ChatSensitiveDataSupportInput() { ConversationReferenceId = "37e533b7-71a6-4e96-ba55-ca8e5357d06b", Query = "Hello! How can I assist you today?", ResponseType = "Question", IsSensitiveInfo = true, ExternalQuestionId = "86fe51fe7efa4b7eb839b236e3b05b7b", ChatHistory = [] };

            // Act and Assert
            await Assert.ThrowsAsync<HelpCopilotServiceException>(() => service.ChatSensitiveDataSupportAsync(new RequestContext { ChatId = Guid.Parse("86fe51fe7efa4b7eb839b236e3b05b7b"), InstanceId = "86fe51fe7efa4b7eb839b236e3b05b7b", ProjectFriendlyId = "ai-ceassistbench", Token = "Bearer xyz" }, input));
        }
    }
}
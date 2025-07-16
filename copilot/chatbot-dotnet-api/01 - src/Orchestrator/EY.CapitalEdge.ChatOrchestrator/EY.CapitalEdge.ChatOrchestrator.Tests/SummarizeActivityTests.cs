using EY.CapitalEdge.ChatOrchestrator.Activities;
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OpenAI;
using OpenAI.Interfaces;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace EY.CapitalEdge.ChatOrchestrator.Tests
{
    public class SummarizeActivityTests
    {
        [Fact]
        public async Task Run_ValidTask_ReturnsSummarization()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "SummarizeActivity")).Returns(mockLogger.Object);

            var mockFunctionContext = new Mock<FunctionContext>();
            var mockOpenAIServiceFactory = new Mock<IOpenAIServiceFactory>();
            var mockOpenAIService = new Mock<IOpenAIService>();
            mockOpenAIServiceFactory.Setup(f => f.GetService("GenericOpenAIService")).Returns(mockOpenAIService.Object);

            var mockStopwatch = new Mock<IStopwatch>();

            var activity = new SummarizeActivity(mockOpenAIServiceFactory.Object, mockLoggerProvider.Object, mockStopwatch.Object);

            string testTask = "Prompt example";
            string expectedSummarization = "testSummarization";

            mockOpenAIService.Setup(s => s.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), default, default))
                .ReturnsAsync(new ChatCompletionCreateResponse()
                {
                    Choices = new List<ChatChoiceResponse>(){
                        new ChatChoiceResponse
                        {
                            Message = new ChatMessage
                            {
                                Content = expectedSummarization
                            }
                        }
                    },
                    Usage = new UsageResponse()
                    {
                        CompletionTokens = 1,
                        PromptTokens = 1,
                        TotalTokens = 2                        
                    }
                });

            // Act
            var result = await activity.Run(
                new SummarizeActivityInput { InstanceId = "05cecd8a143545bc9d05cb6dd69ada1c", ChatId = Guid.NewGuid(), Task = testTask }, mockFunctionContext.Object);

            // Assert
            Assert.Equal(expectedSummarization, result);
        }

        [Fact]
        public async Task Run_WhenExceptionOccurs_ReturnsErrorMessage()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "SummarizeActivity")).Returns(mockLogger.Object);

            var mockFunctionContext = new Mock<FunctionContext>();
            var mockOpenAIServiceFactory = new Mock<IOpenAIServiceFactory>();
            var mockOpenAIService = new Mock<IOpenAIService>();
            mockOpenAIServiceFactory.Setup(f => f.GetService("GenericOpenAIService")).Returns(mockOpenAIService.Object);

            var mockStopwatch = new Mock<IStopwatch>();

            var activity = new SummarizeActivity(mockOpenAIServiceFactory.Object, mockLoggerProvider.Object, mockStopwatch.Object);

            string testTask = "Prompt example";
            string errorMessage = "An error occurred during the summarization process. Unable to generate a summary for the provided task.";

            mockOpenAIService.Setup(s => s.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), default, default))
                .ThrowsAsync(new Exception());

            // Act
            var result = await activity.Run(
                new SummarizeActivityInput { InstanceId = "05cecd8a143545bc9d05cb6dd69ada1c", ChatId = Guid.NewGuid(), Task = testTask }, mockFunctionContext.Object);

            // Assert
            Assert.Equal(errorMessage, result);
        }
    }
}

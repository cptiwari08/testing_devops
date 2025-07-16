using EY.CapitalEdge.ChatOrchestrator.Activities;
using EY.CapitalEdge.ChatOrchestrator.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;

namespace EY.CapitalEdge.ChatOrchestrator.Tests
{
    public class ReadPromptFromFileActivityTests
    {
        [Fact]
        public async Task Run_ValidFileName_ReturnsFileContent()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "ReadPromptFromFileActivity")).Returns(mockLogger.Object);
            var mockFunctionContext = new Mock<FunctionContext>();
            var activity = new ReadPromptFromFileActivity(mockLoggerProvider.Object);

            string testFileName = "test-file.txt";

            // Act
            var result = await activity.Run(
                new ReadPromptFromFileActivityInput { InstanceId = "05cecd8a143545bc9d05cb6dd69ada1c", ChatId = Guid.NewGuid(), FileName = testFileName }, mockFunctionContext.Object);

            // Assert
            Assert.Contains("Test content", result);
        }

        [Fact]
        public async Task Run_WhenException_ReturnsEmpty()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "ReadPromptFromFileActivity")).Returns(mockLogger.Object);
            var mockFunctionContext = new Mock<FunctionContext>();
            var activity = new ReadPromptFromFileActivity(mockLoggerProvider.Object);

            string testFileName = "non-existent-file.txt";

            // Act
            var result = await activity.Run(
                new ReadPromptFromFileActivityInput { InstanceId = "05cecd8a143545bc9d05cb6dd69ada1c", ChatId = Guid.NewGuid(), FileName = testFileName }, mockFunctionContext.Object);

            // Assert
            Assert.Contains("", result);
        }
    }
}

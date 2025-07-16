using Azure;
using Azure.Security.KeyVault.Secrets;
using EY.CapitalEdge.ChatOrchestrator.Activities;
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Services;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace EY.CapitalEdge.ChatOrchestrator.Tests
{
    public class FollowUpSuggestionTests
    {
        [Theory]
        [InlineData("{\"appInfo\":{\"key\":\"CE4_PMO\",\"name\":\"PMO\",\"teamTypeIds\":[1]}}", true, false)]
        [InlineData("{\"activePOApps\":[{\"key\":\"CE4_PMO\",\"name\":\"PMO\"}]}", false, true)]
        public async Task FollowUpSuggestionActivity_WhenData_ReturnFollowUpQuestionAsync(string context, bool isThereAnyAppInfo, bool isThereAnyActivePOApps)
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "FollowUpSuggestionActivity")).Returns(mockLogger.Object);

            var suggestion = new Mock<ISuggestion>();
            var mockEmbeddings = new List<Models.Suggestion>()
            {
                new Models.Suggestion
                {
                    Id = 1,
                    Chunk = "List out TSAs with behind schedule tasks linked.",
                    Source = "project-data",
                    Embedding = new float[] { },
                    VisibleToAssistant = true
                },
               new Models.Suggestion
                {
                    Id = 2,
                    Chunk = "How many milestones does each team have?",
                    Source = "project-data",
                    Embedding = new float[] { },
                    VisibleToAssistant = true
                },
                new Models.Suggestion
                {
                    Id = 3,
                    Chunk = "List out workplan items due next week in PMO app.",
                    Source = "project-data",
                    Embedding = new float[] { },
                    VisibleToAssistant = true
                }
            };

            suggestion.Setup(s => s.GetEmbeddingsAsync(It.IsAny<SearchClientDto>(), It.IsAny<string>())).ReturnsAsync(mockEmbeddings);

            var mmrQuestionSelectorWithEmbedding = new Mock<IMmrQuestionSelectorWithEmbedding>();
            mmrQuestionSelectorWithEmbedding.Setup(m => m.SelectSimilarQuestionsAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new List<string>()
            {
                "List out TSAs with behind schedule tasks linked.",
                "How many milestones does each team have?",
                "List out workplan items due next week in PMO app."
            });

            var common = new Mock<ICommon>();
            common.Setup(c => c.GetTokenData(It.IsAny<ILogger>(), It.IsAny<string>())).Returns(
                new Token { AiSearchInstanceName = "https://test.search.windows.net/", MetadataIndexName = "metadata-index-test" });

            var sampleSecret = new KeyVaultSecret("euwdsatddeazs01", "{\"endpoint\":\"https://test.search.windows.net\",\"version\":\"2023-11-01\",\"key\":\"xyz\" }");
            var sampleResponse = Response.FromValue(sampleSecret, Mock.Of<Response>());
            var secretClient = new Mock<SecretClient>();
            secretClient.Setup(s => s.GetSecretAsync(It.IsAny<string>(), null, default(CancellationToken)))
                .ReturnsAsync(sampleResponse);

            common.Setup(c => c.Deserialize<AzureSearchConfigDto>(It.IsAny<string>()))
                .Returns(new AzureSearchConfigDto { Endpoint = "https://test.search.windows.net", Key = "xyz", Version = "2023-11-01" });

            Dictionary<string, object?> contextObj = JsonSerializer.Deserialize<Dictionary<string, object?>>(context, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Dictionary<string, object?>();

            common.Setup(c => c.IsThereAnyAppInfo(contextObj)).Returns(isThereAnyAppInfo);
            common.Setup(c => c.GetAppInfo(contextObj)).Returns(new AppInfo { Key = "CE4_PMO", Name = "PMO", TeamTypeIds = [1] });

            common.Setup(c => c.IsThereAnyActivePOApps(contextObj)).Returns(isThereAnyActivePOApps);
            common.Setup(c => c.GetActivePOApps(contextObj)).Returns(new List<ActivePOApps> { new ActivePOApps { Key = "CE4_PMO", Name = "PMO" } });

            var followUpSuggestion = new FollowUpSuggestionActivity(
                mockLoggerProvider.Object,
                suggestion.Object,
                mmrQuestionSelectorWithEmbedding.Object,
                common.Object,
                secretClient.Object);

            var mockFunctionContext = new Mock<FunctionContext>();

            
            var followUpSuggestionInput = new FollowUpSuggestionInput
            {
                ChatId = Guid.NewGuid(),
                InstanceId = "05cecd8a143545bc9d05cb6dd69ada1c",
                ChatHistory = new List<ChatHistory>()
                {
                    new ChatHistory
                    {
                        MessageId = 1,
                        Role = "user",
                        Content = "How to export report to PPT?",
                    },
                    new ChatHistory
                    {
                        MessageId = 2,
                        Role = "assistant",
                        Content = "You can export the report to PPT by following these steps.",
                    }
                },
                Input = new ChatQuestion
                {
                    MessageId = 1,
                    ChatId = Guid.NewGuid(),
                    ProjectFriendlyId = "ai-ceassistbench",
                    Question = "How to export report to PPT?",
                    Sources = ["project-data"],
                    Context = contextObj,
                    InputSources = ["project-data"],
                    Token = "Bearer xyz"
                }
            };

            // Act
            var result = await followUpSuggestion.Run(followUpSuggestionInput, mockFunctionContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FollowUpSuggestionsResponse>>(result);
            suggestion.Verify(s => s.GetEmbeddingsAsync(It.IsAny<SearchClientDto>(), It.IsAny<string>()), Times.Once);
            mmrQuestionSelectorWithEmbedding.Verify(m => m.SetQuestions(It.IsAny<Dictionary<string, float[]>>()), Times.Once);
            mmrQuestionSelectorWithEmbedding.Verify(m => m.RemoveSimilarQuestionsAsync(It.IsAny<List<string>>()), Times.Once);
            mmrQuestionSelectorWithEmbedding.Verify(m => m.SelectSimilarQuestionsAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }


        [Fact]
        public async Task FollowUpSuggestionActivity_WhenError_ReturnEmptyFollowUpQuestion()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            var mockLoggerProvider = new Mock<Services.ILoggerProvider>();
            mockLoggerProvider.Setup(p => p.GetLogger(It.IsAny<FunctionContext>(), "FollowUpSuggestionActivity")).Returns(mockLogger.Object);

            var suggestion = new Mock<ISuggestion>();
            var mmrQuestionSelectorWithEmbedding = new Mock<IMmrQuestionSelectorWithEmbedding>();
            var common = new Mock<ICommon>();
            var secretClient = new Mock<SecretClient>();

            var followUpSuggestion = new FollowUpSuggestionActivity(
                mockLoggerProvider.Object,
                suggestion.Object,
                mmrQuestionSelectorWithEmbedding.Object,
                common.Object,
                secretClient.Object);

            var mockFunctionContext = new Mock<FunctionContext>();

            var followUpSuggestionInput = new FollowUpSuggestionInput
            {
                ChatId = Guid.NewGuid(),
                InstanceId = "05cecd8a143545bc9d05cb6dd69ada1c",
                ChatHistory = new List<ChatHistory>(),
                Input = new ChatQuestion
                {
                    MessageId = 1,
                    ChatId = Guid.NewGuid(),
                    ProjectFriendlyId = "ai-ceassistbench",
                    Question = "How to export report to PPT?",
                    Sources = ["projet-data"],
                    Context = new Dictionary<string, object?>(),
                    InputSources = ["projet-data"],
                    Token = "Bearer xyz"
                }
            };

            // Act
            var result = await followUpSuggestion.Run(followUpSuggestionInput, mockFunctionContext.Object);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FollowUpSuggestionsResponse>>(result);
            Assert.Empty(result);
        }
    }
}

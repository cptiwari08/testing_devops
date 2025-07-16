using EY.CapitalEdge.ChatOrchestrator.Activities;
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Tests.Fakes;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using Microsoft.Extensions.Logging;
using Moq;

namespace EY.CapitalEdge.ChatOrchestrator.Tests
{
    public class ChatOrchestratorTests
    {
        [Fact]
        public async Task ChatOrchestrator_EYGuidanceSource_Ok()
        {
            // Arrange
            var mockOrchestrationContext = new Mock<FakeTaskOrchestrationContext>();
            mockOrchestrationContext.Setup(c => c.CreateReplaySafeLogger<Orchestrators.ChatOrchestrator>()).Returns(new FakeLogger());
            var input = new ChatQuestion()
            {
                MessageId = 1,
                ChatId = Guid.Parse("16ed317e-120a-4ef1-8439-f5e6ffbcd26f"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "How to export report to PPT?",
                Sources = ["ey-guidance"],
                InputSources = ["ey-guidance"],
                Token = "Bearer xyz",
                Context = []
            };
            mockOrchestrationContext.Setup(c => c.GetInput<ChatQuestion>()).Returns(input);

            var projectContext = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/configuration/PROJECT_CONTEXT",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"id\":1,\"title\":\"\",\"key\":\"PROJECT_CONTEXT\",\"value\":\"Let's Test\",\"isEnabled\":false}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/configuration/PROJECT_CONTEXT")), default))
                .ReturnsAsync(projectContext);

            var chatHistory = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/user/chat/16ed317e-120a-4ef1-8439-f5e6ffbcd26f",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"key\":\"user:Daniel.Rivera@ey.com:chatid:b43fde43-91f0-4521-96f6-68b69f38d517\",\"content\":[{\"messageId\":1,\"role\":\"user\",\"content\":\"When was the deal announced?\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:02.4027667Z\",\"response\":null,\"lastUpdatedTime\":null,\"status\":null},{\"messageId\":2,\"role\":\"assistant\",\"content\":\"Internet\\nSorry, I am unable to answer your question at the moment. Please try again later.\\n\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:12.5654683Z\",\"response\":[{\"sourceName\":\"summary\",\"content\":\"No backend responses available for summarization.\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":null},{\"sourceName\":\"internet\",\"content\":\"Unprocessable Entity\",\"status\":\"422\",\"sqlQuery\":null,\"citingSources\":[]}],\"lastUpdatedTime\":\"2024-05-07T19:57:13.1234558Z\",\"status\":\"pending\"}]}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/user/chat/")), default))
                .ReturnsAsync(chatHistory);

            var helpCopilotChatStart = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://localhost:7291/api/v1/help-copilot/chat/start",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"backend\":\"help-copilot\",\"response\":{\"conversationReferenceId\":\"64348a33-24ab-4fa0-8806-79185362dac2\",\"welcomeTextMessage\":\"Hello!  I’m here to assist you with any queries you might have related to SaT methodology and tools.\"},\"citingSources\":[],\"rawReponse\":null}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.EndsWith("/api/v1/help-copilot/chat/start")), default))
                .ReturnsAsync(helpCopilotChatStart);

            var helpCopilot = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://localhost:7291/api/v1/help-copilot/chat",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"backend\":\"help-copilot\",\"response\":\"To export documents from Kira, perform the following steps: 1. On the ‘document list’ tab, select the documents you would like to export. To select each document individually, click the checkbox to the left of each document name. To select all the documents on the page, click the checkbox at the top of the document checkboxes. 2. Then click ’export’. 3. In the dialogue box that appears, choose the PDF format and the layout of your exported documents. You can configure your export to exclude worksheets, smart fields, and/or tags.\",\"citingSources\":[{\"sourceName\":\"help-copilot\",\"sourceType\":\"documents\",\"sourceValue\":[{\"active\":false,\"documentGuid\":\"b344bb9b-5402-42e9-a1a8-0fcd49d3b5d9\",\"documentName\":\"Kira - Detailed User Guide.pdf\",\"isVideoDocument\":false,\"pages\":[{\"pageNumber\":19,\"metaData\":null}],\"version\":\"v1.0\",\"videoUrl\":null}]}],\"rawReponse\":{\"isSuccess\":false,\"messages\":[{\"conversationGuid\":\"e27164c7-db4f-4c31-8545-6b67a89b9fe5\",\"conversationStatusId\":2,\"messageGuid\":\"b1c628ca-307f-4658-b728-1cdfabf24117\",\"messageTypeId\":1,\"messageText\":\"To export documents from Kira, perform the following steps: 1. On the ‘document list’ tab, select the documents you would like to export. To select each document individually, click the checkbox to the left of each document name. To select all the documents on the page, click the checkbox at the top of the document checkboxes. 2. Then click ’export’. 3. In the dialogue box that appears, choose the PDF format and the layout of your exported documents. You can configure your export to exclude worksheets, smart fields, and/or tags.\",\"messageCreatedDate\":\"2024-05-14T13:41:37.8056033Z\",\"user\":{\"guid\":\"1b8b3c2d-8274-47c1-9c07-524591c4b721\",\"displayName\":\"Daniel Rivera\",\"emailId\":\"Daniel.Rivera@ey.com\",\"createdDate\":\"0001-01-01T00:00:00\",\"userIdentity\":null,\"roles\":null},\"additionalInfo\":null,\"isMessageLiked\":null,\"score\":78.87,\"showFeedbackOptions\":true,\"showTypingEffect\":true,\"chatResponseGuid\":\"b1c628ca-307f-4658-b728-1cdfabf24117\",\"currentResponseCount\":1,\"totalResponseCount\":1,\"documents\":[{\"active\":false,\"documentGuid\":\"b344bb9b-5402-42e9-a1a8-0fcd49d3b5d9\",\"documentName\":\"Kira - Detailed User Guide.pdf\",\"isVideoDocument\":false,\"pages\":[{\"pageNumber\":19,\"metaData\":null}],\"version\":\"v1.0\",\"videoUrl\":null}]}],\"status\":0,\"code\":0,\"message\":null}}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.EndsWith("/api/v1/help-copilot/chat/sensitive-data-support")), default))
                .ReturnsAsync(helpCopilot);

            var eyIpd = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "http://127.0.0.1:8000/ey-ip",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"backend\":\"ey-ip\",\"sql\":\"\",\"response\":\"To export a report to PDF, you can use programs like MS Word, Excel, or PowerPoint. Look for the \\\"Save As\\\" or \\\"Export\\\" option and choose PDF as the file format. Additionally, there are online platforms and document management systems that offer PDF export functionality. Just select the document you want to export and choose the PDF format.\",\"citingSources\":[],\"rawResponse\":{\"response\":\"To export a report to PDF, you can use programs like MS Word, Excel, or PowerPoint. Look for the \\\"Save As\\\" or \\\"Export\\\" option and choose PDF as the file format. Additionally, there are online platforms and document management systems that offer PDF export functionality. Just select the document you want to export and choose the PDF format.\",\"sources\":[],\"source_nodes\":[],\"is_dummy_stream\":false}}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/ey-ip")), default))
                .ReturnsAsync(eyIpd);

            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<string>(nameof(SummarizeActivity), It.IsAny<SummarizeActivityInput>(), default))
                .ReturnsAsync("Summarized response");

            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.GetTokenData(It.IsAny<ILogger>(), It.IsAny<String>())).Returns(new Token()
            {
                UniqueName = "Daniel.Rivera@ey.com",
                Email = "Daniel.Rivera@ey.com",
                FamilyName = "Rivera",
                GivenName = "Daniel",
                UserType = "Internal",
                Oid = "1b8b3c2d-8274-47c1-9c07-524591c4b721",
                CeOid = "a6a33b9a-a1b2-402e-9071-3186a5f39788",
                Upn = "Daniel.Rivera@ey.com",
                SpUrl = "https://eyus.sharepoint.com/sites/ce5-productdevelop-cedevprjco-dev",
                PoAppUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/ce4",
                PoApiUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco-ce4/api",
                CopilotAppUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot",
                CopilotApiUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api",
                ProjectId = "d0b71f8e-ead4-4e1d-bd70-7681c3dd491d",
                Scope = new List<string> { "ey-guidance", "internet", "project-docs", "project-data" },
                Nbf = 1715173550,
                Exp = 1715177150,
                Iat = 1715173550,
                Iss = "https://eycapitaledge-dev.ey.com",
                Aud = "d0b71f8e-ead4-4e1d-bd70-7681c3dd491d"
            });
            mockCommon.Setup(c => c.Deserialize<ProjectContextResponse>(It.IsAny<string>())).Returns(new ProjectContextResponse()
            {
                Id = 1,
                Title = "",
                Key = "PROJECT_CONTEXT",
                Value = "Project description",
                IsEnabled = false
            });
            mockCommon.Setup(c => c.Deserialize<ChatHistoryResponse>(It.IsAny<string>())).Returns(new ChatHistoryResponse()
            {
                Key = "user:Daniel.Rivera@ey.com:chatid:b43fde43-91f0-4521-96f6-68b69f38d517",
                Content = new List<Models.ChatHistoryService.Message> {
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 1,
                        Role = "user",
                        Content = "How to export report to PDF?",
                        Sources = ["ey-guidance"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null
                    },
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 2,
                        Role = "assistant",
                        Content = "Pending",
                        Sources = ["ey-guidance"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null,
                        LastUpdatedTime = new DateTime()
                    }
                }
            });
            mockCommon.Setup(c => c.Deserialize<BackendResponse>(It.Is<string>(s => s.Contains("ey-guidance")))).Returns(new BackendResponse()
            {
                Backend = "ey-guidance",
                Sql = null,
                Response = "\"To export documents to PDF from Kira, follow these steps: 1. Select the documents you would like to export on the ‘document list’ tab. You can select each document individually or select all the documents on the page. 2. Click ’export’. 3. In the dialogue box that pops up, choose the PDF format and configure your export to exclude worksheets, smart fields, and/or tags if needed. 4. Set your preferred layout of the exported documents.\"",
                CitingSources = null,
                RawReponse = null
            });
            mockCommon.Setup(c => c.Deserialize<BackendResponse>(It.Is<string>(s => s.Contains("help-copilot")))).Returns(new BackendResponse()
            {
                Backend = "ey-guidance-help-copilot",
                Sql = null,
                Response = "To export documents from Kira, perform the following steps: 1. On the ‘document list’ tab, select the documents you would like to export. To select each document individually, click the checkbox to the left of each document name. To select all the documents on the page, click the checkbox at the top of the document checkboxes. 2. Then click ’export’. 3. In the dialogue box that appears, choose the PDF format and the layout of your exported documents. You can configure your export to exclude worksheets, smart fields, and/or tags.",
                CitingSources = [],
                RawReponse = null
            });
            mockCommon.Setup(c => c.Deserialize<BackendResponse>(It.Is<string>(s => s.Contains("welcomeTextMessage")))).Returns(new BackendResponse()
            {
                Backend = "help-copilot",
                Sql = null,
                Response = "{\"backend\":\"help-copilot\",\"response\":{\"conversationReferenceId\":\"64348a33-24ab-4fa0-8806-79185362dac2\",\"welcomeTextMessage\":\"Hello!  I’m here to assist you with any queries you might have related to SaT methodology and tools.\"},\"citingSources\":[],\"rawReponse\":null}",
                CitingSources = [],
                RawReponse = null
            });
            mockCommon.Setup(c => c.Deserialize<ConversationResponse>(It.Is<string>(s => s.Contains("welcomeTextMessage")))).Returns(new ConversationResponse()
            {
                ConversationReferenceId = "64348a33-24ab-4fa0-8806-79185362dac2",
                WelcomeTextMessage = "Hello!  I’m here to assist you with any queries you might have related to SaT methodology and tools."
            });
            mockCommon.Setup(c => c.Deserialize<BackendResponse>(It.Is<string>(s => s.Contains("\"ey-ip\"")))).Returns(new BackendResponse()
            {
                Backend = "ey-ip",
                Sql = null,
                Response = "To export a report to PDF, you can use programs like MS Word, Excel, or PowerPoint. Look for the \"Save As\" or \"Export\" option and choose PDF as the file format. Additionally, there are online platforms and document management systems that offer PDF export functionality. Just select the document you want to export and choose the PDF format.",
                CitingSources = [],
                RawReponse = null
            });
            mockCommon.Setup(c => c.GetEnvironmentVariable(It.IsAny<ILogger>(), "ProjectDataReturnSql")).Returns("true");

            // Act
            var chatOrchestrator = new Orchestrators.ChatOrchestrator(mockCommon.Object);
            var result = await chatOrchestrator.RunOrchestrator(mockOrchestrationContext.Object);

            // Assert
            Assert.NotNull(result);
            mockOrchestrationContext.Verify(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/chat/start")), default), Times.Once);
            mockOrchestrationContext.Verify(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/ey-ip")), default), Times.Once);
        }

        [Fact]
        public async Task ChatOrchestrator_ProjectDataSource_Ok()
        {
            // Arrange
            var mockOrchestrationContext = new Mock<FakeTaskOrchestrationContext>();
            mockOrchestrationContext.Setup(c => c.CreateReplaySafeLogger<Orchestrators.ChatOrchestrator>()).Returns(new FakeLogger());
            var input = new ChatQuestion()
            {
                MessageId = 1,
                ChatId = Guid.Parse("c0fa8cbf-daeb-4539-ae0e-7220bb92a498"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "Which project team has the most overdue risks?",
                Sources = ["project-data"],
                InputSources = ["project-data"],
                Context = new Dictionary<string, object?>
                {
                    { 
                        "suggestion", new { 
                            id = "id", 
                            sqlQuery = "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc",
                            source = "project-data"
                        }
                    }
                },
                Token = "Bearer xyz"
            };
            mockOrchestrationContext.Setup(c => c.GetInput<ChatQuestion>()).Returns(input);

            var projectContext = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/configuration/PROJECT_CONTEXT",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"id\":1,\"title\":\"\",\"key\":\"PROJECT_CONTEXT\",\"value\":\"Project description\",\"isEnabled\":false}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/configuration/PROJECT_CONTEXT")), default))
                .ReturnsAsync(projectContext);

            var chatHistory = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/user/chat/fdbacb99-4eed-4d26-9045-4ba8ecd7c3b3",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"key\":\"user:Daniel.Rivera@ey.com:chatid:b43fde43-91f0-4521-96f6-68b69f38d517\",\"content\":[{\"messageId\":1,\"role\":\"user\",\"content\":\"When was the deal announced?\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:02.4027667Z\",\"response\":null,\"lastUpdatedTime\":null,\"status\":null},{\"messageId\":2,\"role\":\"assistant\",\"content\":\"Internet\\nSorry, I am unable to answer your question at the moment. Please try again later.\\n\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:12.5654683Z\",\"response\":[{\"sourceName\":\"summary\",\"content\":\"No backend responses available for summarization.\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":null},{\"sourceName\":\"internet\",\"content\":\"Unprocessable Entity\",\"status\":\"422\",\"sqlQuery\":null,\"citingSources\":[]}],\"lastUpdatedTime\":\"2024-05-07T19:57:13.1234558Z\",\"status\":\"pending\"}]}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/user/chat/")), default))
                .ReturnsAsync(chatHistory);

            var projectData = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "http://127.0.0.1:8000/project-data",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"backend\":\"project-data\",\"sql\":\"SELECT      pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc\",\"response\":\"The project team with the most overdue risks is the \\\"Project\\\" team, with a count of 1.\",\"citingSources\":[{\"sourceName\":\"project-data\",\"sourceType\":\"table\",\"sourceValue\":[\"RisksAndIssues\",\"ProjectTeams\",\"statuses\"]}],\"rawResponse\":null}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/project-data")), default))
                .ReturnsAsync(projectData);

            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<string>(nameof(SummarizeActivity), It.IsAny<SummarizeActivityInput>(), default))
                .ReturnsAsync("Rewritten response: \"The 'Project' team has the most overdue risks with a count of 1.\"");

            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.GetTokenData(It.IsAny<ILogger>(), It.IsAny<String>())).Returns(new Token()
            {
                UniqueName = "Daniel.Rivera@ey.com",
                Email = "Daniel.Rivera@ey.com",
                FamilyName = "Rivera",
                GivenName = "Daniel",
                UserType = "Internal",
                Oid = "1b8b3c2d-8274-47c1-9c07-524591c4b721",
                CeOid = "a6a33b9a-a1b2-402e-9071-3186a5f39788",
                Upn = "Daniel.Rivera@ey.com",
                SpUrl = "https://eyus.sharepoint.com/sites/ce5-productdevelop-cedevprjco-dev",
                PoAppUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/ce4",
                PoApiUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco-ce4/api",
                CopilotAppUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot",
                CopilotApiUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api",
                ProjectId = "d0b71f8e-ead4-4e1d-bd70-7681c3dd491d",
                Scope = new List<string> { "ey-guidance", "internet", "project-docs", "project-data" },
                Nbf = 1715173550,
                Exp = 1715177150,
                Iat = 1715173550,
                Iss = "https://eycapitaledge-dev.ey.com",
                Aud = "d0b71f8e-ead4-4e1d-bd70-7681c3dd491d"
            });
            mockCommon.Setup(c => c.Deserialize<ProjectContextResponse>(It.IsAny<string>())).Returns(new ProjectContextResponse()
            {
                Id = 1,
                Title = "",
                Key = "PROJECT_CONTEXT",
                Value = "Project description",
                IsEnabled = false
            });
            mockCommon.Setup(c => c.Deserialize<ChatHistoryResponse>(It.IsAny<string>())).Returns(new ChatHistoryResponse()
            {
                Key = "user:Daniel.Rivera@ey.com:chatid:b43fde43-91f0-4521-96f6-68b69f38d517",
                Content = new List<Models.ChatHistoryService.Message> {
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 1,
                        Role = "user",
                        Content = "When was the deal announced?",
                        Sources = ["project-data"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null
                    },
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 2,
                        Role = "assistant",
                        Content = "Pending",
                        Sources = ["project-data"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null,
                        LastUpdatedTime = new DateTime()
                    }
                }
            });
            mockCommon.Setup(c => c.Deserialize<BackendResponse>(It.IsAny<string>())).Returns(new BackendResponse()
            {
                Backend = "project-data",
                Sql = "SELECT      pt.Title AS ProjectTeamTitle,     COUNT(1) AS OverdueRiskCount FROM      RisksAndIssues RI INNER JOIN      ProjectTeams pt ON RI.ProjectTeamId = pt.ID LEFT JOIN      statuses S ON RI.ItemStatusId = S.ID WHERE      RI.IssueRiskCategory = 'Risk'     AND RI.ItemDueDate < GETDATE()     AND (S.[Key] IS NULL OR S.[Key] NOT IN ('COMPLETED', 'CLOSED')) GROUP BY      pt.Title ORDER BY  OverdueRiskCount desc",
                Response = "To export a report to PowerPoint (PPT)....",
                CitingSources = new List<CitingSource> { new CitingSource() { SourceName = "project-data", SourceType = "table", SourceValue = new List<string>() { "RisksAndIssues", "ProjectTeams", "statuses" } } },
                RawReponse = null
            });
            mockCommon.Setup(c => c.GetEnvironmentVariable(It.IsAny<ILogger>(), "ProjectDataReturnSql")).Returns("true");

            // Act
            var chatOrchestrator = new Orchestrators.ChatOrchestrator(mockCommon.Object);
            var result = await chatOrchestrator.RunOrchestrator(mockOrchestrationContext.Object);

            // Assert
            Assert.NotNull(result);
            mockOrchestrationContext.Verify(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/project-data")), default), Times.Once);
        }

        [Fact]
        public async Task ChatOrchestrator_ProjectDocsSource_Ok()
        {
            // Arrange
            var mockOrchestrationContext = new Mock<FakeTaskOrchestrationContext>();
            mockOrchestrationContext.Setup(c => c.CreateReplaySafeLogger<Orchestrators.ChatOrchestrator>()).Returns(new FakeLogger());
            var input = new ChatQuestion()
            {
                MessageId = 1,
                ChatId = Guid.Parse("de28ea8e-ff09-404f-bd70-95ecdc07a76c"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "What is the governance structure for this project?",
                Sources = ["project-docs"],
                InputSources = ["project-docs"],
                Token = "Bearer xyz",
                Context = []
            };
            mockOrchestrationContext.Setup(c => c.GetInput<ChatQuestion>()).Returns(input);

            var projectContext = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/configuration/PROJECT_CONTEXT",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"id\":1,\"title\":\"\",\"key\":\"PROJECT_CONTEXT\",\"value\":\"Project description\",\"isEnabled\":false}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity), 
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/configuration/PROJECT_CONTEXT")), default))
                .ReturnsAsync(projectContext);

            var chatHistory = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/user/chat/fdbacb99-4eed-4d26-9045-4ba8ecd7c3b3",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"key\":\"user:Daniel.Rivera@ey.com:chatid:de28ea8e-ff09-404f-bd70-95ecdc07a76c\",\"content\":[{\"messageId\":1,\"role\":\"user\",\"content\":\"When was the deal announced?\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:02.4027667Z\",\"response\":null,\"lastUpdatedTime\":null,\"status\":null},{\"messageId\":2,\"role\":\"assistant\",\"content\":\"Internet\\nSorry, I am unable to answer your question at the moment. Please try again later.\\n\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:12.5654683Z\",\"response\":[{\"sourceName\":\"summary\",\"content\":\"No backend responses available for summarization.\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":null},{\"sourceName\":\"internet\",\"content\":\"Unprocessable Entity\",\"status\":\"422\",\"sqlQuery\":null,\"citingSources\":[]}],\"lastUpdatedTime\":\"2024-05-07T19:57:13.1234558Z\",\"status\":\"pending\"}]}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/user/chat/")), default))
                .ReturnsAsync(chatHistory);

            var projectDocs = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "http://10.0.11.145:8000/project-docs",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"backend\":\"project-docs\",\"sql\":null,\"response\":\"The governance structure of this project is...\",\"citingSources\":[],\"rawResponse\":{\"response\":\"Raw response\",\"sources\":[],\"source_nodes\":[]}}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/project-docs")), default))
                .ReturnsAsync(projectDocs);

            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<string>(nameof(SummarizeActivity), 
                    It.IsAny<SummarizeActivityInput>(), default))
                .ReturnsAsync("Summarized response");

            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.GetTokenData(It.IsAny<ILogger>(), It.IsAny<String>())).Returns(new Token()
            {
                UniqueName = "Daniel.Rivera@ey.com",
                Email = "Daniel.Rivera@ey.com",
                FamilyName = "Rivera",
                GivenName = "Daniel",
                UserType = "Internal",
                Oid = "1b8b3c2d-8274-47c1-9c07-524591c4b721",
                CeOid = "a6a33b9a-a1b2-402e-9071-3186a5f39788",
                Upn = "Daniel.Rivera@ey.com",
                SpUrl = "https://eyus.sharepoint.com/sites/ce5-productdevelop-cedevprjco-dev",
                PoAppUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/ce4",
                PoApiUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco-ce4/api",
                CopilotAppUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot",
                CopilotApiUrl = "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api",
                ProjectId = "d0b71f8e-ead4-4e1d-bd70-7681c3dd491d",
                Scope = new List<string> { "ey-guidance", "internet", "project-docs", "project-data" },
                Nbf = 1715173550,
                Exp = 1715177150,
                Iat = 1715173550,
                Iss = "https://eycapitaledge-dev.ey.com",
                Aud = "d0b71f8e-ead4-4e1d-bd70-7681c3dd491d"
            });
            mockCommon.Setup(c => c.Deserialize<ProjectContextResponse>(It.IsAny<string>())).Returns(new ProjectContextResponse()
            {
                Id = 1,
                Title = "",
                Key = "PROJECT_CONTEXT",
                Value = "Project description",
                IsEnabled = false
            });
            mockCommon.Setup(c => c.Deserialize<ChatHistoryResponse>(It.IsAny<string>())).Returns(new ChatHistoryResponse()
            {
                Key = "user:Daniel.Rivera@ey.com:chatid:b43fde43-91f0-4521-96f6-68b69f38d517",
                Content = new List<Models.ChatHistoryService.Message> {
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 1,
                        Role = "user",
                        Content = "When was the deal announced?",
                        Sources = ["internet"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null
                    },
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 2,
                        Role = "assistant",
                        Content = "Internet\nSorry, I am unable to answer your question at the moment. Please try again later.\n",
                        Sources = ["internet"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null,
                        LastUpdatedTime = new DateTime()
                    }
                }
            });
            mockCommon.Setup(c => c.Deserialize<BackendResponse>(It.IsAny<string>())).Returns(new BackendResponse()
            {
                Backend = "project-docs",
                Sql = null,
                Response = "The governance structure of this project is...",
                CitingSources = new List<CitingSource> { new CitingSource() { SourceName = "project-docs", SourceType = "project-docs", SourceValue = new List<string>() } },
                RawReponse = null
            });
            mockCommon.Setup(c => c.GetEnvironmentVariable(It.IsAny<ILogger>(), "ProjectDataReturnSql")).Returns("true");

            // Act
            var chatOrchestrator = new Orchestrators.ChatOrchestrator(mockCommon.Object);
            var result = await chatOrchestrator.RunOrchestrator(mockOrchestrationContext.Object);

            // Assert
            Assert.NotNull(result);
            mockOrchestrationContext.Verify(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/project-docs")), default), Times.Once);
        }

        [Fact]
        public async Task ChatOrchestrator_InternetSource_Ok()
        {
            // Arrange
            var mockOrchestrationContext = new Mock<FakeTaskOrchestrationContext>();
            mockOrchestrationContext.Setup(c => c.CreateReplaySafeLogger<Orchestrators.ChatOrchestrator>()).Returns(new FakeLogger());
            var input = new ChatQuestion()
            {
                MessageId = 1,
                ChatId = Guid.Parse("89da0749-e1af-40ff-9bc5-11d7a5c54164"),
                ProjectFriendlyId = "ai-ceassistbench",
                Question = "How to export report to PPT?",
                Sources = ["internet"],
                InputSources = ["internet"],
                Token = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkRhbmllbC5SaXZlcmFAZXkuY29tIiwiZW1haWwiOiJEYW5pZWwuUml2ZXJhQGV5LmNvbSIsImZhbWlseV9uYW1lIjoiUml2ZXJhIiwiZ2l2ZW5fbmFtZSI6IkRhbmllbCIsInVzZXJfdHlwZSI6IkludGVybmFsIiwib2lkIjoiMWI4YjNjMmQtODI3NC00N2MxLTljMDctNTI0NTkxYzRiNzIxIiwiY2Vfb2lkIjoiMzMyYmU0NjYtYTEyZS00ZDNlLTk5YzEtZTY3ZTJkNzljOWRkIiwidXBuIjoiRGFuaWVsLlJpdmVyYUBleS5jb20iLCJzcF91cmwiOiJodHRwczovL2V5dXMuc2hhcmVwb2ludC5jb20vc2l0ZXMvY2U1LXByb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28tZGV2IiwicG9fYXBwX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY2U0IiwicG9fYXBpX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28tY2U0L2FwaSIsImNvcGlsb3RfYXBwX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY29waWxvdCIsImNvcGlsb3RfYXBpX3VybCI6Imh0dHBzOi8vZXljYXBpdGFsZWRnZS1kZXYuZXkuY29tL3Byb2R1Y3RkZXZlbG9wLWNlZGV2cHJqY28vY29waWxvdC9hcGkiLCJkYXRhX2luZGV4X25hbWUiOiIiLCJkb2NzX2luZGV4X25hbWUiOiIiLCJnbG9zc19pbmRleF9uYW1lIjoiIiwicHJvamVjdF9pZCI6ImQwYjcxZjhlLWVhZDQtNGUxZC1iZDcwLTc2ODFjM2RkNDkxZCIsInByb2plY3RfZnJpZW5kbHlfaWQiOiJwcm9kdWN0ZGV2ZWxvcC1jZWRldnByamNvIiwic2NvcGUiOlsiZXktZ3VpZGFuY2UiLCJpbnRlcm5ldCIsInByb2plY3QtZG9jcyIsInByb2plY3QtZGF0YSJdLCJuYmYiOjE3MTUxMDczMDYsImV4cCI6MTcxNTExMDkwNiwiaWF0IjoxNzE1MTA3MzA2LCJpc3MiOiJodHRwczovL2V5Y2FwaXRhbGVkZ2UtZGV2LmV5LmNvbSIsImF1ZCI6ImQwYjcxZjhlLWVhZDQtNGUxZC1iZDcwLTc2ODFjM2RkNDkxZCJ9.DkF7OU7tdrnP-PVR-iYiINGfJZHYTi8lEeyu6Df9KaWXHYLwpPy8sNYxSS-P69SQOfufaJ0yoc-M5mQKO1qJ8zYxK7ebOOD4L9wtjVmeHSNfMGr68lXUJoiP4pnwqBOD2PYzCXG_SbKX9KUlr_DC03_MBnyqVZG9guXiXMp2p6k3YLLIi83HNmbsZblBJNnxhY2h_kd9KjdbeUougqtI1Wfm9sEpkIOCvJPPsmTj6OwpcclLK0z8wij6FNfN_tL2OI4J_y7fl4CoqXQxvfIpnMcm6nYaz7g76zlDLqoPoq_Q_JiHmRCf8vfKpNEUUm8ERa7qsW4IwVLUcUu5uabFAJazDEeMtOS-kEvvkJr6JtaBPaLo9txqJiaP0bmB0Y8_n4GY9RofNOsY49mCNuDCVzc-F0JWXGbwkpcqvYdFaVDHqOor_l6PuvNaXtAc7-44OJmjqZB2rovzAyKmyI5NZ_VmmKvjAeHazXEEDrPMsnDxqf5iP1wJCwqAguJjUcsGOnkD02aXY9wrcuw7-lFfIeA0_GKKv7ET_bWgW_6OpgwPlOi_asMSAh_oq-OM4nGzV7KmXCYytC-xxV1z2w9ZV9ztzkNDuVqZm75B5pSb-9XLlQvT6odsHClTr0q6-eJir3mujx6ZZnpdl0GvdmmHTyorj20iKE5udWjBp4aPAZM",
                Context = []
            };
            mockOrchestrationContext.Setup(c => c.GetInput<ChatQuestion>()).Returns(input);

            var projectContext = new SerializableHttpResponseMessage(
                method: HttpMethod.Post, 
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/configuration/PROJECT_CONTEXT",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"id\":1,\"title\":\"\",\"key\":\"PROJECT_CONTEXT\",\"value\":\"Project description\",\"isEnabled\":false}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/configuration/PROJECT_CONTEXT")), default))
                .ReturnsAsync(projectContext);

            var chatHistory = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "https://eycapitaledge-dev.ey.com/productdevelop-cedevprjco/copilot/api/user/chat/fdbacb99-4eed-4d26-9045-4ba8ecd7c3b3",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"key\":\"user:Daniel.Rivera@ey.com:chatid:b43fde43-91f0-4521-96f6-68b69f38d517\",\"content\":[{\"messageId\":1,\"role\":\"user\",\"content\":\"When was the deal announced?\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:02.4027667Z\",\"response\":null,\"lastUpdatedTime\":null,\"status\":null},{\"messageId\":2,\"role\":\"assistant\",\"content\":\"Internet\\nSorry, I am unable to answer your question at the moment. Please try again later.\\n\",\"sources\":[\"internet\"],\"context\":{\"suggestion\":{\"id\":\"117\",\"sqlQuery\":null,\"source\":\"internet\"},\"isMessageLiked\":null},\"createdTime\":\"2024-05-07T19:57:12.5654683Z\",\"response\":[{\"sourceName\":\"summary\",\"content\":\"No backend responses available for summarization.\",\"status\":\"200\",\"sqlQuery\":null,\"citingSources\":null},{\"sourceName\":\"internet\",\"content\":\"Unprocessable Entity\",\"status\":\"422\",\"sqlQuery\":null,\"citingSources\":[]}],\"lastUpdatedTime\":\"2024-05-07T19:57:13.1234558Z\",\"status\":\"pending\"}]}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("copilot/api/user/chat/")), default))
                .ReturnsAsync(chatHistory);

            var internet = new SerializableHttpResponseMessage(
                method: HttpMethod.Post,
                requestUri: "",
                statusCode: 200,
                reasonPhrase: "OK",
                isSuccessStatusCode: true,
                response: "{\"backend\":\"internet\",\"sql\":null,\"response\":\"To export a report to PowerPoint (PPT)....\",\"citingSources\":[{\"sourceName\":\"internet\",\"sourceType\":\"internet\",\"sourceValue\":[]}],\"rawResponse\":{\"response\":\"Raw response\",\"sources\":[],\"source_nodes\":[]}}"
            );
            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                    It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/internet")), default))
                .ReturnsAsync(internet);

            mockOrchestrationContext
                .Setup(c => c.CallActivityAsync<string>(nameof(SummarizeActivity), It.IsAny<SummarizeActivityInput>(), default))
                .ReturnsAsync("Summarized response");

            var mockCommon = new Mock<ICommon>();
            mockCommon.Setup(c => c.GetTokenData(It.IsAny<ILogger>(), It.IsAny<String>())).Returns(new Token()
            {
                UniqueName = "Daniel.Rivera@ey.com",
                Email = "Daniel.Rivera@ey.com",
                FamilyName = "Rivera",
                GivenName = "Daniel",
                UserType = "Internal",
                Oid = "1b8b3c2d-8274-47c1-9c07-524591c4b721",
                CeOid = "a6a33b9a-a1b2-402e-9071-3186a5f39788",
                Upn = "Daniel.Rivera@ey.com",
                SpUrl = "https://eyus.sharepoint.com/sites/ce5-productdevlop-ceqaprjco-qa",
                PoAppUrl = "https://eycapitaledge-qa.ey.com/productdevlop-ceqaprjco/ce4",
                PoApiUrl = "https://eycapitaledge-qa.ey.com/productdevlop-ceqaprjco-ce4/api",
                CopilotAppUrl = "https://eycapitaledge-qa.ey.com/productdevlop-ceqaprjco/copilot",
                CopilotApiUrl = "https://eycapitaledge-qa.ey.com/productdevlop-ceqaprjco/copilot/api",
                ProjectId = "a3151756-3768-4abe-a28a-ca51bed4f659",
                Scope = new List<string> { "ey-guidance", "internet", "project-docs", "project-data" },
                Nbf = 1715173550,
                Exp = 1715177150,
                Iat = 1715173550,
                Iss = "https://eycapitaledge-dev.ey.com",
                Aud = "a3151756-3768-4abe-a28a-ca51bed4f659"
            });
            mockCommon.Setup(c => c.Deserialize<ProjectContextResponse>(It.IsAny<string>())).Returns(new ProjectContextResponse()
            {
                Id = 1,
                Title = "",
                Key = "PROJECT_CONTEXT",
                Value = "Project description",
                IsEnabled = false
            });
            mockCommon.Setup(c => c.Deserialize<ChatHistoryResponse>(It.IsAny<string>())).Returns(new ChatHistoryResponse()
            {
                Key = "user:Daniel.Rivera@ey.com:chatid:b43fde43-91f0-4521-96f6-68b69f38d517",
                Content = new List<Models.ChatHistoryService.Message> { 
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 1,
                        Role = "user",
                        Content = "When was the deal announced?",
                        Sources = ["internet"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null
                    },
                    new Models.ChatHistoryService.Message()
                    {
                        MessageId = 2,
                        Role = "assistant",
                        Content = "Internet\nSorry, I am unable to answer your question at the moment. Please try again later.\n",
                        Sources = ["internet"],
                        Context = null,
                        CreatedTime = new DateTime(),
                        Response = null,
                        LastUpdatedTime = new DateTime()
                    }
                }
            });
            mockCommon.Setup(c => c.Deserialize<BackendResponse>(It.IsAny<string>())).Returns(new BackendResponse()
            {
                Backend = "internet",
                Sql = null,
                Response = "To export a report to PowerPoint (PPT)....",
                CitingSources = new List<CitingSource> { new CitingSource() { SourceName = "internet", SourceType = "internet", SourceValue = new List<string>() } },
                RawReponse = null
            });
            mockCommon.Setup(c => c.GetEnvironmentVariable(It.IsAny<ILogger>(), "ProjectDataReturnSql")).Returns("true");

            // Act
            var chatOrchestrator = new Orchestrators.ChatOrchestrator(mockCommon.Object);
            var result = await chatOrchestrator.RunOrchestrator(mockOrchestrationContext.Object);

            // Assert
            Assert.NotNull(result);
            mockOrchestrationContext.Verify(c => c.CallActivityAsync<SerializableHttpResponseMessage>(nameof(HttpCallActivity),
                It.Is<HttpCallActivityInput>(s => s.SerializedRequest.RequestUri.Contains("/internet")), default), Times.Once);
        }
    }
}

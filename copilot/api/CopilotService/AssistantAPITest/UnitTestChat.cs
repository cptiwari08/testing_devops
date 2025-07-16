using AssistantAPITest.Dependency;
using AssistantAPITest.DependencyMockups;
using EY.CE.Copilot.API.Clients;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Handler;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Contexts;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using System.Configuration;

namespace AssistantAPITest
{
    public class TestsChat
    {
        private ChatClient client;
        //create private variables for Setup variables
        private IAppLoggerService appinsightMock;
        private ISessionClient sessionMock;
        private IConfiguration configuration;
        private IHttpContextAccessor httpContextMock;
        private CopilotContext context;
        private IFireForgetRepositoryHandler fireForgetRepositoryHandler;
        [SetUp]
        public void Setup()
        {            
            appinsightMock = new AppLoggerServiceMockup().appinsightMock.Object;
            sessionMock = new SessionClientMockup().sessionClient.Object;
            configuration = new ConfigurationDependency().configuration;
            httpContextMock = new HttpContextMockup().httpContextAccessorMock.Object;
            context = new CopilotContextDependency().context;
            fireForgetRepositoryHandler = new Dependency.FireForgetRepositoryHandler();           
        }

        [Test]
        public async Task SaveMessageFeedback()
        {
            
            //Generate dummy data
            Chat.MessageFeedback feedback = new Chat.MessageFeedback()
            {
                ChatId = "test-project-id",
                MessageId = 2,
                IsLiked = true,
                FeedbackText = "Negativee",
                InstanceId = "123-123-123"
            };
            client = new ChatClient(configuration, null, sessionMock,null, context, httpContextMock, appinsightMock, null, null, null,null);
            await client.PostFeedback(feedback);
            Assert.Pass();
        }

        [Test]
        public async Task OneInputSource()
        {
            Mock<IOrchestratorClient> orchestratorMock = new Mock<IOrchestratorClient>();
            orchestratorMock.Setup(Setup => Setup.GetRequest(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new ContentResult
            {
                Content = "{\r\n  \"name\": \"ChatOrchestrator\",\r\n  \"instanceId\": \"5d6b6ffa639b45a3b0faca47a95db5fe\",\r\n  \"runtimeStatus\": \"Completed\",\r\n  \"input\": {\r\n    \"messageId\": 1,\r\n    \"chatId\": \"623ee719-e79e-41a8-934f-86ba330ddf28\",\r\n    \"question\": \"How do you add a new field to the workplan?\",\r\n    \"sources\": [\r\n      \"ey-guidance\"\r\n    ],\r\n    \"inputSources\": [\r\n      \"ey-guidance\"\r\n    ]\r\n  },\r\n  \"customStatus\": null,\r\n  \"output\": {\r\n    \"messageId\": 2,\r\n    \"role\": \"assistant\",\r\n    \"content\": \"Summary\\nI’m sorry, but I don’t have enough information to answer that. Can you please ask a more specific question or prompt?\\n\\n\\nEY Guidance\\nThe contexts provided do not contain specific information on how to add a new field to the workplan.\\n\\n\\nHelp Copilot\\nThe contexts provided do not contain specific information on how to add a new field to the workplan.\\n\\n\\nEY IP\\nNo Content\\n\\n\\n\",\r\n    \"sources\": [\r\n      \"ey-guidance\"\r\n    ],\r\n    \"inputSources\": [\r\n      \"ey-guidance\"\r\n    ],\r\n    \"context\": {\r\n      \"suggestion\": {\r\n        \"id\": \"158\",\r\n        \"sqlQuery\": null,\r\n        \"source\": \"ey-guidance\"\r\n      },\r\n      \"isMessageLiked\": null\r\n    },\r\n    \"response\": [\r\n      {\r\n        \"sourceName\": \"summary\",\r\n        \"content\": \"I’m sorry, but I don’t have enough information to answer that. Can you please ask a more specific question or prompt?\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": null\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance\",\r\n        \"content\": \"The contexts provided do not contain specific information on how to add a new field to the workplan.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": null\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance-help-copilot\",\r\n        \"content\": \"The contexts provided do not contain specific information on how to add a new field to the workplan.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": [\r\n          {\r\n            \"sourceName\": \"ey-guidance-help-copilot\",\r\n            \"sourceType\": \"documents\",\r\n            \"sourceValue\": []\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance-ey-ip\",\r\n        \"content\": \"No Content\",\r\n        \"status\": \"204\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": []\r\n      }\r\n    ],\r\n    \"createdTime\": \"2024-06-26T09:41:31.0711548Z\",\r\n    \"lastUpdatedTime\": \"2024-06-26T09:41:48.6684826Z\"\r\n  },\r\n  \"createdTime\": \"2024-06-26T09:41:30Z\",\r\n  \"lastUpdatedTime\": \"2024-06-26T09:41:48Z\"\r\n}",
                ContentType = "application/json",
                StatusCode = 200
            });
            client = new ChatClient(configuration, orchestratorMock.Object, sessionMock, null, context, httpContextMock, appinsightMock, null, fireForgetRepositoryHandler, null, null);
            Chat.Status input = new Chat.Status { Uri = "https://eycapitaledgeapim-dev.ey.com/ceassistant-orchestrator-status/instances/7dd2ae974e29438ea2334f77558a53e7" };
            var response =(ContentResult) await client.GetStatus("", input, "623ee719-e79e-41a8-934f-86ba330ddf28");
            var output = JsonConvert.DeserializeObject<Chat.Output>(response.Content);
            if (output.Response.Count <2)
                Assert.Fail();
            if (output.Response[0].SourceName != "summary")
                Assert.Fail();
            if (output.Response[1].SourceName != "ey-guidance")
                Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public async Task TwoInputSourceTwoOutput()
        {
            Mock<IOrchestratorClient> orchestratorMock = new Mock<IOrchestratorClient>();
            orchestratorMock.Setup(Setup => Setup.GetRequest(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new ContentResult
            {
                Content = "{\r\n  \"name\": \"ChatOrchestrator\",\r\n  \"instanceId\": \"7dd2ae974e29438ea2334f77558a53e7\",\r\n  \"runtimeStatus\": \"Completed\",\r\n  \"input\": {\r\n    \"messageId\": 17,\r\n    \"chatId\": \"623ee719-e79e-41a8-934f-86ba330ddf28\",\r\n    \"question\": \"what is EY?\",\r\n    \"sources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ],\r\n    \"inputSources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ]\r\n  },\r\n  \"customStatus\": null,\r\n  \"output\": {\r\n    \"messageId\": 18,\r\n    \"role\": \"assistant\",\r\n    \"content\": \"Summary\\nEY, or Ernst & Young Global Limited, is a multinational professional services network headquartered in London, England. It is one of the \\\"Big Four\\\" accounting firms and offers a wide array of services, including auditing, tax, consulting, and advisory services to various entities like corporations, governments, and individuals. Recognized as a global leader in these fields, EY aims to build trust and confidence in capital markets and economies globally. EY also refers to member firms of Ernst & Young Global Limited, each a separate legal entity, developing leaders who deliver to their stakeholders.\\n\\n\\nInternet\\nEY refers to Ernst & Young Global Limited, commonly known as EY. It is a multinational professional services network with headquarters in London, England. EY is one of the largest professional services networks in the world and is considered one of the \\\"Big Four\\\" accounting firms. The organization provides a wide range of services, including auditing, tax, consulting, and advisory services to its clients which include corporations, governments, and individuals.\\n\\n\\nEY Guidance\\nEY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\\n\\n\\nHelp Copilot\\nEY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\\n\\n\\nEY IP\\nNo Content\\n\\n\\n\",\r\n    \"sources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ],\r\n    \"inputSources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ],\r\n    \"context\": {\r\n      \"isMessageLiked\": null\r\n    },\r\n    \"response\": [\r\n      {\r\n        \"sourceName\": \"summary\",\r\n        \"content\": \"EY, or Ernst & Young Global Limited, is a multinational professional services network headquartered in London, England. It is one of the \\\"Big Four\\\" accounting firms and offers a wide array of services, including auditing, tax, consulting, and advisory services to various entities like corporations, governments, and individuals. Recognized as a global leader in these fields, EY aims to build trust and confidence in capital markets and economies globally. EY also refers to member firms of Ernst & Young Global Limited, each a separate legal entity, developing leaders who deliver to their stakeholders.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": null\r\n      },\r\n      {\r\n        \"sourceName\": \"internet\",\r\n        \"content\": \"EY refers to Ernst & Young Global Limited, commonly known as EY. It is a multinational professional services network with headquarters in London, England. EY is one of the largest professional services networks in the world and is considered one of the \\\"Big Four\\\" accounting firms. The organization provides a wide range of services, including auditing, tax, consulting, and advisory services to its clients which include corporations, governments, and individuals.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": [\r\n          {\r\n            \"sourceName\": \"internet\",\r\n            \"sourceType\": \"internet\",\r\n            \"sourceValue\": []\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance\",\r\n        \"content\": \"EY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": null\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance-help-copilot\",\r\n        \"content\": \"EY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": [\r\n          {\r\n            \"sourceName\": \"ey-guidance-help-copilot\",\r\n            \"sourceType\": \"documents\",\r\n            \"sourceValue\": [\r\n              {\r\n                \"active\": false,\r\n                \"documentGuid\": \"ea575235-f382-49e6-bcd4-28c4f82d3597\",\r\n                \"documentName\": \"Corporate Finance Platform Overview.pdf\",\r\n                \"isVideoDocument\": false,\r\n                \"pages\": [\r\n                  {\r\n                    \"pageNumber\": 6,\r\n                    \"metaData\": null\r\n                  }\r\n                ],\r\n                \"version\": \"v1.0\",\r\n                \"videoUrl\": null\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance-ey-ip\",\r\n        \"content\": \"No Content\",\r\n        \"status\": \"204\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": []\r\n      }\r\n    ],\r\n    \"createdTime\": \"2024-06-26T09:57:20.6391626Z\",\r\n    \"lastUpdatedTime\": \"2024-06-26T09:57:59.5705595Z\"\r\n  },\r\n  \"createdTime\": \"2024-06-26T09:57:20Z\",\r\n  \"lastUpdatedTime\": \"2024-06-26T09:57:59Z\"\r\n}",
                ContentType = "application/json",
                StatusCode = 200
            });
            client = new ChatClient(configuration, orchestratorMock.Object, sessionMock,null, context, httpContextMock, appinsightMock, null, fireForgetRepositoryHandler, null, null);
            Chat.Status input = new Chat.Status { Uri = "https://eycapitaledgeapim-dev.ey.com/ceassistant-orchestrator-status/instances/7dd2ae974e29438ea2334f77558a53e7" };
            var response = (ContentResult)await client.GetStatus("", input, "623ee719-e79e-41a8-934f-86ba330ddf28");
            var output = JsonConvert.DeserializeObject<Chat.Output>(response.Content);
            if (output.Response.Count < 3)
                Assert.Fail();
            if (output.Response[0].SourceName != "summary")
                Assert.Fail();
            if (output.Response[1].SourceName != "internet" || output.Response[1].status != "200")
                Assert.Fail();
            if (output.Response[2].SourceName != "ey-guidance" || output.Response[2].status != "200")
                Assert.Fail();
            Assert.Pass();
        }

        [Test]
        public async Task TwoInputSourceOneOutput()
        {
            Mock<IOrchestratorClient> orchestratorMock = new Mock<IOrchestratorClient>();
            orchestratorMock.Setup(Setup => Setup.GetRequest(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new ContentResult
            {
                Content = "{\r\n  \"name\": \"ChatOrchestrator\",\r\n  \"instanceId\": \"7dd2ae974e29438ea2334f77558a53e7\",\r\n  \"runtimeStatus\": \"Completed\",\r\n  \"input\": {\r\n    \"messageId\": 17,\r\n    \"chatId\": \"623ee719-e79e-41a8-934f-86ba330ddf28\",\r\n    \"question\": \"what is EY?\",\r\n    \"sources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ],\r\n    \"inputSources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ]\r\n  },\r\n  \"customStatus\": null,\r\n  \"output\": {\r\n    \"messageId\": 18,\r\n    \"role\": \"assistant\",\r\n    \"content\": \"Summary\\nEY, or Ernst & Young Global Limited, is a multinational professional services network headquartered in London, England. It is one of the \\\"Big Four\\\" accounting firms and offers a wide array of services, including auditing, tax, consulting, and advisory services to various entities like corporations, governments, and individuals. Recognized as a global leader in these fields, EY aims to build trust and confidence in capital markets and economies globally. EY also refers to member firms of Ernst & Young Global Limited, each a separate legal entity, developing leaders who deliver to their stakeholders.\\n\\n\\nInternet\\nEY refers to Ernst & Young Global Limited, commonly known as EY. It is a multinational professional services network with headquarters in London, England. EY is one of the largest professional services networks in the world and is considered one of the \\\"Big Four\\\" accounting firms. The organization provides a wide range of services, including auditing, tax, consulting, and advisory services to its clients which include corporations, governments, and individuals.\\n\\n\\nEY Guidance\\nEY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\\n\\n\\nHelp Copilot\\nEY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\\n\\n\\nEY IP\\nNo Content\\n\\n\\n\",\r\n    \"sources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ],\r\n    \"inputSources\": [\r\n      \"internet\",\r\n      \"ey-guidance\"\r\n    ],\r\n    \"context\": {\r\n      \"isMessageLiked\": null\r\n    },\r\n    \"response\": [\r\n      {\r\n        \"sourceName\": \"summary\",\r\n        \"content\": \"EY, or Ernst & Young Global Limited, is a multinational professional services network headquartered in London, England. It is one of the \\\"Big Four\\\" accounting firms and offers a wide array of services, including auditing, tax, consulting, and advisory services to various entities like corporations, governments, and individuals. Recognized as a global leader in these fields, EY aims to build trust and confidence in capital markets and economies globally. EY also refers to member firms of Ernst & Young Global Limited, each a separate legal entity, developing leaders who deliver to their stakeholders.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": null\r\n      },\r\n      {\r\n        \"sourceName\": \"internet\",\r\n        \"content\": \"This datasource is not valid\",\r\n        \"status\": \"500\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": [\r\n          {\r\n            \"sourceName\": \"internet\",\r\n            \"sourceType\": \"internet\",\r\n            \"sourceValue\": []\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance\",\r\n        \"content\": \"EY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": null\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance-help-copilot\",\r\n        \"content\": \"EY is a global leader in assurance, tax, transaction and advisory services. The insights and quality services they deliver help build trust and confidence in the capital markets and in economies the world over. They develop outstanding leaders who team to deliver on their promises to all of their stakeholders. EY refers to the global organization, and may refer to one or more, of the member firms of Ernst & Young Global Limited, each of which is a separate legal entity.\",\r\n        \"status\": \"200\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": [\r\n          {\r\n            \"sourceName\": \"ey-guidance-help-copilot\",\r\n            \"sourceType\": \"documents\",\r\n            \"sourceValue\": [\r\n              {\r\n                \"active\": false,\r\n                \"documentGuid\": \"ea575235-f382-49e6-bcd4-28c4f82d3597\",\r\n                \"documentName\": \"Corporate Finance Platform Overview.pdf\",\r\n                \"isVideoDocument\": false,\r\n                \"pages\": [\r\n                  {\r\n                    \"pageNumber\": 6,\r\n                    \"metaData\": null\r\n                  }\r\n                ],\r\n                \"version\": \"v1.0\",\r\n                \"videoUrl\": null\r\n              }\r\n            ]\r\n          }\r\n        ]\r\n      },\r\n      {\r\n        \"sourceName\": \"ey-guidance-ey-ip\",\r\n        \"content\": \"No Content\",\r\n        \"status\": \"204\",\r\n        \"sqlQuery\": null,\r\n        \"citingSources\": []\r\n      }\r\n    ],\r\n    \"createdTime\": \"2024-06-26T09:57:20.6391626Z\",\r\n    \"lastUpdatedTime\": \"2024-06-26T09:57:59.5705595Z\"\r\n  },\r\n  \"createdTime\": \"2024-06-26T09:57:20Z\",\r\n  \"lastUpdatedTime\": \"2024-06-26T09:57:59Z\"\r\n}",
                ContentType = "application/json",
                StatusCode = 200
            });
            client = new ChatClient(configuration, orchestratorMock.Object, sessionMock, null, context, httpContextMock, appinsightMock, null, fireForgetRepositoryHandler, null, null);
            Chat.Status input = new Chat.Status { Uri = "https://eycapitaledgeapim-dev.ey.com/ceassistant-orchestrator-status/instances/7dd2ae974e29438ea2334f77558a53e7" };
            var response = (ContentResult)await client.GetStatus("", input, "623ee719-e79e-41a8-934f-86ba330ddf28");
            var output = JsonConvert.DeserializeObject<Chat.Output>(response.Content);
            if (output.Response.Count < 3)
                Assert.Fail();
            if (output.Response[0].SourceName != "summary")
                Assert.Fail();
            if (output.Response[1].SourceName != "internet" || output.Response[1].status!="500")
                Assert.Fail();
            if (output.Response[2].SourceName != "ey-guidance" || output.Response[2].status != "200")
                Assert.Fail();
            Assert.Pass();
        }
    }
}
using System.Diagnostics.CodeAnalysis;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    [OpenApiExample(typeof(ChatQuestionInputExample))]
    public class ChatQuestionInput
    {
        [JsonProperty("messageId")]
        public int MessageId { get; set; }
        [JsonProperty("chatId")]
        public Guid ChatId { get; set; }
        [JsonProperty("question")]
        public required string Question { get; set; }
        [JsonProperty("sources")]
        public required string[] Sources { get; set; }
        [JsonProperty("context")]
        public Dictionary<string, object?>? Context { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ChatQuestionInputExample : OpenApiExample<ChatQuestionInput>
    {
        public override IOpenApiExample<ChatQuestionInput> Build(NamingStrategy? namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("ChatQuestionInputExample", new ChatQuestionInput
            {
                MessageId = 1,
                ChatId = Guid.NewGuid(),
                Question = "What Cost Reduction value levers are available? Give me a list of five of them",
                Sources = ["ey-guidance", "internet", "project-docs", "project-data"],
                Context = new Dictionary<string, object?>
                {
                    { "suggestion", new {
                        id = "10001",
                        sqlQuery = "select * from workplan",
                        source = "project-data"
                    }}
                }
            }));
            return this;
        }
    }
}

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Models
{
    [ExcludeFromCodeCoverage]
    [OpenApiExample(typeof(ChatQuestionBodyResponseExample))]
    public class ChatQuestionBodyResponse
    {
        public required string Id { get; set; }
        public required string PurgeHistoryDeleteUri { get; set; }
        public required string SendEventPostUri { get; set; }
        public required string StatusQueryGetUri { get; set; }
        public required string TerminatePostUri { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ChatQuestionBodyResponseExample : OpenApiExample<ChatQuestionBodyResponse>
    {
        public override IOpenApiExample<ChatQuestionBodyResponse> Build(NamingStrategy? namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("ChatQuestionBodyResponseExample", new ChatQuestionBodyResponse
            {
                Id = "d5799de7336845c28a46e9bf8396d461",
                PurgeHistoryDeleteUri = "/runtime/webhooks/durabletask/instances/d5799de7336845c28a46e9bf8396d461?code={code}",
                SendEventPostUri = "runtime/webhooks/durabletask/instances/d5799de7336845c28a46e9bf8396d461/raiseEvent/{eventName}?code={code}",
                StatusQueryGetUri = "runtime/webhooks/durabletask/instances/d5799de7336845c28a46e9bf8396d461?code={code}",
                TerminatePostUri = "runtime/webhooks/durabletask/instances/d5799de7336845c28a46e9bf8396d461/terminate?reason={{text}}}&code={code}"
            }));
            return this;
        }
    }
}

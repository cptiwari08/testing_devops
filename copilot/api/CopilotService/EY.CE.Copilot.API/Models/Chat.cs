using EY.CE.Copilot.API.Static;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EY.CE.Copilot.API.Models
{
    public class Chat
    {
        public class AppInfo
        {
            [JsonProperty("key")]
            public string? Key { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("teamTypeIds")]
            public List<int?>? TeamTypeIds { get; set; }
        }

        public class ActivePOApp
        {
            [JsonProperty("key")]
            public string? Key { get; set; }

            [JsonProperty("name")]
            public string? Name { get; set; }
        }

        public class CitingSource
        {
            [JsonProperty("sourceName")]
            public string? SourceName { get; set; }

            [JsonProperty("sourceType")]
            public string? SourceType { get; set; }

            [JsonProperty("sourceValue")]
            public List<Object>? SourceValue { get; set; }
        }

        public class Context
        {
            [JsonProperty("suggestion", NullValueHandling = NullValueHandling.Ignore)]
            public Suggestion? Suggestion { get; set; }

            [JsonProperty("isMessageLiked", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsMessageLiked { get; set; }

            [JsonProperty("consent", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Consent { get; set; }

            [JsonProperty("documents")]
            public IEnumerable<string>? Documents { get; set; }

            [JsonProperty("excludeDocuments")]
            public IEnumerable<string>? ExcludeDocuments { get; set; }

            [JsonProperty("appInfo")]
            public AppInfo? AppInfo { get; set; }

            [JsonProperty("activePOApps")]
            public List<ActivePOApp>? ActivePOApps { get; set; }
        }

        public class Status
        {
            [JsonProperty("uri")]
            public string? Uri { get; set; }
            [JsonProperty("messageId")]
            public int? MessageId { get; set; }
            [JsonProperty("instanceId")]
            public string? InstanceId { get; set; }
        }

        public class Input
        {
            [JsonProperty("messageId")]
            public int? MessageId { get; set; }

            [JsonProperty("chatId")]
            public string? ChatId { get; set; }

            [SpecialCharNotAllowed()]
            [ScriptNotAllowed()]
            [StringLength(255)]
            [JsonProperty("question")]
            public string? Question { get; set; }

            [JsonProperty("sources")]
            public List<string>? Sources { get; set; }

            [JsonProperty("inputSources")]
            public List<string>? InputSources { get; set; }
            
            [JsonProperty("context")]
            public Context? Context { get; set; }

        }

        public class Output
        {
            [JsonProperty("messageId")]
            public int? MessageId { get; set; }

            [JsonProperty("role")]
            public string? Role { get; set; }

            [JsonProperty("content")]
            public string? Content { get; set; }

            [JsonProperty("sources")]
            public List<string>? Sources { get; set; }

            [JsonProperty("inputSources")]
            public List<string>? InputSources { get; set; }

            [JsonProperty("context")]
            public Context? Context { get; set; }

            [JsonProperty("response")]
            public List<Response>? Response { get; set; }

            [JsonProperty("createdTime")]
            public DateTime? CreatedTime { get; set; }

            [JsonProperty("lastUpdatedTime")]
            public DateTime? LastUpdatedTime { get; set; }

            [JsonProperty("followUpSuggestions")]
            public List<Object>? FollowUpSuggestions { get; set; }

            [JsonProperty("InstanceId")]
            public string? InstanceId { get; set; }

        }

        public class Page
        {
            [JsonProperty("pageNumber")]
            public int? PageNumber { get; set; }

            [JsonProperty("metaData")]
            public object? MetaData { get; set; }
        }
        public class Response
        {
            [JsonProperty("sourceName")]
            public string SourceName { get; set; }

            [JsonProperty("content")]
            public string content { get; set; }

            [JsonProperty("status")]
            public string status { get; set; }

            [JsonProperty("sqlQuery")]
            public object sqlQuery { get; set; }

            [JsonProperty("citingSources")]
            public List<CitingSource> citingSources { get; set; }
        }


        public class ResponseObject
        {
            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("instanceId")]
            public string? InstanceId { get; set; }

            [JsonProperty("runtimeStatus")]
            public string? RuntimeStatus { get; set; }

            [JsonProperty("input")]
            public Input? Input { get; set; }

            [JsonProperty("customStatus")]
            public string? CustomStatus { get; set; }

            [JsonProperty("output")]
            public Output? Output { get; set; }

            [JsonProperty("createdTime")]
            public DateTime? CreatedTime { get; set; }

            [JsonProperty("lastUpdatedTime")]
            public DateTime? LastUpdatedTime { get; set; }
        }

        public class Suggestion
        {
            [JsonProperty("id")]
            public string? Id { get; set; }

            [JsonProperty("sqlQuery")]
            public string? SqlQuery { get; set; }

            [JsonProperty("source")]
            public string? Source { get; set; }
        }

        public class Message
        {
            [JsonProperty("messageId")]
            public int? MessageId { get; set; }

            public string? InstanceId { get; set; }

            [JsonProperty("role")]
            public string? Role { get; set; }

            [JsonProperty("content")]
            public string? Content { get; set; }

            [JsonProperty("sources")]
            public List<string>? Sources { get; set; }

            [JsonProperty("inputSources")]
            public List<string>? InputSources { get; set; }


            [JsonProperty("context")]
            public Context? Context { get; set; }

            [JsonProperty("createdTime")]
            public DateTime? CreatedTime { get; set; }

            [JsonProperty("response")]
            public List<Response> Response { get; set; }

            [JsonProperty("lastUpdatedTime")]
            public DateTime? LastUpdatedTime { get; set; }

            public string? Status { get; set; }
            public int? StatusCode { get; set; }

            [JsonProperty("followUpSuggestions")]
            public List<Object>? FollowUpSuggestions { get; set; }
        }

        public class MessageFeedback
        {
            public int? ID { get; set; }
            public bool? IsLiked { get; set; }
            public string ChatId { get; set; }
            public long MessageId { get; set; }
            public string? InstanceId { get; set; }

            [SpecialCharNotAllowed()]
            [ScriptNotAllowed()]
            [StringLength(1000)]
            public string? FeedbackText { get; set; }
            public bool? Consent { get; set; }=false;
        }

        public class CopilotFeedback
        {
            public int Rating { get; set; }

            [SpecialCharNotAllowed()]
            [ScriptNotAllowed()]
            [StringLength(1000)]
            public string? FeedbackText { get; set; }
        }

        public class OrchestratorStatus
        {
            public string Id { get; set; }
            public string PurgeHistoryDeleteUri { get; set; }
            public string SendEventPostUri { get; set; }
            public string StatusQueryGetUri { get; set; }
            public string TerminatePostUri { get; set; }
            public string SuspendPostUri { get; set; }
            public string ResumePostUri { get; set; }
        }
        //write a method to convert Chat.output to Chat.Message which takes chat.output as parameter
        public static Message ConvertOutputToMessage(ResponseObject response)
        {
            Output output = response.Output;
            return new Message
            {
                MessageId = output.MessageId,
                InstanceId = response.InstanceId,
                Role = output.Role,
                Content = output.Content,
                Sources = output.Sources,
                InputSources = output.InputSources,
                Context = output.Context,
                Response = output.Response,
                CreatedTime = output.CreatedTime,
                LastUpdatedTime = output.LastUpdatedTime,
                FollowUpSuggestions = output.FollowUpSuggestions
            };
        }
    }
}

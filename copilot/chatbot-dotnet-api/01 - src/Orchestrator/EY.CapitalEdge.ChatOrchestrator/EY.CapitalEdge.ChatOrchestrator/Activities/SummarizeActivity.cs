using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Interfaces;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Models;
using System.Text.Json;
using EY.CapitalEdge.ChatOrchestrator.Services;

namespace EY.CapitalEdge.ChatOrchestrator.Activities
{
    public class SummarizeActivity
    {
        private readonly IOpenAIService _openAiService;
        private readonly Services.ILoggerProvider _loggerProvider;
        private readonly IStopwatch _stopwatch;

        private const string ProjectFriendlyId = "projectFriendlyId";

        public SummarizeActivity(IOpenAIServiceFactory openAIServiceFactory, Services.ILoggerProvider loggerProvider, IStopwatch stopwatch)
        {
            _openAiService = openAIServiceFactory.GetService("GenericOpenAIService");
            _loggerProvider = loggerProvider;
            _stopwatch = stopwatch;
        }

        /// <summary>
        /// Summarization activity using OpenAI
        /// </summary>
        /// <param name="task">task</param>
        /// <param name="executionContext">executionContext</param>
        /// <returns>Summarization</returns>
        [Function(nameof(SummarizeActivity))]
        public async Task<string> Run([ActivityTrigger] SummarizeActivityInput input, FunctionContext executionContext)
        {
            const string errorMessage = "An error occurred during the summarization process. Unable to generate a summary for the provided task.";
            ILogger log = _loggerProvider.GetLogger(executionContext, "SummarizeActivity");
            ILogger logger = new CustomLogger(log, $"[instanceId:{input.InstanceId}] [chatId:{input.ChatId}] [{ProjectFriendlyId}:{input.ProjectFriendlyId}]");
            logger.LogInformation("Getting summarization.");

            string? summarize = null;

            try
            {
                _stopwatch.Start();

                var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromUser(input.Task)
                    },
                    Model = OpenAI.ObjectModels.Models.Gpt_4
                });

                _stopwatch.Stop();
                TimeSpan timeElapsed = _stopwatch.Elapsed;

                if (!completionResult.Successful)
                {
                    logger.LogError("Error getting summarization, Status code: {StatusCode}, Time: {Time} ms, Details: {Details}", 
                        completionResult.HttpStatusCode, timeElapsed.TotalMilliseconds, JsonSerializer.Serialize(completionResult));
                    return errorMessage;
                }

                logger.LogInformation("completion_tokens: {0}, prompts_tokens: {1}, total_tokens: {2}, http_status_code: {3}, time: {4} ms, source: summarization",
                    completionResult.Usage.CompletionTokens,
                    completionResult.Usage.PromptTokens,
                    completionResult.Usage.TotalTokens,
                    completionResult.HttpStatusCode,
                    timeElapsed.TotalMilliseconds
                );

                summarize = completionResult.Choices[0].Message.Content;

                return summarize ?? "No response";
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{ErrorMessage}. Exception details: {ExceptionMessage}. Task: {Task}", errorMessage, ex.Message, input.Task);
                return errorMessage;
            }
        }
    }
}
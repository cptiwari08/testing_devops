using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using Started.Activities;
using Started.Models;

namespace Started.Orchestrations
{
    public class CapitalEdgeChatOrchestrator
    {
        [Function(nameof(CapitalEdgeChatOrchestrator))]
        public async Task<QuestionInfo> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(CapitalEdgeChatOrchestrator));
            logger.LogInformation("Orchestrator is running.");

            QuestionInfo? input = context.GetInput<QuestionInfo>();
            if (input is null || string.IsNullOrWhiteSpace(input.Question) || string.IsNullOrWhiteSpace(input.UserContext))
                throw new ArgumentNullException(nameof(context), "QuestionInfo was not provided correctly");

            string intention = await context.CallActivityAsync<string>(nameof(GetQuestionIntentionActivity), input.Question);
            List<string> sources = await context.CallActivityAsync<List<string>>(nameof(GetDataSourcesFromIntentionActivity), intention);

            var tasks = new List<Task<string>>();
            foreach (var source in sources)
            {
                tasks.Add(context.CallActivityAsync<string>(nameof(HttpCallActivity), source));
            }

            await Task.WhenAll(tasks);

            // Logic to get the answer from the question based on the data sources

            input.Answer = await context.CallActivityAsync<string>(nameof(GetQuestionAnswerActivity), input);
            await context.CallActivityAsync(nameof(PushQuestionAnswerToQueueActivity), input);

            return input;
        }
    }
}

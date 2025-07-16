using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Started.Models;

namespace Started.Activities
{
    public class PushQuestionAnswerToQueueActivity
    {
        [Function(nameof(PushQuestionAnswerToQueueActivity))]
        public async Task Run([ActivityTrigger] QuestionInfo questionInfo, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("PushQuestionAnswerToQueueActivity");
            logger.LogInformation("Pushing question with answer to queue: {questionInfo} .", questionInfo);
        }
    }
}
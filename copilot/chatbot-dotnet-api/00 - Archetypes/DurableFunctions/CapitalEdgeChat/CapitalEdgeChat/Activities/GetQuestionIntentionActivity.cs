using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Started.Activities
{
    public class GetQuestionIntentionActivity
    {
        [Function(nameof(GetQuestionIntentionActivity))]
        public async Task<string> Run([ActivityTrigger] string question, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("GetQuestionIntentionActivity");
            logger.LogInformation("Getting intetion from question: {question}.", question);

            return "Intetion";
        }
    }
}
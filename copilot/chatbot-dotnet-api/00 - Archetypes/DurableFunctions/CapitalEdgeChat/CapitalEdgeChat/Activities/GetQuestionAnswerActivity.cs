using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Started.Models;
using System.Net.Http;

namespace Started.Activities
{
    public class GetQuestionAnswerActivity
    {
        [Function(nameof(GetQuestionAnswerActivity))]
        public async Task<string> Run([ActivityTrigger] QuestionInfo question, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("GetQuestionAnswerActivity");
            logger.LogInformation("Getting question answer.");

            // Logic to get the answer from the question

            return "Answer";
        }
    }
}
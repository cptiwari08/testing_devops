using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Started.Activities
{
    public class GetDataSourcesFromIntentionActivity
    {
        [Function(nameof(GetDataSourcesFromIntentionActivity))]
        public async Task<List<string>> Run([ActivityTrigger] string intention, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("GetDataSourcesFromIntentionActivity");
            logger.LogInformation("Getting data sources from intention: {intention}.", intention);

            List<string> dataSources = new()
            {
                "http://localhost:7019/api/product",
                "http://localhost:7019/api/product"
            };

            return dataSources;
        }
    }
}
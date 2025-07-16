using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.DurableTask.Client;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Clients
{
    [ExcludeFromCodeCoverage]
    public static class PurgeInstanceHistoryClient
    {
        /// <summary>
        /// Purge instance history orchestrations older than 1 week
        /// </summary>
        /// <param name="client"></param>
        /// <param name="myTimer"></param>
        [Function("PurgeInstanceHistoryClient")]
        public async static Task Run(
            [DurableClient] DurableTaskClient client,
            [TimerTrigger("0,30 * * * *" // Every 30 minutes
            #if DEBUG 
                , RunOnStartup=true 
            #endif
            )] TimerInfo myTimer,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger(nameof(PurgeInstanceHistoryClient));
            logger.LogInformation("C# Timer trigger function executed at: {0}", DateTime.Now);

            if (myTimer.ScheduleStatus is not null)
            {
                logger.LogInformation("Next timer schedule at: {0}", myTimer.ScheduleStatus.Next);
            }

            int purgeInstanceHistoryTimeInMinutes = int.TryParse(Environment.GetEnvironmentVariable("PurgeInstanceHistoryCreatedTo"), out int number) ? number : -30;
            
            var result = await client.PurgeInstancesAsync(
                    DateTime.MinValue,
                    DateTime.UtcNow.AddMinutes(purgeInstanceHistoryTimeInMinutes),
                    new List<OrchestrationRuntimeStatus>
                    {
                        OrchestrationRuntimeStatus.Completed,
                        OrchestrationRuntimeStatus.Failed,
                        OrchestrationRuntimeStatus.Terminated,
                        OrchestrationRuntimeStatus.Suspended
                    });

            logger.LogInformation("purgeInstanceHistoryTimeInMinutes: {0}, PurgeInstancesAsync result: {1}", purgeInstanceHistoryTimeInMinutes, result.PurgedInstanceCount);
        }
    }
}

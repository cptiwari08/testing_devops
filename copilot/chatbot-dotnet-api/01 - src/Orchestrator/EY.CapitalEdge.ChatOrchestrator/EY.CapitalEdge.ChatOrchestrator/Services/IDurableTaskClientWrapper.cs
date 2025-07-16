using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using System.Net;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface IDurableTaskClientWrapper
    {
        public void SetDurableTaskClient(DurableTaskClient client);
        Task<HttpResponseData> CreateCheckStatusResponseAsync(HttpRequestData request, string instanceId, HttpStatusCode statusCode, CancellationToken cancellation);
    }
}

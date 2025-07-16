using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    [ExcludeFromCodeCoverage]
    public class DurableTaskClientWrapper : IDurableTaskClientWrapper
    {
        private DurableTaskClient? _client;

        public void SetDurableTaskClient(DurableTaskClient client)
        {
            _client = client;
        }

        public Task<HttpResponseData> CreateCheckStatusResponseAsync(HttpRequestData request, string instanceId, HttpStatusCode statusCode, CancellationToken cancellation)
        {
            if (_client == null)
            {
                throw new InvalidOperationException("DurableTaskClient is not set.");
            }
            return _client.CreateCheckStatusResponseAsync(request, instanceId, statusCode, cancellation);
        }
    }
}

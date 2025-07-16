using Azure.Search.Documents;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface ISearchClientFactory
    {
        SearchClient CreateSearchClient(string serviceEndpoint, string credential, string indexName);
    }
}
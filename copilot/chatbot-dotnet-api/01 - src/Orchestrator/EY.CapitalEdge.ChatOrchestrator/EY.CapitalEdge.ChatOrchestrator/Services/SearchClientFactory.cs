using Azure.Search.Documents;
using Azure;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    [ExcludeFromCodeCoverage]
    public class SearchClientFactory : ISearchClientFactory
    {
        public SearchClient CreateSearchClient(string serviceEndpoint, string credential, string indexName)
        {
            return new SearchClient(
                new Uri(serviceEndpoint),
                indexName,
                new AzureKeyCredential(credential));
        }
    }
}

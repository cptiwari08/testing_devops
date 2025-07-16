using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using EY.CapitalEdge.ChatOrchestrator.Models;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    [ExcludeFromCodeCoverage]
    public class Suggestion : ISuggestion
    {
        private readonly ISearchClientFactory _searchClientFactory;
        private const string prjsuggestions = "metadata/prjsuggestions";

        public Suggestion(ISearchClientFactory searchClient)
        {
            _searchClientFactory = searchClient;
        }

        public async Task<List<Models.Suggestion>> GetEmbeddingsAsync(SearchClientDto searchClientDto, string filter)
        {
            string searchText = "*";
            var options = new SearchOptions
            {
                QueryType = SearchQueryType.Full,
                Select = {
                    "indexType",
                    $"{prjsuggestions}/idSuggestion", 
                    "chunk", 
                    "embedding", 
                    $"{prjsuggestions}/source",
                    $"{prjsuggestions}/appAffinity",
                    $"{prjsuggestions}/visibleToAssistant",
                    $"{prjsuggestions}/isIncluded"
                },
                Filter = filter,
                SearchMode = SearchMode.All
            };

            var searchClient = _searchClientFactory.CreateSearchClient(searchClientDto.ServiceEndpoint, searchClientDto.Credential, searchClientDto.IndexName);
            var results = await searchClient.SearchAsync<GenericIndex>(searchText, options);

            List<Models.Suggestion> embeddings = [];
            await foreach (SearchResult<GenericIndex> result in results.Value.GetResultsAsync())
            {
                GenericIndex doc = result.Document;
                embeddings.Add(new Models.Suggestion
                {
                    Id = doc.Metadata.PrjSuggestions.Id,
                    Chunk = doc.Chunk,
                    Embedding = doc.Embedding,
                    Source = doc.Metadata.PrjSuggestions.Source,
                    AppAffinity = doc.Metadata.PrjSuggestions.AppAffinity,
                    VisibleToAssistant = doc.Metadata.PrjSuggestions.VisibleToAssistant,
                    IsIncluded = doc.Metadata.PrjSuggestions.IsIncluded
                });
            }

            return embeddings;
        }
    }
}

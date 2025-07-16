using EY.CE.Copilot.API.Models;
namespace EY.CE.Copilot.API.Contracts
{
    public interface ISearchClient
    {
        public Task<List<QuerySearchOutput>> PerformSimilaritySearch(QuerySearchInput input);
    }
}

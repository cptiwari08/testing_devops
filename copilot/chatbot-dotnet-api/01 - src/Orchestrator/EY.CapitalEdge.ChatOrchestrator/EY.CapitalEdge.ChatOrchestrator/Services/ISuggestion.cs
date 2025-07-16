using EY.CapitalEdge.ChatOrchestrator.Models;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface ISuggestion
    {
        Task<List<Models.Suggestion>> GetEmbeddingsAsync(SearchClientDto searchClientDto, string filter);
    }
}
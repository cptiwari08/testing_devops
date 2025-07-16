using EY.CE.Copilot.API.Clients;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Contracts
{
    public interface ISuggestionsClient
    {
        Task<List<Suggestion>> GetAll(bool? filterVisibleToAssistant);
        Task<Suggestion> Get(int id);
        Task<List<QueryValidationResult>> Add(List<SuggestionInsert> suggestions);
        Task<List<QueryValidationResult>> Update(List<SuggestionUpdate> suggestion);
        Task Delete(int id);
        Task<List<Suggestion>> GetSuggestionsBySources(List<string> sources);
        Task<QueryValidationResult> ValidateQuerySyntax(string query);
    }
}

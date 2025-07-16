using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IPromptClient
    {
        Task<List<AssistantPrompt>> GetAll();
        Task<List<AssistantPrompt>> GetByQuery(string key, string agent);
        Task Add(List<PromptInsert> input);
        Task Update(List<PromptUpdate> input);
        Task Delete(int id);
    }
}

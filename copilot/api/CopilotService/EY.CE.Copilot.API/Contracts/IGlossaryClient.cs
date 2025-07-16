using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IGlossaryClient
    {
        Task<List<Glossary>> GetAll();
        Task Add(List<GlossaryInsert> input);
        Task Update(List<GlossaryUpdate> input);
        Task Delete(int id);
    }
}

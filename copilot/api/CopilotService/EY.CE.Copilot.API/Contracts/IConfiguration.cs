using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IConfig
    {
        Task<CopilotConfiguration> GetConfiguration(string key);
        Task UpdateConfiguration(CopilotConfiguration configuration);
        Task UpdateConfiguration(ConfigurationUpdate configuration);
    }
}

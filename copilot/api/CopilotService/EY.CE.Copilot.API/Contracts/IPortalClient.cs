using EY.CE.Copilot.API.Models;
using static EY.CE.Copilot.API.Models.Chat;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IPortalClient : IConfig
    {
        Task<ProjectData> GetProjectDetails();
        Task<PortalUser> GetUserDetails(string emailId);
        Task<User> GetMe(string emailId);
        Task<List<ActivePOApp>> GetApps(string token);
        Task TriggerWorkflows(AsyncOperationRequest request, string userEmail, string instanceId);
    }
}

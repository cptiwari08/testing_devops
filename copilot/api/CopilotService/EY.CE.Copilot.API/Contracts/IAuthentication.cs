using EY.CE.Copilot.API.Models;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IAuthentication
    {
        Task<AuthToken> GetSSPAuthToken();
    }
}

using Microsoft.AspNetCore.Mvc;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IOrchestratorClient
    {
        Task<IActionResult> GetRequest(string token, string endpoint);
        Task<IActionResult> ParameterizedGetRequest(string token, string endpoint, Dictionary<string, string> queryParameters);
        Task<IActionResult> PostRequest(string token, string endpoint, string jsonPayload);
    }
}

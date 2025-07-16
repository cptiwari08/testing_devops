using EY.CE.Copilot.API.Clients;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;
using System.Net;

namespace EY.CE.Copilot.API.Contracts
{
    public interface IContentGeneratorClient
    {
        Task<IEnumerable<ContentGeneratorQuery>> GetDataByFilter(string app, string generatorType);
        Task<object> GetDataFromRedis(string instanceId, ContentGenerator input);
        Task StartRequest(AsyncOperationRequest request, string instanceId);
        Task<HttpStatusCode> TerminateContentGenerator(AppType appType, string instanceId);
    }
}
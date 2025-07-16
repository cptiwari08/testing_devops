using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using Microsoft.Extensions.Logging;

namespace EY.CapitalEdge.ChatOrchestrator.Utils
{
    public interface ICommon
    {
        T Deserialize<T>(string? json);
        string GetEnvironmentVariable(ILogger logger, string variableName);
        Suggestion GetSuggestedQuestion(Dictionary<string, object?>? context);
        Token GetTokenData(ILogger logger, string token);
        bool IsThereAnySuggestedQuestion(Dictionary<string, object?>? context);
        string SerializeToCamelCase(object obj);
        bool ValidateToken(ILogger logger, string token, string issuer, string publicKey);
        public bool IsValidUrl(string urlString);
        public bool IsThereAnyAppInfo(Dictionary<string, object?>? context);
        public AppInfo GetAppInfo(Dictionary<string, object?>? context);
        bool IsThereAnyActivePOApps(Dictionary<string, object?>? context);
        List<ActivePOApps> GetActivePOApps(Dictionary<string, object?>? context);
    }
}
using EY.CapitalEdge.HelpCopilot.Utils.Models;

namespace EY.CapitalEdge.HelpCopilot.Utils
{
    public interface ICommon
    {
        bool ValidateToken(string token, BackendInput input);

        string? ExtractValueFromContext(IDictionary<string, object> context, string key);
    }
}
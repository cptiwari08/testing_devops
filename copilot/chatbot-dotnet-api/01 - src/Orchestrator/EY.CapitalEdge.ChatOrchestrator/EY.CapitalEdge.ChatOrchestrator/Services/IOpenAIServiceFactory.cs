using OpenAI.Interfaces;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface IOpenAIServiceFactory
    {
        IOpenAIService GetService(string key);
    }
}

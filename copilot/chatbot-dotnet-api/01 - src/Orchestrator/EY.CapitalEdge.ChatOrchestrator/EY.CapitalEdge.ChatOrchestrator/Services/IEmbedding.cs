using OpenAI.ObjectModels.ResponseModels;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface IEmbedding
    {
        Task<EmbeddingCreateResponse> CreateEmbeddingsAsync(List<string> input);

        Task<EmbeddingCreateResponse> CreateEmbeddingAsync(string input);
    }
}

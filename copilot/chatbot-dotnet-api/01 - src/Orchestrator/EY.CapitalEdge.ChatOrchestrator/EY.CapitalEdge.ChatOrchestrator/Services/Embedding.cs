using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    [ExcludeFromCodeCoverage]
    public class Embedding : IEmbedding
    {
        private readonly IOpenAIService _embeddingOpenAIService;

        public Embedding(IOpenAIServiceFactory openAIServiceFactory)
        {
            _embeddingOpenAIService = openAIServiceFactory.GetService("EmbeddingOpenAIService");
        }

        public async Task<EmbeddingCreateResponse> CreateEmbeddingsAsync(List<string> input)
        {
            var result = await _embeddingOpenAIService.Embeddings.CreateEmbedding(new EmbeddingCreateRequest()
            {
                EncodingFormat = "float",
                InputAsList = input,
                Model = OpenAI.ObjectModels.Models.TextEmbeddingAdaV2,
                // Dimensions = 1536
            });

            return result;
        }

        public async Task<EmbeddingCreateResponse> CreateEmbeddingAsync(string input)
        {
            var result = await _embeddingOpenAIService.Embeddings.CreateEmbedding(new EmbeddingCreateRequest()
            {
                EncodingFormat = "float",
                Input = input,
                Model = OpenAI.ObjectModels.Models.TextEmbeddingAdaV2,
                // Dimensions = 1536
            });

            return result;
        }
    }
}

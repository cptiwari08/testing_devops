using Azure;
using Azure.AI.OpenAI;
using EY.CE.Copilot.API.Common;
using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.API.Static;
using EY.SaT.CapitalEdge.Extensions.Logging.Enums;
using EY.SaT.CapitalEdge.Extensions.Logging.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;


namespace EY.CE.Copilot.API.Clients
{
    public class SearchClient : BaseClass, ISearchClient
    {
        AzureOpenAIClient _client;
        private readonly IConfiguration _configuration;
        private readonly ISessionClient _sessionClient;
        public SearchClient(IConfiguration configuration, ISessionClient session, IAppLoggerService logger) : base(logger, nameof(SearchClient))
        {
            _configuration = configuration;
            string endpoint = _configuration[SharedKeyVault.OPEN_AI_API_ENDPOINT];
            string apiKey = _configuration[SharedKeyVault.OPEN_AI_API_KEY];
            _sessionClient = session;
            _client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            
        }
        public async Task<List<TextEmbedding>> GenerateEmbeddingsAsync(List<string> texts)
        {
            var embeddings = new List<TextEmbedding>();

            foreach (var text in texts)
            {
                var embedding = await GenerateEmbeddingAsync(text);
                TextEmbedding output = new TextEmbedding();
                output.name = text;
                output.embedding = embedding;
                embeddings.Add(output);
            }

            return embeddings;
        }

        private async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            try
            {
                string key = "EmbeddingsTeam_" + text;
                var cachedValue = await _sessionClient.GetRedisCache(key);
                if (!string.IsNullOrEmpty(cachedValue))
                {
                    Log(AppLogLevel.Information, "Retrieving value from cache for key " + key);
                    return JsonConvert.DeserializeObject<float[]>(cachedValue);
                }
                else
                {
                    var embeddingClient = _client.GetEmbeddingClient(_configuration[SharedKeyVault.OPEN_AI_EMBEDDING_DEPLOYMENT]);
                    var response = await embeddingClient.GenerateEmbeddingAsync(text);
                    Log(AppLogLevel.Information, "Setting redis cache for key " + key);
                    await _sessionClient.SetRedisCache(key, JsonConvert.SerializeObject(response.Value.ToFloats().ToArray()));
                    return response.Value.ToFloats().ToArray();
                }
            }

            catch (Exception ex)
            {
                Log(AppLogLevel.Error, "Error generating embedding" +ex.Message, exception : ex);
                throw;
            }
        }
        public async Task<List<QuerySearchOutput>> PerformSimilaritySearch(QuerySearchInput input)
        {
            try
            {
                List<QuerySearchOutput> result = new List<QuerySearchOutput>();
                Log(AppLogLevel.Information, "Performing similarity check for search string " + string.Join(",", input.searchString));
                var dataSourceEmbeddings = await GenerateEmbeddingsAsync(input.dataSource);
                var searchEmbeddings = await GenerateEmbeddingsAsync(input.searchString);
                foreach (var search in searchEmbeddings)
                {
                    var similarityScores = dataSourceEmbeddings.Select(source => new outputEmbedding { score = CosineSimilarity(source.embedding, search.embedding), name = source.name }).ToList();
                    var threshold = 0.8; // Adjust this threshold as needed
                    var output = similarityScores.Where(item => item.score >= threshold).OrderByDescending(e => e.score).Take(input.TopNResult).ToList();
                    result.Add(new QuerySearchOutput { SearchString = search.name, SearchOutput = output });
                }
                return result;
            }
            catch (Exception ex)
            {
                Log(AppLogLevel.Error, $"Error in method PerformSimilaritySearch - {ex.Message}", nameof(PerformSimilaritySearch), exception: ex);
                throw;
            }
        }
        static double CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            double dotProduct = vectorA.Zip(vectorB, (a, b) => a * b).Sum();
            double magnitudeA = Math.Sqrt(vectorA.Sum(a => a * a));
            double magnitudeB = Math.Sqrt(vectorB.Sum(b => b * b));

            return dotProduct / (magnitudeA * magnitudeB);
        }
    }
}

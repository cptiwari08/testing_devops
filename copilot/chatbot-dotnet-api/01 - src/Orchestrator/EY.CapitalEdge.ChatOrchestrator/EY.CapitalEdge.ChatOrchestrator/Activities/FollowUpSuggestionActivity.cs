using Azure.Security.KeyVault.Secrets;
using EY.CapitalEdge.ChatOrchestrator.Models;
using EY.CapitalEdge.ChatOrchestrator.Services;
using EY.CapitalEdge.ChatOrchestrator.Utils;
using EY.CapitalEdge.ChatOrchestrator.Utils.Dtos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace EY.CapitalEdge.ChatOrchestrator.Activities
{
    public class FollowUpSuggestionActivity
    {
        private readonly Services.ILoggerProvider _loggerProvider;
        private readonly ISuggestion _suggestion;
        private readonly IMmrQuestionSelectorWithEmbedding _mmrSuggestionSelectorWithEmbedding;
        private readonly ICommon _common;
        private readonly SecretClient _secretClient;
        private const string EYIPBackend = "ey-ip";
        private const string HelpCopilotBackend = "help-copilot";
        private const string prjsuggestions = "metadata/prjsuggestions";
        private const string ProjectFriendlyId = "projectFriendlyId";

        public FollowUpSuggestionActivity(
            Services.ILoggerProvider loggerProvider, 
            ISuggestion suggestion, 
            IMmrQuestionSelectorWithEmbedding mmrSuggestionSelectorWithEmbedding,
            ICommon common,
            SecretClient secretClient)
        {
            _loggerProvider = loggerProvider ?? throw new ArgumentNullException(nameof(loggerProvider));
            _suggestion = suggestion ?? throw new ArgumentNullException(nameof(suggestion));
            _mmrSuggestionSelectorWithEmbedding = mmrSuggestionSelectorWithEmbedding ?? throw new ArgumentNullException(nameof(mmrSuggestionSelectorWithEmbedding));
            _common = common ?? throw new ArgumentNullException(nameof(common));
            _secretClient = secretClient ?? throw new ArgumentNullException(nameof(secretClient));
        }

        [Function(nameof(FollowUpSuggestionActivity))]
        public async Task<List<FollowUpSuggestionsResponse>> Run([ActivityTrigger] FollowUpSuggestionInput input, FunctionContext executionContext)
        {
            ILogger log = _loggerProvider.GetLogger(executionContext, "FollowUpSuggestionActivity");
            ILogger logger = new CustomLogger(log, $"[instanceId:{input.InstanceId}] [chatId:{input.ChatId}] [{ProjectFriendlyId}:{input.ProjectFriendlyId}]");
            logger.LogInformation("FollowUpSuggestionsActivity.");

            int numQuestions = int.TryParse(Environment.GetEnvironmentVariable("MmrQuestionSelectorNumQuestions"), out int number) ? number : 3;
            List<FollowUpSuggestionsResponse> result = [];

            try
            {
                string sourcesFilter = string.Concat("(", 
                    string.Join(" or ", input.Input.Sources.Select(source =>
                        $"search.ismatch('{source.Replace($"-{EYIPBackend}", "").Replace($"-{HelpCopilotBackend}", "")}', '{prjsuggestions}/source', 'full', 'all')")), ")");

                string appAffinityFilter = string.Empty;
                if (_common.IsThereAnyAppInfo(input.Input.Context))
                {
                    AppInfo appInfo = _common.GetAppInfo(input.Input.Context);
                    appAffinityFilter = $"search.ismatch('{appInfo.Key}', '{prjsuggestions}/appAffinity', 'full', 'all')";
                }
                else if (_common.IsThereAnyActivePOApps(input.Input.Context))
                {
                    var activePOApps = _common.GetActivePOApps(input.Input.Context);
                    appAffinityFilter = string.Concat("(", 
                        string.Join(" or ", activePOApps.Select(app =>
                            $"search.ismatch('{app.Key}', '{prjsuggestions}/appAffinity', 'full', 'all')")), ")");
                }

                var filter = 
                    $" indexType eq 'prjsuggestions'" +
                    $" and {prjsuggestions}/visibleToAssistant eq true" +
                    $" and {prjsuggestions}/isIncluded eq true" + 
                    $" and {sourcesFilter}";

                if (!string.IsNullOrEmpty(appAffinityFilter))
                    filter += $" and {appAffinityFilter}";

                logger.LogInformation("Filter: {0}", filter);

                List<Models.Suggestion> suggestionEmbeddings = await _suggestion.GetEmbeddingsAsync(
                    await GetSearchClientInfo(logger, input.Input.Token), 
                    filter
                );

                logger.LogInformation("Suggestion embeddings count: {0}", suggestionEmbeddings.Count);

                suggestionEmbeddings = suggestionEmbeddings.DistinctBy(x => x.Chunk).ToList();

                Dictionary<string, float[]> questions = [];
                foreach (var item in suggestionEmbeddings)
                {
                    questions.Add(item.Chunk, item.Embedding);
                }

                _mmrSuggestionSelectorWithEmbedding.SetQuestions(questions);

                // Remove similar questions already asked in the chat history and the current question
                // to avoid suggesting the same question again
                List<string> chatHistory = input.ChatHistory.Select(s => s.Content).Distinct().ToList();
                chatHistory.Add(input.Input.Question);

                await _mmrSuggestionSelectorWithEmbedding.RemoveSimilarQuestionsAsync(chatHistory.Distinct().ToList());

                List<string> followUpSuggestions =
                    await _mmrSuggestionSelectorWithEmbedding.SelectSimilarQuestionsAsync(input.Input.Question, numQuestions);


                foreach (var suggestion in followUpSuggestions)
                {
                    var sug = suggestionEmbeddings.First(x => x.Chunk == suggestion);
                    result.Add(new FollowUpSuggestionsResponse { Id = sug.Id, SuggestionText = sug.Chunk });
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error calling function FollowUpSuggestionsActivity.");
                return result;
            }
        }

        /// <summary>
        /// Get search client info from key vault
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="inputToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [ExcludeFromCodeCoverage]
        private async Task<SearchClientDto> GetSearchClientInfo(ILogger logger, string inputToken)
        {
            Token token = _common.GetTokenData(logger, inputToken);

            if (token == null)
                throw new ArgumentException("Token is not provided or invalid.");

            string? searchInstanceName = token.AiSearchInstanceName;
            if (string.IsNullOrWhiteSpace(searchInstanceName))
                throw new ArgumentException("AiSearchInstanceName is not provided in the token.");

            string? indexName = token.MetadataIndexName;
            if (string.IsNullOrWhiteSpace(indexName))
                throw new ArgumentException("MetadataIndexName is not provided in the token.");

            var secret = await _secretClient.GetSecretAsync(searchInstanceName);

            var azureSearchConfigDto = _common.Deserialize<AzureSearchConfigDto>(secret.Value.Value);

            var searchClientDto = new SearchClientDto()
            {
                ServiceEndpoint = azureSearchConfigDto.Endpoint,
                Credential = azureSearchConfigDto.Key,
                IndexName = indexName
            };
            return searchClientDto;
        }
    }
}
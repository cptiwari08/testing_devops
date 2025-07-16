using OpenAI.ObjectModels.ResponseModels;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    [ExcludeFromCodeCoverage]
    public class MmrQuestionSelectorWithEmbedding : IMmrQuestionSelectorWithEmbedding
    {
        private Dictionary<string, float[]>? _questionEmbeddings = null;

        private readonly IEmbedding _embedding;

        public MmrQuestionSelectorWithEmbedding(IEmbedding embedding)
        {
            _embedding = embedding ?? throw new ArgumentNullException(nameof(embedding));
        }

        /// <summary>
        /// Set the questions to be used for selection
        /// </summary>
        /// <param name="questions">List of questions</param>
        public async Task SetQuestionsAsync(List<string> questions)
        {
            var embeddings = await _embedding.CreateEmbeddingsAsync(questions);
            Dictionary<string, float[]> questionEmbeddings = MapEmbeddingResponseListToDictionary(questions, embeddings.Data);
            SetQuestions(questionEmbeddings);
        }

        public void SetQuestions(Dictionary<string, float[]> questions)
        {
            _questionEmbeddings = questions;
        }

        /// <summary>
        /// Remove similar questions that match with the questions to check
        /// </summary>
        public async Task RemoveSimilarQuestionsAsync(List<string> questionsToCheck)
        {
            if (_questionEmbeddings is null)
                throw new InvalidOperationException("Questions must be set before removing similar questions");

            var embeddings = await _embedding.CreateEmbeddingsAsync(questionsToCheck);
            Dictionary<string, float[]> questionEmbeddings = MapEmbeddingResponseListToDictionary(questionsToCheck, embeddings.Data);
            foreach (var q1 in questionEmbeddings)
            {
                foreach (var q2 in _questionEmbeddings)
                {
                    double relevance = CosineSimilarity(q1.Value, q2.Value);
                    if (relevance > 0.9)
                    {
                        _questionEmbeddings.Remove(q2.Key);
                    }
                }
            }
        }

        public async Task<List<string>> SelectSimilarQuestionsAsync(string targetQuestion, int numQuestions)
        {
            double lambda = GetLambda();
            if (IsLambdaValid(lambda))
                throw new ArgumentException("MmrQuestionSelectorLambda must be between 0.0 and 1.0");

            if (_questionEmbeddings is null)
                throw new InvalidOperationException("MmrQuestionSelectorWithEmbedding: Questions must be set before removing similar questions");

            List<string> questions = _questionEmbeddings.Keys.ToList();
            var selectedQuestions = new List<string>();
            var remainingQuestions = new List<string>(questions);
            var targetEmbedding = ((await _embedding.CreateEmbeddingAsync(targetQuestion))?.Data.FirstOrDefault()?.Embedding.Select(d => (float)d).ToArray()) ?? throw new InvalidOperationException("Target question embedding could not be created");
            var questionEmbeddings = remainingQuestions.ToDictionary(question => question, question => GetEmbedding(question));

            while (selectedQuestions.Count < numQuestions && remainingQuestions.Count != 0)
            {
                string? nextQuestion = null;
                double maxScore = double.MinValue;

                foreach (var question in remainingQuestions)
                {
                    var embedding = questionEmbeddings[question];
                    double relevance = CosineSimilarity(embedding, targetEmbedding);
                    double diversity = selectedQuestions.Count != 0 ? selectedQuestions.Min(selected => 1 - CosineSimilarity(embedding, questionEmbeddings[selected])) : 0;
                    double score = lambda * relevance + (1.0 - lambda) * diversity; // Adjust the balance as needed using lambda

                    if (score > maxScore)
                    {
                        maxScore = score;
                        nextQuestion = question;
                    }
                }

                if (nextQuestion != null)
                {
                    selectedQuestions.Add(nextQuestion);
                    remainingQuestions.Remove(nextQuestion);
                }
            }

            return selectedQuestions;
        }

        private static Dictionary<string, float[]> MapEmbeddingResponseListToDictionary(List<string> questions, List<EmbeddingResponse> embeddings)
        {
            Dictionary<string, float[]> result = new Dictionary<string, float[]>();
            foreach (EmbeddingResponse embedding in embeddings)
            {
                float[] embeddingArray = embedding.Embedding.Select(d => (float)d).ToArray();
                result.Add(questions[embedding.Index ?? 0], embeddingArray);
            }
            return result;
        }

        private static double CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            double dotProduct = 0.0;
            double normA = 0.0;
            double normB = 0.0;
            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                normA += Math.Pow(vectorA[i], 2);
                normB += Math.Pow(vectorB[i], 2);
            }
            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }

        private float[] GetEmbedding(string question)
        {
            if (_questionEmbeddings is null)
                throw new InvalidOperationException("Questions must be set before removing similar questions");

            return _questionEmbeddings[question];
        }

        /// <summary>
        /// Get lambda value from environment variable, or default to 0.5
        /// </summary>
        /// <returns>Lamda value</returns>
        private static double GetLambda()
        {
            return double.TryParse(Environment.GetEnvironmentVariable("MmrQuestionSelectorLambda"),
                NumberStyles.Any, CultureInfo.InvariantCulture, out double number) ? number : 0.5;
        }

        /// <summary>
        /// Check if lambda is valid
        /// </summary>
        /// <param name="lambda">lambda</param>
        /// <returns>True/False</returns>
        private static bool IsLambdaValid(double lambda)
        {
            return lambda < 0.0 || lambda > 1.0;
        }
    }
}

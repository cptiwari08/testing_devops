namespace EY.CapitalEdge.ChatOrchestrator.Services
{
    public interface IMmrQuestionSelectorWithEmbedding
    {
        Task SetQuestionsAsync(List<string> questions);

        void SetQuestions(Dictionary<string, float[]> questions);

        Task RemoveSimilarQuestionsAsync(List<string> questionsToCheck);

        Task<List<string>> SelectSimilarQuestionsAsync(string targetQuestion, int numQuestions);
    }
}

namespace EY.CE.Copilot.API.Mapper
{
    public class CopilotFeedback
    {
        public static Data.Models.CopilotFeedback CreateInsertModel(Models.Chat.CopilotFeedback apiFeedback,
            string userId)
        {
            return new EY.CE.Copilot.Data.Models.CopilotFeedback
            {
                FeedbackText = apiFeedback.FeedbackText == null ? string.Empty : apiFeedback.FeedbackText,
                Rating = apiFeedback.Rating,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };
        }
    }
}

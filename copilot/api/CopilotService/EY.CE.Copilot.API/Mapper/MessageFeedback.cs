namespace EY.CE.Copilot.API.Mapper
{
    public static class MessageFeedback
    {
        public static Data.Models.MessageFeedback CreateInsertModel(Models.Chat.MessageFeedback apiFeedback,
            string userId)
        {
            return new EY.CE.Copilot.Data.Models.MessageFeedback
            {
                ChatId = apiFeedback.ChatId,
                MessageId = apiFeedback.MessageId,
                FeedbackText = apiFeedback.FeedbackText == null?string.Empty:apiFeedback.FeedbackText,
                IsLiked = apiFeedback.IsLiked,
                Consent = apiFeedback.Consent,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                InstanceId = apiFeedback.InstanceId,
                UserId = userId
            };
        }

        public static void CreateUpdateModel(Models.Chat.MessageFeedback apiFeedback,
            Data.Models.MessageFeedback modelFeedback)
        {
            if (apiFeedback.FeedbackText != null)
                modelFeedback.FeedbackText = apiFeedback.FeedbackText;
            modelFeedback.IsLiked = apiFeedback.IsLiked;
            modelFeedback.Consent = apiFeedback.Consent ?? modelFeedback.Consent;
            modelFeedback.UpdatedAt = DateTime.Now;
        }
    }
}

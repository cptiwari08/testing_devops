using EY.CE.Copilot.API.Static;

namespace EY.CE.Copilot.API.Mapper
{
    public class ChatHistory
    {
        public static Data.Models.ChatHistory CreateInsertModelForUser(string chatId, Models.Chat.Message message,
            string userId)
        {
            var chatHistory = new Data.Models.ChatHistory
            {
                ChatId = chatId,
                MessageId = message.MessageId,
                InstanceId = message.InstanceId,
                Message = message.Content,
                UserRole = message.Role,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                UpdatedBy = userId,
                UpdatedAt = DateTime.Now,
                UserId = userId,
                Status = message.Status
            };
            if (message.Context != null && message.Context.Suggestion != null && !string.IsNullOrWhiteSpace(message.Context.Suggestion.Id))
                chatHistory.SuggestionId = Convert.ToInt32(message.Context.Suggestion.Id);
            return chatHistory;
        }
        public static void CreateUpdateModel(Models.Chat.Message apiMessage, Data.Models.ChatHistory? modelMessage, 
            string? content, TimeSpan? timeEllapsed, string status, int statusCode)
        {
            if(modelMessage != null && apiMessage != null)
            {
                modelMessage.Message = apiMessage.Content;
                modelMessage.AdditionalInfo = content;
                modelMessage.UpdatedAt = DateTime.Now;
                modelMessage.Status = status;
                modelMessage.StatusCode  = statusCode;
                TimeSpan timeSpan = timeEllapsed ?? TimeSpan.Zero;
                if(timeSpan != TimeSpan.Zero)
                    modelMessage.TimeToResolve = (long) timeSpan.TotalMilliseconds;
            }
            
        }
    }
}

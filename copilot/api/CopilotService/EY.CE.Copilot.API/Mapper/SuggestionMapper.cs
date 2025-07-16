using EY.CE.Copilot.API.Models;

namespace EY.CE.Copilot.API.Mapper
{
    public class SuggestionMapper
    {
        public static Data.Models.Suggestion CreateInsertModelForUser(SuggestionInsert apiSuggestion,
            string userId)
        {
            Data.Models.Suggestion suggestion = new Data.Models.Suggestion
            {
                Source = apiSuggestion.Source,
                SuggestionText = apiSuggestion.SuggestionText,
                AppAffinity = apiSuggestion.AppAffinity,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                UpdatedBy = userId,
                UpdatedAt = DateTime.Now,
                AnswerSQL = apiSuggestion.AnswerSQL,
                VisibleToAssistant = apiSuggestion.VisibleToAssistant,
                IsIncluded = apiSuggestion.IsIncluded
            };
            return suggestion;
        }

        public static void CreateUpdateModelForUser(List<SuggestionUpdate> apiSuggestions, List<Data.Models.Suggestion> dbSuggestions,
           string userId)
        {
            foreach (var dbSuggestion in dbSuggestions)
            {
                var apiSuggestion = apiSuggestions.Where(s => s.ID == dbSuggestion.ID).FirstOrDefault();
                if (apiSuggestion != null)
                {    
                    dbSuggestion.SuggestionText = apiSuggestion.SuggestionText ?? dbSuggestion.SuggestionText;
                    dbSuggestion.Source = apiSuggestion.Source ?? dbSuggestion.Source;
                    dbSuggestion.AppAffinity = apiSuggestion.AppAffinity ?? dbSuggestion.AppAffinity;
                    dbSuggestion.UpdatedAt = DateTime.Now;
                    dbSuggestion.UpdatedBy = userId;
                    dbSuggestion.AnswerSQL = apiSuggestion.AnswerSQL ?? dbSuggestion.AnswerSQL;
                    dbSuggestion.VisibleToAssistant = apiSuggestion.VisibleToAssistant ?? dbSuggestion.VisibleToAssistant;
                    dbSuggestion.IsIncluded = apiSuggestion.IsIncluded ?? dbSuggestion.IsIncluded;
                }
            }
        }
    }
}

using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Mapper
{
    public class PromptMapper
    {
        public static AssistantPrompt CreateInsertModelForUser(PromptInsert PromptInsert, string userId) {
            Data.Models.AssistantPrompt prompts = new Data.Models.AssistantPrompt
            {
                Title = PromptInsert.Title,
                Type = PromptInsert.Type,
                Key = PromptInsert.Key,
                IsActive = PromptInsert.IsActive??false,
                Prompt = PromptInsert.Prompt,
                OriginalPrompt = PromptInsert.OriginalPrompt,
                Description = PromptInsert.Description,
                Agent = PromptInsert.Agent,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                UpdatedBy = userId,
                UpdatedAt = DateTime.Now
            };
            return prompts;
        }

        public static void CreateUpdateModelForUser(List<PromptUpdate> input, List<Data.Models.AssistantPrompt> dbPrompts,
           string userId)
        {
            foreach (var dbPrompt in dbPrompts)
            {
                var inputPrompt = input.Where(s => s.ID == dbPrompt.ID).FirstOrDefault();
                if (inputPrompt != null)
                {
                    dbPrompt.Title = inputPrompt.Title ?? dbPrompt.Title;
                    dbPrompt.Type = inputPrompt.Type ?? dbPrompt.Type;
                    dbPrompt.Key = inputPrompt.Key ?? dbPrompt.Key;
                    dbPrompt.IsActive = inputPrompt.IsActive ?? dbPrompt.IsActive;
                    dbPrompt.Prompt = inputPrompt.Prompt ?? dbPrompt.Prompt;
                    dbPrompt.OriginalPrompt = inputPrompt.OriginalPrompt ?? dbPrompt.OriginalPrompt;
                    dbPrompt.Description = inputPrompt.Description ?? dbPrompt.Description;
                    dbPrompt.Agent = inputPrompt.Agent ?? dbPrompt.Agent;
                    dbPrompt.UpdatedAt = DateTime.Now;
                    dbPrompt.UpdatedBy = userId;
                }
            }
        }
    }
}

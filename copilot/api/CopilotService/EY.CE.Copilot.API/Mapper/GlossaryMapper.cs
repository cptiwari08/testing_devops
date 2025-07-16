using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Mapper
{
    public class GlossaryMapper
    {
        public static Glossary CreateInsertModelForUser(GlossaryInsert glossaryInsert, string userId) {
            Data.Models.Glossary glossary = new Data.Models.Glossary
            {
                Context = glossaryInsert.Context,
                TableName = glossaryInsert.TableName,
                IsSchema = glossaryInsert.IsSchema,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                UpdatedBy = userId,
                UpdatedAt = DateTime.Now,
                Tag = glossaryInsert.Tag,
            };
            return glossary;
        }

        public static void CreateUpdateModelForUser(List<GlossaryUpdate> input, List<Data.Models.Glossary> gbGlossaries,
           string userId)
        {
            foreach (var dbGlossary in gbGlossaries)
            {
                var inputGlossary = input.Where(s => s.ID == dbGlossary.ID).FirstOrDefault();
                if (inputGlossary != null)
                {
                    dbGlossary.TableName = inputGlossary.TableName ?? dbGlossary.TableName;
                    dbGlossary.Context = inputGlossary.Context ?? dbGlossary.Context;
                    dbGlossary.IsSchema = inputGlossary.IsSchema ?? dbGlossary.IsSchema;
                    dbGlossary.Tag = inputGlossary.Tag ?? dbGlossary.Tag;
                    dbGlossary.UpdatedAt = DateTime.Now;
                    dbGlossary.UpdatedBy = userId;
                }
            }
        }
    }
}

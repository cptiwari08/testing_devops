using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Mapper
{
    public class GeneratorHistoryMapper
    {
        public static Data.Models.GeneratorHistory CreateInsertModelForUser(GeneratorActivityRecord content)
        {
            var generatorContent = new Data.Models.GeneratorHistory
            {
                AdditionalInfo = content.AdditionalInfo,
                CreatedAt = DateTime.Now,
                CreatedBy = content.UserId,
                InstanceId = content.InstanceId,
                Status =content.Status,
                StatusCode=content.StatusCode,
                Type = content.Type,
                UpdatedAt = DateTime.Now,
                UpdatedBy = content.UserId,
                UserEmail = content.UserId
            };
           return generatorContent;
        }

        public static Data.Models.GeneratorHistory CreateUpdateModelForUser(GeneratorActivityRecord content, GeneratorHistory contentHistory)
        {
            contentHistory.AdditionalInfo = content.AdditionalInfo;
            contentHistory.Status = content.Status;
            contentHistory.StatusCode = content.StatusCode;
            contentHistory.UpdatedAt = DateTime.Now;
            contentHistory.UpdatedBy = content.UserId;
         
            return contentHistory;
        }
    }
}

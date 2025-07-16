using EY.CE.Copilot.API.Contracts;
using EY.CE.Copilot.API.Models;
using EY.CE.Copilot.Data.Models;

namespace EY.CE.Copilot.API.Extensions
{
    public static class SuggestionExtensions
    {
        public const string typeOther = "other";
        public const string peValueCreation = "PE Value Creation";
        public const string peValueCreationUpdated = "Private Equity Value Creation";
        public static void ReplaceAndFilterTags(this List<Suggestion> input, IPortalClient client, string userId)
        {
            var replaceData = client.GetProjectDetails().Result;
            FilterTags(input, replaceData, userId);
            ReplaceTags(input, replaceData, userId);
        }
        private static void FilterTags(List<Suggestion> input, ProjectData replaceData, string userId)
        {
            if (replaceData.Data.Type.ToLower() == typeOther)
                input.RemoveAll(item => item.SuggestionText.Contains("{ProjectType}"));
        }

        private static void ReplaceTags(this List<Suggestion> input, ProjectData replaceData, string userId)
        {
            foreach (var item in input)
            {
                item.SuggestionText = ReplaceTagWithValue(item.SuggestionText, replaceData, userId);
                item.AnswerSQL = ReplaceTagWithValue(item.AnswerSQL, replaceData, userId);
            }
        }

        static string ReplaceTagWithValue(string input, ProjectData replaceData, string userId)
        {
            if(string.IsNullOrWhiteSpace(input))
                return input;
            input = input.Replace("{Username}", userId);
            input = input.Replace("{ProjectType}", GetProjectType(replaceData.Data.Type));
            input = input.Replace("{ProjectTypeId}", replaceData.Data.TypeId);
            input = input.Replace("{Sector}", replaceData.Data.Sector);
            input = input.Replace("{ProjectSectorId}", replaceData.Data.SectorId);
            input = input.Replace("{ProjectArea}", replaceData.Data.Area);
            input = input.Replace("{ProjectAreaId}", replaceData.Data.AreaId);
            input = input.Replace("{ProjectRegion}", replaceData.Data.Region);
            input = input.Replace("{ProjectRegionId}", replaceData.Data.RegionId);
            return input;
        }

        static string GetProjectType(string projectType)
        {
            return projectType == peValueCreation ?
                peValueCreationUpdated : projectType;
        }
    }
}

namespace EY.CE.Copilot.Data.Static
{
    public class Constants
    {
        public static class Source
        {
            public static readonly SourceItem EYGuidance = new SourceItem { DisplayName = "EY Guidance", Key = "ey-guidance" };
            public static readonly SourceItem Internet = new SourceItem { DisplayName = "Internet", Key = "internet" };
            public static readonly SourceItem ProjectDocuments = new SourceItem { DisplayName = "Project Documents", Key = "project-docs" };
            public static readonly SourceItem ProjectData = new SourceItem { DisplayName = "Project Data", Key = "project-data" };
        }
        public class SourceItem
        {
            public string Key { get; set; }
            public string DisplayName { get; set; }
        }

        internal static class App
        {
            public const string ProjectManagement = "CE4-PMO";
            public const string ValueCapture = "CE4-VC";
            public const string OperatingModel = "CE4-OM";
            public const string TSA = "CE4-TSA";
            public const string ProjectLevel = "PROJECT_LEVEL";
        }

        public static Dictionary<string, string> SourceKeyDisplayNames = new Dictionary<string, string>
        {
            {"EY Guidance","ey-guidance" },
            {"Internet","internet" },
            {"Project Documents","project-docs" },
            {"Project Data","project-data" }
        };
    }
}

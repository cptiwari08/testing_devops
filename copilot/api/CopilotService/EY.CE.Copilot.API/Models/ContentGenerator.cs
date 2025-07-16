namespace EY.CE.Copilot.API.Models
{
    public class ContentGenerator
    {
        public AppType appType { get; set; }
        public List<string>? ProjectTeamsRemoveList { get; set; }
        public Workplan? Workplan { get; set; }
        public ProjectStatus? ProjectStatus { get; set; }
        public ProjectCharter? ProjectCharter { get; set; }
    }

    public enum AppType
    {
        WorkplanGenerator,
	    StatusReportGenerator,
	    LegalEntity,
	    ProjectCharterGenerator,
  	    OpModelGenerator,
        TSAGenerator
    }
}

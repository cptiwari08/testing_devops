namespace EY.CE.Copilot.API.Models
{
    public class TSA : GeneratorResponseMeta
    {
        public TSAOutput? output { get; set; }
    }
    public class TSAObject : GeneratorResponseMeta
    {
        public TSAInput input { get; set; }
        public TSAOutputObject? output { get; set; }
        
    }

    public class TSAInput
    {
        public List<object> eyIP {  get; set; }
        public List<object> ceApps { get; set; }
    }
    public class TSAOutputObject
    {
        public List<object>? response { get; set; }
    }

    public class TSAOutput
    {
        public List<TSAResponse>? response { get; set; }
    }

    public class TSAResponse
    {
        public ProjectTeam projectTeam { get; set; }
        public List<TSAContent> content { get; set; }
        public List<CitingSource> citingSources { get; set; }
        public string status { get; set; }
    }
    
    public class TSAContent
    {
        public string title {  get; set; }
        public string serviceInScopeDescription { get; set; }
        public TSA1Disposition tSADay1Disposition { get; set; }
        public List<string> assistantGeneratorOrigins { get; set; }
        public List<string> associatedProcesses { get; set; }
        public List<string> associatedSystems { get; set; }
        public List<string> associatedFacilities { get; set; }
        public List<string> associatedAssets { get; set; }
        public List<string> associatedTPAs { get; set; }
        public List<string> associatedAITs { get; set; }

    }
    public class TSA1Disposition
    {
        public string key { get; set; }
    }
}

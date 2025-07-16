using Newtonsoft.Json.Linq;

namespace EY.CE.Copilot.API.Models
{
    
    public class ProjectCharterResponse
    {
        public string sourceName { get; set; }
        public List<string> content { get; set; }
        public string status { get; set; }
        public List<CitingSource> citingSources { get; set; }
    }

    public class ProjectCharter : GeneratorResponseMeta
    {
        public ProjectCharterOutput output { get; set; }
    }
    public class ProjectCharterOutput
    {
        public List<ProjectCharterResponse> response { get; set; }
    }
}

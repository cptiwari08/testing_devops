using Newtonsoft.Json.Linq;

namespace EY.CE.Copilot.API.Models
{
    public class GeneratorResponseMeta
    {
        public string name { get; set; }
        public string instanceId { get; set; }
        public string runtimeStatus { get; set; }
        public string errorMessage { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime lastUpdatedTime { get; set; }
    }

    public class ProjectStatus : GeneratorResponseMeta
    {
        public Response output { get; set; }
    }

    public class Response
    {
        public List<object> response { get; set; }
    }
    public class ProjectStatusJson : GeneratorResponseMeta
    {
        public ProjectStatusOutput output { get; set; }
    }
    
    public class ProjectStatusOutput
    {
        public JArray response { get; set; }
    }

    public class ProjectStatusResponse
    {
        public string sourceName { get; set; }
        
        public string status { get; set; }
        public List<ProjectCitingSources> citingSources { get; set; }

    }

    public class ProjectCitingSources 
    {
        public string sourceName { get; set; }
        public string sourceType { get; set; }
        public List<ProjectSourceValue> sourceValue { get; set; }   
        public string context { get; set; }
    }

    public class ProjectSourceValue {
        public string tableName { get; set; }
        public int? rowcount { get; set; }
        public int? count { get; set; }
        public string sqlQuery { get; set; }
        public string status { get; set; }
    }
    public class ContentString : ProjectStatusResponse
    {
        public string content { get; set; }
    }
    public class ContentArray : ProjectStatusResponse
    {
        public List<string> content { get; set; }
    }
    public class ContentObject : ProjectStatusResponse
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public int? ID { get; set; }
        public string? Title { get; set; }
    }

   

}

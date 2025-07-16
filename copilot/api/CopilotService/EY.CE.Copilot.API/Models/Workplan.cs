
namespace EY.CE.Copilot.API.Models
{
    public class WorkplanOutputFinal
    {
        public WorkplanOutput output { get; set; }
        public string runtimeStatus { get;set; }
        public string instanceId { get; set; }
        public string name { get; set; }
        public DateTime createdTime {  get; set; }
        public DateTime lastUpdatedTime { get; set; }
    }
    public class WorkplanOutput
    {

        public List<Projectresponse> response { get; set; }
    }
    public class Projectresponse
    {
        public ProjectTeam projectTeam { get; set; }
        public List<Activity> content { get; set; }
        public string status { get; set; }
        public List<CitingSource> citingSources { get; set; }
    }

    public class CitingSource
    {
        public string sourceName { get; set; }
        public string sourceType { get; set; }
        public string? content { get; set; }
        public List<SourceValue> sourceValue { get; set; }
    }

    public class SourceValue
    {
        public string documentId { get; set; }
        public string chunk_text { get; set; }
        public List<int> pages { get; set; }
        public string documentName { get; set; }
        public string chunk_id { get; set; }
        public string tableName { get; set; }
        public int? rowCount { get; set; }
        public string sqlQuery { get; set; }
        public string Category { get; set; }
        public int? count { get; set; }
    }
    public class ProjectTeam
    {
        public string title { get; set; }
        public int id { get; set; }
    }

    public class Activity
    {
        public string title { get; set; }
        public string workPlanTaskType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TaskDueDate { get; set; }
        public List<Activity> children { get; set; }
    }

    public class WorkplanInput
    {
        public List<string> ProjectTeamsRemoveList { get; set; }
    }

    public class Workplan
    {
        public WorkplanOutputFinal Output;
        public List<Projectresponse> FilterOutputByTeams(List<string> projectTeamsRemoveList)
        {
            if (Output != null && Output.output !=null)
            {
                List<Projectresponse> filterOutput = new List<Projectresponse>();
                foreach (var response in Output.output.response)
                {
                    var projectTeam = response.projectTeam.title;
                    if (!projectTeamsRemoveList.Contains(projectTeam))
                        filterOutput.Add(response);
                }
                return filterOutput;
            }
            return null;
        }
    }
}

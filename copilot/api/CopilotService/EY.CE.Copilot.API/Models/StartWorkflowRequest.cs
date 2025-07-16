namespace EY.CE.Copilot.API.Models
{
    public class StartWorkflowRequest
    {
        public string BaseUrl { get; set; }

        public string CorrelationId { get; set; }

        public string RequesterUsername { get; set; }

        public string Message { get; set; }
        public string WorkflowKey { get; set; }
        public string WorkflowParameters { get; set; }
    }
}

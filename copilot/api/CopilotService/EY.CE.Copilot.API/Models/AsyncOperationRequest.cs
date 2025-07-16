namespace EY.CE.Copilot.API.Models
{
    public class AsyncOperationRequest
    {
        public string? RequestUrl { get; set; }
        public dynamic Payload { get; set; }
        public string? FileName { get; set; }
        public string? OperationType { get; set; }
    }
}

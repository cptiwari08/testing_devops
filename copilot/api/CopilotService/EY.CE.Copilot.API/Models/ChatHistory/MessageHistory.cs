namespace EY.CE.Copilot.API.Models.ChatHistory
{
    public class MessageHistory
    {
        public int messageId { get; set; }
        public string role { get; set; }
        public string content { get; set; }
        public List<string> inputSources { get; set; }
        public List<Response> response { get; set; }
    }

    public class Response
    {
        public List<CitingSource> citingSources { get; set; }
    }

    public class CitingSource
    {
        public string sourceType { get; set; }
        public string sourceName { get; set; }
        public List<object> sourceValue { get; set; }
    }

    public class Source
    {
        public string documentName { get; set; }
    }
}

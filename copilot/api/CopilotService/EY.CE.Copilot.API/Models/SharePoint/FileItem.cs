using System.Text.Json.Serialization;
using static EY.CE.Copilot.API.Static.Constants;

namespace EY.CE.Copilot.API.Models
{
    public class FileItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Path { get; set; }
        public string LinkingUri { get; set; }
        public string EmbedUri { get; set; }
        public List<string>? VisibleToAssistant { get; set; }
        public string AssistantProcessingStatus { get; set; }
        public string AssistantProcessingStatusMessage { get; set; }
        public string AuthorId { get; set; }
        public string EditorId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
    }

    internal class SpFileListItem
    {
        public string GUID { get; set; }
        public SpFile File { get; set; }
        public string ServerRedirectedEmbedUri { get; set; }
        [JsonPropertyName(SharePoint.VisibleToAssistantTitle)]
        public List<string>? VisibleToAssistant { get; set; }
        [JsonPropertyName(SharePoint.AssistantProcessingStatusTitle)]
        public string AssistantProcessingStatus { get; set; }
        [JsonPropertyName(SharePoint.AssistantProcessingStatusMessageTitle)]
        public string AssistantProcessingStatusMessage { get; set; }
        public SpUser Author { get; set; }
        public SpUser Editor { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
    }

    internal class SpFile
    {
        public string Name { get; set; }
        public string Length { get; set; }
        public string ServerRelativeUrl { get; set; }
        public string LinkingUri { get; set; }
    }

    internal class SpUser
    {
        public string EMail { get; set; }
    }
}


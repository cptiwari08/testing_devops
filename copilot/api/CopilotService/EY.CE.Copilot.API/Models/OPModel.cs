namespace EY.CE.Copilot.API.Models
{
    public class OpModel : GeneratorResponseMeta
    {
        public OpModelOutput output { get; set; }
    }

    public class OpModelOutput
    {
        public List<OpModelResponse> response { get; set; }
    }
    public class OpModelResponse
    {
        public List<OpModelContent> content { get; set; }
        public string? status { get; set; }
        public List<CitingSource>? citingSources { get; set; }
    }
  
public class OpModelCitingSources
    {
        public string? sourceName { get; set; }
        public string? sourceType { get; set; }
        public string? content { get; set; }
        public List<OpModelSourceValue>? sourceValue { get; set; }
    }

    public class OpModelContent
    {
        public string? title { get; set; }
        public NodeType? nodeType { get; set; }
        public List<Child>? children { get; set; }
    }

    public class NodeType
    {
        public string key { get; set; }
    }

    public class OpModelSourceValue
    {
        public string? documentId { get; set; }
        public string? documentName { get; set; }
        public string? chunk_id { get; set; }
        public List<object>? pages { get; set; }
        public string? chunk_text { get; set; }
        public string? tableName { get; set; }
        public int? rowCount { get; set; }
        public string? sqlQuery { get; set; }
    }

    public class Child
    {
        public string? title { get; set; }
        public NodeType? nodeType { get; set; }
        public List<Child>? children { get; set; }
    }
}

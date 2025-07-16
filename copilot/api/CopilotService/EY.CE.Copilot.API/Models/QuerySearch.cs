namespace EY.CE.Copilot.API.Models
{
    public class QuerySearchInput
    {
        public List<string> dataSource{  get; set; }
        public List<string> searchString {  get; set; }
        public int TopNResult { get; set; } = 1;
    }
    public class QuerySearchOutput { 
        public string SearchString { get; set; }
        public List<outputEmbedding> SearchOutput { get; set; }
    }
    public class TextEmbedding
    {
        public string name { get; set; }
        public float[] embedding { get; set; }
    }

    public class outputEmbedding
    {
        public string name { get; set; }
        public double score { get; set; }
    }
}

namespace EY.CapitalEdge.ChatOrchestrator.Utils
{
    public class SerializableHttpResponseMessage
    {
        public HttpMethod? Method { get; set; }
        public string RequestUri { get; set; }
        public int? StatusCode { get; set; }
        public string? ReasonPhrase { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string? Response { get; set; }

        public SerializableHttpResponseMessage(HttpMethod? method, string requestUri, int? statusCode, string? reasonPhrase, bool isSuccessStatusCode, string? response)
        {
            Method = method;
            RequestUri = requestUri;
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            IsSuccessStatusCode = isSuccessStatusCode;
            Response = response;
        }
    }
}

using System.Text;

namespace EY.CapitalEdge.ChatOrchestrator.Utils
{
    public static class HttpRequestConverter
    {
        public static HttpRequestMessage ConvertToHttpRequestMessage(SerializableHttpRequestMessage serializableHttpRequestMessage)
        {
            var request = new HttpRequestMessage(serializableHttpRequestMessage.Method, new Uri(serializableHttpRequestMessage.RequestUri));

            if (serializableHttpRequestMessage.Headers is not null)
                foreach (var header in serializableHttpRequestMessage.Headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value.Split(","));

            if (serializableHttpRequestMessage.Content is not null)
                request.Content = new StringContent(
                    serializableHttpRequestMessage.Content.Data, 
                    serializableHttpRequestMessage.Content.Encoding == "UTF8"? Encoding.UTF8: Encoding.Unicode, 
                    serializableHttpRequestMessage.Content.MediaType);

            return request;
        }
    }

    public class SerializableHttpRequestMessage
    {
        public HttpMethod Method { get; set; }
        public string RequestUri { get; set; }
        public Dictionary<string, string>? Headers { get; set; }
        public StrContent? Content { get; set; }

        public SerializableHttpRequestMessage(HttpMethod method, string requestUri, Dictionary<string, string>? headers = null, StrContent? content = null)
        {
            Method = method;
            RequestUri = requestUri;
            Headers =  headers ?? new Dictionary<string, string>();
            Content = content;
        }
    }

    public class StrContent 
    {
        public string Data { get; set; }
        public string? Encoding { get; set; }
        public string MediaType { get; set; }
        public StrContent(string data, string mediaType, string? encoding = null)
        {
            Data = data; 
            Encoding = encoding ?? "UTF8"; 
            MediaType = mediaType;
        }
    }
}

using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System.Net;

namespace EY.CapitalEdge.ChatOrchestrator.Tests.Fakes
{
    public class FakeHttpResponseData : HttpResponseData
    {
        public FakeHttpResponseData(FunctionContext functionContext) : base(functionContext)
        {

        }

        public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Accepted;
        public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();
        public override Stream Body { get; set; } = new MemoryStream();
        public override HttpCookies Cookies { get; }
    }
}

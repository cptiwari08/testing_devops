using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;

namespace AssistantAPITest.DependencyMockups
{
    internal class HttpContextMockup
    {
        //HeaderDictionary headers = new HeaderDictionary(new Dictionary<string, StringValues>
        //{
        //    {"emailaddress","Bikram1.Nayak@ey.com" }
        //});
        Dictionary<object, object> headerItems= new Dictionary<object, object>{
            {"emailaddress","TestEY.Assistant@ey.com" }
        };
        internal Mock<IHttpContextAccessor> httpContextAccessorMock;
        internal HttpContextMockup()
        {
            //var httpRequestMock = new Mock<HttpRequest>();
            //httpRequestMock.Setup(x => x.Headers).Returns(headers);
            GetAuthToken();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Items).Returns(headerItems);
            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);
        }

        private void GetAuthToken()
        {
            //code to get auth token
            headerItems.Add(HeaderNames.Authorization, "");
        }
    }
}

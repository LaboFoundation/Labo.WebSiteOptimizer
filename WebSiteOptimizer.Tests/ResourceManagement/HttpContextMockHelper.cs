using System.Web;
using Moq;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement
{
    internal static class HttpContextMockHelper
    {
        internal static Mock<HttpContextBase> CreateHttpContext(HttpResponseBase httpResponseStub, HttpRequestBase httpRequestStub)
        {
            Mock<HttpContextBase> httpContextBaseMock = new Mock<HttpContextBase>();
            httpContextBaseMock.SetupGet(x => x.Response).Returns(() => httpResponseStub);
            httpContextBaseMock.SetupGet(x => x.Request).Returns(() => httpRequestStub);
            return httpContextBaseMock;
        }
    }
}

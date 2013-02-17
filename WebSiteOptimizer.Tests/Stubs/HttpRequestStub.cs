using System.Collections.Specialized;
using System.Web;

namespace Labo.WebSiteOptimizer.Tests.Stubs
{
    public sealed class HttpRequestStub : HttpRequestBase
    {
        private readonly NameValueCollection m_Headers;
        public override NameValueCollection Headers
        {
            get
            {
                return m_Headers;
            }
        }

        public HttpRequestStub(NameValueCollection headers = null)
        {
            m_Headers = headers ?? new NameValueCollection();
        }
    }
}
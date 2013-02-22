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

        private readonly HttpBrowserCapabilitiesBase m_BrowserCapabilities;
        public override HttpBrowserCapabilitiesBase Browser
        {
            get
            {
                return m_BrowserCapabilities;
            }
        }

        public HttpRequestStub(NameValueCollection headers = null)
            : this(headers, new HttpBrowserCapabilitiesStub(string.Empty, 0))
        {
        }

        public HttpRequestStub(NameValueCollection headers, HttpBrowserCapabilitiesBase browserCapabilities)
        {
            m_BrowserCapabilities = browserCapabilities;
            m_Headers = headers ?? new NameValueCollection();
        }
    }
}
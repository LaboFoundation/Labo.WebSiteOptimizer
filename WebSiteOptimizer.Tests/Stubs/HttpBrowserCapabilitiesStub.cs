using System;
using System.Web;

namespace Labo.WebSiteOptimizer.Tests.Stubs
{
    public class HttpBrowserCapabilitiesStub : HttpBrowserCapabilitiesBase
    {
        private readonly string m_Browser;
        private readonly int m_MajorVersion;

        public HttpBrowserCapabilitiesStub(string browser, int majorVersion)
        {
            m_Browser = browser;
            m_MajorVersion = majorVersion;
        }

        public override string Browser
        {
            get
            {
                return m_Browser;
            }
        }

        public override int MajorVersion
        {
            get
            {
                return m_MajorVersion;
            }
        }

        public override bool IsBrowser(string browserName)
        {
            return string.Compare(browserName, m_Browser, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}

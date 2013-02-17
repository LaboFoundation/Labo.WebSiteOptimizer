using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace Labo.WebSiteOptimizer.Tests.Stubs
{
    public sealed class HttpResponseStub : HttpResponseBase
    {
        private readonly Stream m_Stream;
        private readonly HttpCachePolicyStub m_Cache;
        private readonly NameValueCollection m_Headers;

        public HttpCachePolicyStub CacheStub
        {
            get { return m_Cache; }
        }

        public HttpResponseStub(Stream stream, NameValueCollection headers = null)
        {
            m_Stream = stream;
            m_Cache = new HttpCachePolicyStub();
            m_Headers = headers ?? new NameValueCollection(StringComparer.OrdinalIgnoreCase);
        }

        public override void BinaryWrite(byte[] buffer)
        {
            m_Stream.Write(buffer, 0, buffer.Length);
        }

        public override NameValueCollection Headers
        {
            get
            {
                return m_Headers;
            }
        }

        public override void AppendHeader(string name, string value)
        {
            Headers[name] += ";" + value;
        }

        public override string Charset { get; set; }

        public override string ContentType { get; set; }

        public override HttpCachePolicyBase Cache
        {
            get
            {
                return m_Cache;
            }
        }
    }
}

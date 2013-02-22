using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.Extensions;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement
{
    [TestFixture]
    public class HttpResponseCompressorTestFixture
    {
        [Test]
        [TestCase("gzip", "gzip")]
        [TestCase("deflate", "deflate")]
        [TestCase("gzip,deflate", "gzip")]
        [TestCase("", null)]
        public void Compress(string acceptEncodingHeader, string expectedContentEncodingHeader)
        {
            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            DateTime now = DateTime.Now;
            dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(now);
            HttpResponseStub httpResponseStub = new HttpResponseStub(new MemoryStream());
            HttpRequestStub httpRequestStub = new HttpRequestStub();
            httpRequestStub.Headers.Add("Accept-Encoding", acceptEncodingHeader);
            Mock<HttpContextBase> httpContextMock = HttpContextMockHelper.CreateHttpContext(httpResponseStub, httpRequestStub);
            HttpResponseCompressor httpResponseCompressor = new HttpResponseCompressor();
            httpResponseCompressor.Compress(httpContextMock.Object, httpResponseCompressor.GetRequestCompressionType(httpContextMock.Object));

            if (expectedContentEncodingHeader.IsNullOrEmpty())
            {
                Assert.AreEqual(null, httpContextMock.Object.Response.Headers["Content-Encoding"]);
            }
            else
            {
                Assert.IsTrue(httpContextMock.Object.Response.Headers["Content-Encoding"].Contains(expectedContentEncodingHeader));
            }
        }

        [Test]
        //IE 7
        [TestCase("gzip", "IE", 7, CompressionType.Gzip)]
        [TestCase("deflate", "IE", 7, CompressionType.Deflate)]
        [TestCase("gzip,deflate", "IE", 7, CompressionType.Gzip)]
        [TestCase("", "IE", 7, CompressionType.None)]
        [TestCase("something", "IE", 7, CompressionType.None)]
        //IE 6
        [TestCase("gzip", "IE", 6, CompressionType.None)]
        [TestCase("deflate", "IE", 6, CompressionType.None)]
        [TestCase("gzip,deflate", "IE", 6, CompressionType.None)]
        [TestCase("", "IE", 6, CompressionType.None)]
        [TestCase("something", "IE", 6, CompressionType.None)]
        //OtherThanIE
        [TestCase("gzip", "OtherThanIE", 6, CompressionType.Gzip)]
        [TestCase("deflate", "OtherThanIE", 6, CompressionType.Deflate)]
        [TestCase("gzip,deflate", "OtherThanIE", 6, CompressionType.Gzip)]
        [TestCase("", "OtherThanIE", 6, CompressionType.None)]
        [TestCase("something", "OtherThanIE", 6, CompressionType.None)]
        public void GetCompressionType(string acceptEncodingHeader, string browser, int browserMajorVersion, CompressionType expectedCompressionType)
        {
            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            DateTime now = DateTime.Now;
            dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(now);
            HttpResponseStub httpResponseStub = new HttpResponseStub(new MemoryStream());
            HttpRequestStub httpRequestStub = new HttpRequestStub(new NameValueCollection(), new HttpBrowserCapabilitiesStub(browser, browserMajorVersion));
            httpRequestStub.Headers.Add("Accept-Encoding", acceptEncodingHeader);
            Mock<HttpContextBase> httpContextMock = HttpContextMockHelper.CreateHttpContext(httpResponseStub, httpRequestStub);
            HttpResponseCompressor httpResponseCompressor = new HttpResponseCompressor();

            Assert.AreEqual(expectedCompressionType, httpResponseCompressor.GetRequestCompressionType(httpContextMock.Object));
        }
    }
}

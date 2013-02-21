using System;
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
        [TestCase("gzip", CompressionType.Gzip)]
        [TestCase("deflate", CompressionType.Deflate)]
        [TestCase("gzip,deflate", CompressionType.Gzip)]
        [TestCase("", CompressionType.None)]
        [TestCase("something", CompressionType.None)]
        public void GetCompressionType(string acceptEncodingHeader, CompressionType expectedCompressionType)
        {
            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            DateTime now = DateTime.Now;
            dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(now);
            HttpResponseStub httpResponseStub = new HttpResponseStub(new MemoryStream());
            HttpRequestStub httpRequestStub = new HttpRequestStub();
            httpRequestStub.Headers.Add("Accept-Encoding", acceptEncodingHeader);
            Mock<HttpContextBase> httpContextMock = HttpContextMockHelper.CreateHttpContext(httpResponseStub, httpRequestStub);
            HttpResponseCompressor httpResponseCompressor = new HttpResponseCompressor();

            Assert.AreEqual(expectedCompressionType, httpResponseCompressor.GetRequestCompressionType(httpContextMock.Object));
        }
    }
}

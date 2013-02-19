using System.IO;
using System.Web;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.Tests.Stubs;
using Moq;
using NUnit.Framework;
namespace Labo.WebSiteOptimizer.Tests.ResourceManagement
{
    [TestFixture]
    public class ClientCompressionHelperTestFixture
    {
        [Test]
        [TestCase("gzip", true, CompressionType.Gzip)]
        [TestCase("deflate", true, CompressionType.Deflate)]
        [TestCase("gzip,deflate", true, CompressionType.Gzip)]
        [TestCase("", true, CompressionType.None)]
        [TestCase("", false, CompressionType.None)]
        [TestCase("gzip", false, CompressionType.None)]
        [TestCase("deflate", false, CompressionType.None)]
        [TestCase("gzip,deflate", false, CompressionType.None)]
        public void GetCompressionType(string acceptEncodingHeader, bool compressResource, CompressionType compressionType)
        {
            HttpRequestStub httpRequestStub = new HttpRequestStub();
            httpRequestStub.Headers.Add("Accept-Encoding", acceptEncodingHeader);
            Mock<HttpContextBase> httpContextBaseMock = HttpContextMockHelper.CreateHttpContext(new HttpResponseStub(new MemoryStream()), httpRequestStub);

            Assert.AreEqual(compressionType, ClientCompressionHelper.GetCompressionType(httpContextBaseMock.Object, compressResource));
        }
    }
}

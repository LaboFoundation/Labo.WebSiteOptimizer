using System;
using System.IO;
using System.Web;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement
{
    [TestFixture]
    public class HttpResponseCacherTestFixture
    {
        [Test]
        public void Cache()
        {
            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            DateTime now = DateTime.Now;
            dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(now);
            HttpResponseStub httpResponseStub = new HttpResponseStub(new MemoryStream());
            HttpRequestStub httpRequestStub = new HttpRequestStub();
            Mock<HttpContextBase> httpContext = HttpContextMockHelper.CreateHttpContext(httpResponseStub, httpRequestStub);

            HttpResponseCacher httpResponseCacher = new HttpResponseCacher(dateTimeProviderMock.Object);
            httpResponseCacher.Cache(httpContext.Object, now);

            Assert.AreEqual("Accept-Encoding", httpResponseStub.CacheStub.VaryByCustom);
            Assert.AreEqual(now, httpResponseStub.CacheStub.LastModifiedDate);
            Assert.AreEqual(true, httpResponseStub.CacheStub.ValidUntilExpires);
            Assert.AreEqual(HttpCacheability.Public, httpResponseStub.CacheStub.Cacheability);
            Assert.AreEqual(now.AddYears(1), httpResponseStub.CacheStub.ExpireDate);
            Assert.AreEqual(TimeSpan.FromDays(365), httpResponseStub.CacheStub.MaxAge);
        }

        [Test]
        public void Cache_ThrowsExceptionWhenHttpContextIsNull()
        {
            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            DateTime now = DateTime.Now;
            dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(now);
            HttpResponseCacher httpResponseCacher = new HttpResponseCacher(dateTimeProviderMock.Object);
            Assert.Throws<ArgumentNullException>(() => httpResponseCacher.Cache(null, now));
        }
    }
}

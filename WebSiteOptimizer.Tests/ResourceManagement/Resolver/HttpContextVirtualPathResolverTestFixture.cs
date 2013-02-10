using System;
using System.Web;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Resolver
{
    [TestFixture]
    public class HttpContextVirtualPathResolverTestFixture
    {
        [Test]
        [TestCase("~/file.css", "file.css")]
        [TestCase("~\\file.css", "file.css")]
        public void Resolve(string path, string expectedResult)
        {
            HttpContextVirtualPathResolver resolver = new HttpContextVirtualPathResolver();
            try
            {
                Mock<HttpContextBase> mock = new Mock<HttpContextBase>();
                mock.Setup(x => x.Server.MapPath(path)).Returns(new WindowsVirtualPathResolver().Resolve(path));
                Utility.HttpContextWrapper.Context = mock.Object;
                string resolve = resolver.Resolve(path);
                Assert.AreEqual(Environment.CurrentDirectory + "\\" + expectedResult, resolve);
            }
            finally
            {
                Utility.HttpContextWrapper.Context = null;
            }
           
        }

        [Test]
        public void Resolve_WithNullPath()
        {
            HttpContextVirtualPathResolver resolver = new HttpContextVirtualPathResolver();
            Assert.Throws<ArgumentNullException>(() => resolver.Resolve(null));
        }

        [Test]
        public void Resolve_WhenHttpContextNull()
        {
            HttpContextVirtualPathResolver resolver = new HttpContextVirtualPathResolver();
            Assert.Throws<InvalidOperationException>(() => resolver.Resolve("~/file.css"));
        }
    }
}

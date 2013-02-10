using System;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Resolver
{
    [TestFixture]
    public class WindowsVirtualPathResolverTestFixture
    {
        [Test]
        [TestCase("~/file.css", "file.css")]
        [TestCase("~\\file.css", "file.css")]
        public void Resolve(string path, string expectedResult)
        {
            WindowsVirtualPathResolver resolver = new WindowsVirtualPathResolver();
            string resolve = resolver.Resolve(path);

            Assert.AreEqual(Environment.CurrentDirectory + "\\" + expectedResult, resolve);
        }

        [Test]
        public void Resolve_WithNullPath()
        {
            WindowsVirtualPathResolver resolver = new WindowsVirtualPathResolver();
            Assert.Throws<ArgumentNullException>(() => resolver.Resolve(null));
        }
    }
}

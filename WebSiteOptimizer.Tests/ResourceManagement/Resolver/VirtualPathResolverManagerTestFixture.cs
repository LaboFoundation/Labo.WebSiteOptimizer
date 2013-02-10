using System;
using System.Web;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Resolver
{
    [TestFixture]
    public class VirtualPathResolverManagerTestFixture
    {
        [Test]
        public void Resolve_HttpContextVirtualPathProviderIsCalled_WhenHttpContextIsNotNull()
        {
            try
            {
                Mock<HttpContextBase> httpContextMock = new Mock<HttpContextBase>();
                Mock<IVirtualPathResolver> httpContextVirtualPathProviderMock = new Mock<IVirtualPathResolver>();
                httpContextVirtualPathProviderMock.Setup(x => x.Resolve(It.IsAny<string>()));
                Mock<IVirtualPathResolver> windowsContextVirtualPathProviderMock = new Mock<IVirtualPathResolver>();
                windowsContextVirtualPathProviderMock.Setup(x => x.Resolve(It.IsAny<string>()));

                Utility.HttpContextWrapper.Context = httpContextMock.Object;
                VirtualPathResolverManager manager = new VirtualPathResolverManager(() => httpContextVirtualPathProviderMock.Object, () => windowsContextVirtualPathProviderMock.Object);
                manager.Resolve("~/file.css");

                httpContextVirtualPathProviderMock.Verify(x => x.Resolve("~/file.css"), Times.Exactly(1));
                windowsContextVirtualPathProviderMock.Verify(x => x.Resolve(It.IsAny<string>()), Times.Never());
            }
            finally
            {
                Utility.HttpContextWrapper.Context = null;
            }
        }

        [Test]
        public void Resolve_WindowsVirtualPathProviderIsCalled_WhenHttpContextIsNull()
        {
            Mock<IVirtualPathResolver> httpContextVirtualPathProviderMock = new Mock<IVirtualPathResolver>();
            httpContextVirtualPathProviderMock.Setup(x => x.Resolve(It.IsAny<string>()));
            Mock<IVirtualPathResolver> windowsContextVirtualPathProviderMock = new Mock<IVirtualPathResolver>();
            windowsContextVirtualPathProviderMock.Setup(x => x.Resolve(It.IsAny<string>()));


            VirtualPathResolverManager manager = new VirtualPathResolverManager(() => httpContextVirtualPathProviderMock.Object, () => windowsContextVirtualPathProviderMock.Object);
            manager.Resolve("~/file.css");

            windowsContextVirtualPathProviderMock.Verify(x => x.Resolve("~/file.css"), Times.Exactly(1));
            httpContextVirtualPathProviderMock.Verify(x => x.Resolve(It.IsAny<string>()), Times.Never());
        }
    }
}

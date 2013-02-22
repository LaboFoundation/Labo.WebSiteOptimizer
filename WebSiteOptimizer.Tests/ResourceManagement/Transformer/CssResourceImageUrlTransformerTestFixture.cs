using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Labo.WebSiteOptimizer.ResourceManagement.Transformer;
using Labo.WebSiteOptimizer.ResourceManagement.VirtualPath;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.Transformer
{
    [TestFixture]
    public class CssResourceImageUrlTransformerTestFixture
    {
        [Test]
        public void Transform()
        {
            Mock<IVirtualPathProvider> virtualPathProviderMock = new Mock<IVirtualPathProvider>();
            CssResourceImageUrlTransformer transformer = new CssResourceImageUrlTransformer(virtualPathProviderMock.Object);
            ResourceReadInfo result = transformer.Transform(new ResourceReadInfo
                {
                    ResourceElement = new ResourceElement
                        {
                            FileName = "~/Content/css/site.css"
                        },
                    ResourceInfo = new ResourceInfo
                        {
                            Content = "background-image:url(../images/x.jpg)"
                        }
                });
        }
    }
}

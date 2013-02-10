using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.ResourceReader
{
    [TestFixture]
    public class ResourceReaderManagerTestFixture
    {
        [Test]
        public void ReadResource_FileSystemResouceReaderCalled_WhenIsEmbeddedResourceFalse()
        {
            const string testDataPath = "ResourceManagement/_TestData/FileSystemResource.css";
            Mock<IResourceReader> fileSystemResourceReader = new Mock<IResourceReader>();
            fileSystemResourceReader.Setup(x => x.ReadResource(testDataPath)).Returns(new ResourceInfo());

            Mock<IResourceReader> embeddedResourceReader = new Mock<IResourceReader>();
            embeddedResourceReader.Setup(x => x.ReadResource(testDataPath)).Returns(new ResourceInfo());

            ResourceReaderManager resourceReaderManager = new ResourceReaderManager(() => embeddedResourceReader.Object, () => fileSystemResourceReader.Object);
            resourceReaderManager.ReadResource(new ResourceReadOptions{FileName = testDataPath, IsEmbeddedResource = false});

            fileSystemResourceReader.Verify(x => x.ReadResource(testDataPath), Times.Exactly(1));
            embeddedResourceReader.Verify(x => x.ReadResource(testDataPath), Times.Never());
        }

        [Test]
        public void ReadResource_EmbeddedResouceReaderCalled_WhenIsEmbeddedResourceTrue()
        {
            const string testDataPath = "Labo.WebSiteOptimizer.Tests://ResourceManagement._TestData.EmbeddedResource.css";
            Mock<IResourceReader> fileSystemResourceReader = new Mock<IResourceReader>();
            fileSystemResourceReader.Setup(x => x.ReadResource(testDataPath)).Returns(new ResourceInfo());

            Mock<IResourceReader> embeddedResourceReader = new Mock<IResourceReader>();
            embeddedResourceReader.Setup(x => x.ReadResource(testDataPath)).Returns(new ResourceInfo());

            ResourceReaderManager resourceReaderManager = new ResourceReaderManager(() => embeddedResourceReader.Object, () => fileSystemResourceReader.Object);
            resourceReaderManager.ReadResource(new ResourceReadOptions { FileName = testDataPath, IsEmbeddedResource = true });

            fileSystemResourceReader.Verify(x => x.ReadResource(testDataPath), Times.Never());
            embeddedResourceReader.Verify(x => x.ReadResource(testDataPath), Times.Exactly(1));
        }
    }
}

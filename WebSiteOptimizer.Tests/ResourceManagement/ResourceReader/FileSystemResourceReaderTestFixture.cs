using System;
using System.IO;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.ResourceReader
{
    [TestFixture]
    public class FileSystemResourceReaderTestFixture
    {
        [Test]
        public void ReadResource()
        {
            const string testDataPath = "ResourceManagement/_TestData/FileSystemResource.css";
            string resourcePath = Path.Combine(Environment.CurrentDirectory, testDataPath);
            Mock<IVirtualPathResolver> virtualPathResolverMock = new Mock<IVirtualPathResolver>();
            virtualPathResolverMock.Setup(x => x.Resolve(testDataPath)).Returns(resourcePath);

            FileSystemResourceReader resourceReader = new FileSystemResourceReader(virtualPathResolverMock.Object);
            ResourceInfo readResource = resourceReader.ReadResource(testDataPath);
            Assert.IsNotNull(readResource);
            Assert.AreEqual("body{ background-color: beige; }", readResource.Content);
            Assert.AreEqual(resourcePath, readResource.DependentFile);
            Assert.AreEqual(File.GetLastWriteTime(resourcePath), readResource.ModifyDate);
        }

        [Test]
        public void ReadResource_WithNullPath()
        {
            const string testDataPath = "ResourceManagement/_TestData/FileSystemResource.css";
            Mock<IVirtualPathResolver> virtualPathResolverMock = new Mock<IVirtualPathResolver>();
            virtualPathResolverMock.Setup(x => x.Resolve(It.IsAny<string>())).Returns((string)null);

            FileSystemResourceReader resourceReader = new FileSystemResourceReader(virtualPathResolverMock.Object);
            Assert.Throws<ArgumentNullException>(() => resourceReader.ReadResource(testDataPath));
            Assert.Throws<ArgumentNullException>(() => resourceReader.ReadResource(null));
        }

        [Test]
        public void ReadResource_WithNotExistingFile()
        {
            const string testDataPath = "ResourceManagement/_TestData/NotExistingResource.css";
            Mock<IVirtualPathResolver> virtualPathResolverMock = new Mock<IVirtualPathResolver>();
            virtualPathResolverMock.Setup(x => x.Resolve(testDataPath)).Returns(testDataPath);

            FileSystemResourceReader resourceReader = new FileSystemResourceReader(virtualPathResolverMock.Object);
            Assert.Throws<FileNotFoundException>(() => resourceReader.ReadResource(testDataPath));
        }
    }
}

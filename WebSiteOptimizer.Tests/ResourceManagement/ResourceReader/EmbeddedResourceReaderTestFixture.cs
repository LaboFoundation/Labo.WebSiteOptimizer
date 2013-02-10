using System;
using System.IO;
using System.Reflection;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Labo.WebSiteOptimizer.Utility.Exceptions;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.ResourceReader
{
    [TestFixture]
    public class EmbeddedResourceReaderTestFixture
    {
        [Test]
        public void ReadResource()
        {
            const string testDataPath = "Labo.WebSiteOptimizer.Tests://ResourceManagement._TestData.EmbeddedResource.css";
            string assemblyLocation = Path.Combine(Environment.CurrentDirectory, Path.GetFileName(Assembly.GetAssembly(typeof(EmbeddedResourceReaderTestFixture)).CodeBase));

            EmbeddedResourceResolver resourceReader = new EmbeddedResourceResolver();
            ResourceInfo readResource = resourceReader.ReadResource(testDataPath);
            Assert.IsNotNull(readResource);
            Assert.AreEqual("body{ font-size: 12px; }", readResource.Content);
            Assert.AreEqual(assemblyLocation, readResource.DependentFile);
            Assert.AreEqual(File.GetLastWriteTime(assemblyLocation), readResource.ModifyDate);
        }

        [Test]
        public void ReadResource_WithNullPath()
        {
            EmbeddedResourceResolver resourceReader = new EmbeddedResourceResolver();
            Assert.Throws<ArgumentNullException>(() => resourceReader.ReadResource(null));
        }

        [Test]
        [TestCase("")]
        [TestCase("AnAssembly://")]
        [TestCase("://")]
        [TestCase("://AnEmbeddedResource.css")]
        public void ReadResource_WithInvalidPathFormat(string path)
        {
            EmbeddedResourceResolver resourceReader = new EmbeddedResourceResolver();
            Assert.Throws<InvalidPathFormatException>(() => resourceReader.ReadResource(path));
        }

        [Test]
        public void ReadResource_NotExistingAssembly()
        {
            EmbeddedResourceResolver resourceReader = new EmbeddedResourceResolver();
            Assert.Throws<AssemblyNotFoundException>(() => resourceReader.ReadResource("NotExisting.Assembly://AnEmbeddedResource.css"));
        }

        [Test]
        public void ReadResource_NotExistingResource()
        {
            EmbeddedResourceResolver resourceReader = new EmbeddedResourceResolver();
            Assert.Throws<EmbeddedResourceNotFoundException>(() => resourceReader.ReadResource("Labo.WebSiteOptimizer.Tests://AnEmbeddedResource.css"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Cacher;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.ResourceManagement.Hasher;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Moq;
using NUnit.Framework;
namespace Labo.WebSiteOptimizer.Tests.ResourceManagement
{
    public sealed class ResourceProcessorTestFixture
    {
        [Test]
        public void CalculateLastModifyDate_MustRetrieveTheBiggestDate()
        {
            Assert.AreEqual(new DateTime(2013, 2, 17, 16, 16, 30, 10), ResourceProcessor.CalculateLastModifyDate(new List<ResourceReadInfo>
                {
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {ModifyDate = new DateTime(2013,2,17, 16, 16, 30, 10)}}
                }));
            Assert.AreEqual(new DateTime(2013, 2, 17, 16, 16, 30, 50), ResourceProcessor.CalculateLastModifyDate(new List<ResourceReadInfo>
                {
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {ModifyDate = new DateTime(2013,2,17, 16, 16, 30, 11)}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {ModifyDate = new DateTime(2013,2,17, 16, 16, 30, 10)}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {ModifyDate = new DateTime(2013,2,17, 16, 16, 30, 50)}}
                }));
            Assert.AreEqual(new DateTime(2013, 2, 19, 16, 16, 30, 10), ResourceProcessor.CalculateLastModifyDate(new List<ResourceReadInfo>
                {
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {ModifyDate = new DateTime(2013,2,17, 16, 16, 30, 10)}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {ModifyDate = new DateTime(2013,2,18, 16, 16, 30, 10)}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {ModifyDate = new DateTime(2013,2,19, 16, 16, 30, 10)}}
                }));
        }

        [Test]
        public void CalculateLastModifyDate_ThrowsException_WhenResourceListIsEmpty()
        {
            Assert.Throws<ResourceProcessorException>(() => ResourceProcessor.CalculateLastModifyDate(new List<ResourceReadInfo>()));
        }

        [Test]
        public void CalculateDependentFiles_MustRetrieveDistinctSetOfResourceFiles()
        {
            Assert.AreEqual(new HashSet<string>{ "1.js" }, ResourceProcessor.CalculateDependentFiles(new List<ResourceReadInfo>
                {
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {DependentFile = "1.js"}}
                }));
            Assert.AreEqual(new HashSet<string>{ "1.js", "2.js" }, ResourceProcessor.CalculateDependentFiles(new List<ResourceReadInfo>
                {
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {DependentFile = "1.js"}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {DependentFile = "2.js"}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {DependentFile = "1.js"}}
                }));
            Assert.AreEqual(new HashSet<string>{ "1.js" }, ResourceProcessor.CalculateDependentFiles(new List<ResourceReadInfo>
                {
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {DependentFile = "1.js"}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {DependentFile = "1.js"}},
                    new ResourceReadInfo {ResourceInfo = new ResourceInfo {DependentFile = "1.js"}}
                }));
             Assert.AreEqual(new HashSet<string>(), ResourceProcessor.CalculateDependentFiles(new List<ResourceReadInfo>()));
        }

        [Test]
        [TestCase(true, null, true)]
        [TestCase(true, true, true)]
        [TestCase(true, false, false)]
        [TestCase(false, null, false)]
        [TestCase(false, true, true)]
        [TestCase(false, false, false)]
        public void MustMinify(bool resourceGroupMinify, bool? resourceElementMinify, bool expectedResult)
        {
            Assert.AreEqual(expectedResult, ResourceProcessor.MustMinify(new ResourceElementGroup{ Minify = resourceGroupMinify}, new ResourceElement{ Minify = resourceElementMinify }));
        }

        [Test]
        public void MinifyContent()
        {
            Mock<IJsMinifier> jsMinifierMock = new Mock<IJsMinifier>();
            jsMinifierMock.Setup(x => x.Minify(It.IsAny<string>(), true, false, null)).Returns(It.IsAny<string>);
            Mock<ICssMinifier> cssMinifierMock = new Mock<ICssMinifier>();
            cssMinifierMock.Setup(x => x.Minify(It.IsAny<string>())).Returns(It.IsAny<string>);
            ResourceProcessor resourceProcessor = CreateResourceProcessor(new Mock<IResourceCacher>(), new Mock<IResourceReaderManager>(),
                new Mock<ICompressionFactory>(), new Mock<IResourceHasher>(), jsMinifierMock, cssMinifierMock);
            
            const string js = "function sum(x,y){return x + y;}";
            resourceProcessor.MinifyContent(ResourceType.Js, js);
            jsMinifierMock.Verify(x => x.Minify(js, true, false, null), Times.Exactly(1));

            const string css = "body{background-color:white;}";
            resourceProcessor.MinifyContent(ResourceType.Css, css);
            cssMinifierMock.Verify(x => x.Minify(css), Times.Exactly(1));
        }

        [Test]
        public void MinifyContent_ThrowsException_WhenUnsupportedResourceIsGiven()
        {
            ResourceProcessor resourceProcessor = CreateResourceProcessor(new Mock<IResourceCacher>(), new Mock<IResourceReaderManager>(),
                new Mock<ICompressionFactory>(), new Mock<IResourceHasher>(), new Mock<IJsMinifier>(), new Mock<ICssMinifier>());

            Assert.Throws<ResourceProcessorException>(() => resourceProcessor.MinifyContent(ResourceType.Img, string.Empty));

        }

        [Test]
        [TestCase("some content", CompressionType.Gzip)]
        [TestCase("some content", CompressionType.Deflate)]
        public void CompressContent(string content, CompressionType compressionType)
        {
            Mock<ICompressionFactory> compressionFactoryMock = new Mock<ICompressionFactory>();
            Mock<ICompressor> compressorMock = new Mock<ICompressor>();
            compressionFactoryMock.Setup(x => x.CreateCompressor(It.IsAny<CompressionType>())).Returns(() => compressorMock.Object);

            ResourceProcessor resourceProcessor = CreateResourceProcessor(new Mock<IResourceCacher>(), new Mock<IResourceReaderManager>(),
                compressionFactoryMock, new Mock<IResourceHasher>(), new Mock<IJsMinifier>(), new Mock<ICssMinifier>());

            byte[] contentBytes = Encoding.UTF8.GetBytes(content);

            resourceProcessor.CompressContent(compressionType, content);

            compressionFactoryMock.Verify(x => x.CreateCompressor(compressionType), Times.Exactly(1));
            compressorMock.Verify(x => x.Compress(contentBytes), Times.Exactly(1));
        }

        [Test]
        [TestCase("some content", "file.css", "01.01.2013")]
        public void ReadResource(string content, string fileName, string fileModifyDate)
        {
            Mock<IResourceReaderManager> resourceReaderManagerMock = new Mock<IResourceReaderManager>();
            ResourceInfo resourceInfo = new ResourceInfo { Content = content, DependentFile = fileName, ModifyDate = DateTime.Parse(fileModifyDate, CultureInfo.InvariantCulture) };
            resourceReaderManagerMock.Setup(x => x.ReadResource(It.IsAny<ResourceReadOptions>()))
                                     .Returns(() => resourceInfo);
            ResourceProcessor resourceProcessor = CreateResourceProcessor(new Mock<IResourceCacher>(), resourceReaderManagerMock,
               new Mock<ICompressionFactory>(), new Mock<IResourceHasher>(), new Mock<IJsMinifier>(), new Mock<ICssMinifier>());

            ResourceElement resourceElement = new ResourceElement {FileName = fileName, IsEmbeddedResource = true};
            IList<ResourceReadInfo> resourceReadInfos = resourceProcessor.ReadResources(new List<ResourceElement> {resourceElement});
            List<ResourceReadInfo> expectedResourceInfos = new List<ResourceReadInfo>
                {
                    new ResourceReadInfo {ResourceInfo = resourceInfo, ResourceElement = resourceElement}
                };
            Assert.AreEqual(expectedResourceInfos[0].ResourceElement, resourceReadInfos[0].ResourceElement);
            Assert.AreEqual(expectedResourceInfos[0].ResourceInfo, resourceReadInfos[0].ResourceInfo);
        }

        [Test]
        [TestCase(false, "Content 1;", "Content 2;", "Content 1;Content 2;")]
        public void MinifyAndCombineResources(bool minify, string content1, string content2, string expectedResult)
        {
            ResourceProcessor resourceProcessor = CreateResourceProcessor(new Mock<IResourceCacher>(), new Mock<IResourceReaderManager>(), 
               new Mock<ICompressionFactory>(), new Mock<IResourceHasher>(), new Mock<IJsMinifier>(), new Mock<ICssMinifier>());

            Mock<IResourceElementGroupConfiguration> resourceElementGroupConfigurationMock = new Mock<IResourceElementGroupConfiguration>();
            resourceElementGroupConfigurationMock.Setup(x => x.Minify).Returns(() => minify);

            List<ResourceReadInfo> resourceReadInfos = new List<ResourceReadInfo>
                {
                    new ResourceReadInfo
                        {
                            ResourceElement = new ResourceElement {Minify = minify},
                            ResourceInfo = new ResourceInfo {Content = content1}
                        },
                    new ResourceReadInfo
                        {
                            ResourceElement = new ResourceElement {Minify = minify},
                            ResourceInfo = new ResourceInfo {Content = content2}
                        }
                };
            string processedContent = resourceProcessor.MinifyAndCombineResources(resourceElementGroupConfigurationMock.Object, ResourceType.Js, resourceReadInfos);
            Assert.AreEqual(expectedResult, processedContent);
        }

        private static ResourceProcessor CreateResourceProcessor(Mock<IResourceCacher> resourceCacherMock, Mock<IResourceReaderManager> resourceReaderManagerMock,
                                                                 Mock<ICompressionFactory> compressionFactoryMock, Mock<IResourceHasher> resourceHasherMock,
                                                                 Mock<IJsMinifier> jsMinifierMock, Mock<ICssMinifier> cssMinifierMock)
        {
            ResourceProcessor resourceProcessor = new ResourceProcessor(resourceCacherMock.Object,
                                                                        resourceReaderManagerMock.Object,
                                                                        compressionFactoryMock.Object,
                                                                        resourceHasherMock.Object,
                                                                        jsMinifierMock.Object, cssMinifierMock.Object);
            return resourceProcessor;
        }
    }
}

using System;
using System.IO;
using System.Web;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement
{
    [TestFixture]
    public class ResourceHandlerTestFixture
    {
        [Test]
        public void HandleResource_HttpResponseContentType_IsJavascript_WhenJavascriptResourceHandled()
        {
            AssertContentType("js", "~/js.js", ResourceType.Js, false, "text/javascript");
        }

        [Test]
        public void HandleResource_HttpResponseContentType_IsCss_WhenCssResourceHandled()
        {
            AssertContentType("css", "~/css.css", ResourceType.Css, false, "text/css");
        }

        [Test]
        public void HandleResource_HttpResponseContentType_ThrowsException_WhenUnsupportedResourceHandled()
        {
            Assert.Throws<ResourceHandlerException>(() => AssertContentType("css", "~/css.css", ResourceType.NotSet, false, "text/css"));
        }

        [Test]
        [TestCase("gzip", true, "gzip")]
        [TestCase("deflate", true, "deflate")]
        [TestCase("gzip,deflate", true, "gzip")]
        [TestCase("", true, null)]
        [TestCase("", false, null)]
        [TestCase("gzip", false, null)]
        [TestCase("deflate", false, null)]
        [TestCase("gzip,deflate", false, null)]
        public void HandleResource_AssertCompressionContentEncoding_AccordingToAccepEncodingHeaderAndCompressResourceConfiguration(string acceptEncodingHeader, bool compressResource, string expectedContentEncodingHeader)
        {
            HttpRequestStub httpRequestStub = new HttpRequestStub();
            httpRequestStub.Headers.Add("Accept-Encoding", acceptEncodingHeader);
            Mock<HttpContextBase> httpContextBaseMock = HandleResource("js", "~/js.js", ResourceType.Js, compressResource, httpRequestStub);

            if (expectedContentEncodingHeader.IsNullOrEmpty())
            {
                Assert.AreEqual(null, httpContextBaseMock.Object.Response.Headers["Content-Encoding"]);
            }
            else
            {
                Assert.IsTrue(httpContextBaseMock.Object.Response.Headers["Content-Encoding"].Contains(expectedContentEncodingHeader));                
            }
        }

        [Test]
        public void HandleResource_AssertResponseCharset()
        {
            Mock<HttpContextBase> httpContextBaseMock = HandleResource("js", "~/js.js", ResourceType.Js, false);
            Assert.AreEqual("utf-8", httpContextBaseMock.Object.Response.Charset);
        }

        [Test]
        public void HandleResource_AssertResponseCachePolicy()
        {
            Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
            DateTime now = DateTime.Now;
            dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(now);
            HttpResponseStub httpResponseStub = new HttpResponseStub(new MemoryStream());
            HandleResource("js", "~/js.js", ResourceType.Js, false, new HttpRequestStub(), httpResponseStub, dateTimeProviderMock.Object, new ProcessedResourceGroupInfo
                {
                    Content = new byte[0],
                    LastModifyDate = now
                });

            Assert.AreEqual("Accept-Encoding", httpResponseStub.CacheStub.VaryByCustom);
            Assert.AreEqual(now, httpResponseStub.CacheStub.LastModifiedDate);
            Assert.AreEqual(true, httpResponseStub.CacheStub.ValidUntilExpires);
            Assert.AreEqual(HttpCacheability.Public, httpResponseStub.CacheStub.Cacheability);
            Assert.AreEqual(now.AddYears(1), httpResponseStub.CacheStub.ExpireDate);
            Assert.AreEqual(TimeSpan.FromDays(365), httpResponseStub.CacheStub.MaxAge);
        }

        private static void AssertContentType(string resourceGroupName, string fileName, ResourceType resourceType, bool compress, string expectedContentType)
        {
            Mock<HttpContextBase> httpContextBaseMock = HandleResource(resourceGroupName, fileName, resourceType, compress);

            Assert.AreEqual(expectedContentType, httpContextBaseMock.Object.Response.ContentType);
        }

        private static Mock<HttpContextBase> HandleResource(string resourceGroupName, string fileName, ResourceType resourceType, bool compress)
        {
            return HandleResource(resourceGroupName, fileName, resourceType, compress, new HttpRequestStub(), new HttpResponseStub(new MemoryStream()), new DefaultDateTimeProvider(), new ProcessedResourceGroupInfo
                {
                    Content = new byte[0],
                    LastModifyDate = DateTime.Now
                });
        }

        private static Mock<HttpContextBase> HandleResource(string resourceGroupName, string fileName, ResourceType resourceType, bool compress, HttpRequestBase httpRequest)
        {
            return HandleResource(resourceGroupName, fileName, resourceType, compress, httpRequest, new HttpResponseStub(new MemoryStream()), new DefaultDateTimeProvider(), new ProcessedResourceGroupInfo
                {
                    Content = new byte[0],
                    LastModifyDate = DateTime.Now
                });
        }

        private static Mock<HttpContextBase> HandleResource(string resourceGroupName, string fileName, ResourceType resourceType, bool compress, HttpRequestBase httpRequest, HttpResponseBase httpResponseStub, IDateTimeProvider dateTimeProvider, ProcessedResourceGroupInfo processedResourceGroupInfo)
        {
            ResourceElementGroup resourceElementGroup = new ResourceElementGroup
                {
                    Name = resourceGroupName,
                    ResourceType = resourceType,
                    Compress = compress,
                    Resources = new ResourceElementCollection {new ResourceElement {FileName = fileName}}
                };
            Mock<IWebResourceConfigurationProvider> webResourceConfigurationMock = CreateWebResourceConfigurationMock(resourceElementGroup);
            Mock<HttpContextBase> httpContextBaseMock = CreateHttpContext(httpResponseStub, httpRequest);

            Mock<IResourceProcessor> resourceProcessorMock = new Mock<IResourceProcessor>();
            resourceProcessorMock.Setup(x => x.ProcessResource(resourceElementGroup, It.IsAny<CompressionType>()))
                                 .Returns(() => processedResourceGroupInfo);

            ResourceHandler resourceHandler = new ResourceHandler(resourceProcessorMock.Object, webResourceConfigurationMock.Object, dateTimeProvider);
            resourceHandler.HandleResource(httpContextBaseMock.Object, resourceType, resourceGroupName);
            return httpContextBaseMock;
        }

        private static Mock<IWebResourceConfigurationProvider> CreateWebResourceConfigurationMock(ResourceElementGroup resourceElementGroup)
        {
            Mock<IWebResourceConfigurationProvider> webResourceConfigurationMock = new Mock<IWebResourceConfigurationProvider>();
            webResourceConfigurationMock.Setup(x => x.GetResourceElementGroup(resourceElementGroup.ResourceType, resourceElementGroup.Name)).Returns(() => resourceElementGroup);
            return webResourceConfigurationMock;
        }

        private static Mock<HttpContextBase> CreateHttpContext(HttpResponseBase httpResponseStub, HttpRequestBase httpRequestStub)
        {
            Mock<HttpContextBase> httpContextBaseMock = new Mock<HttpContextBase>();
            httpContextBaseMock.SetupGet(x => x.Response).Returns(() => httpResponseStub);
            httpContextBaseMock.SetupGet(x => x.Request).Returns(() => httpRequestStub);
            return httpContextBaseMock;
        }
    }
}

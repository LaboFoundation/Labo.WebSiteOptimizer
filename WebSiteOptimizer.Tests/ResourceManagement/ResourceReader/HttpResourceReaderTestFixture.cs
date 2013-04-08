using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.SelfHost;

using Labo.WebSiteOptimizer.Extensions;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Labo.WebSiteOptimizer.ResourceManagement.VirtualPath;

using Moq;

using NUnit.Framework;

namespace Labo.WebSiteOptimizer.Tests.ResourceManagement.ResourceReader
{
    [TestFixture]
    public class HttpResourceReaderTestFixture
    {
        private sealed class ContentHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                {
                    string baseFolder = GetContentsFolder();
                    string suffix = request.RequestUri.AbsolutePath.Substring(1);
                    string fullPath = Path.Combine(baseFolder, suffix);
                    HttpResponseMessage httpResponseMessage = request.CreateResponse();
                    if (!File.Exists(fullPath))
                    {
                        httpResponseMessage.StatusCode = HttpStatusCode.NotFound;
                        return httpResponseMessage;
                    }
                    httpResponseMessage.Content = new StreamContent(new FileStream(fullPath, FileMode.Open));
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/javascript");
                    httpResponseMessage.Content.Headers.LastModified = File.GetLastWriteTime(fullPath);
                    return httpResponseMessage;
                });
            }
        }

        private HttpSelfHostServer m_SelfHostServer;
        private const string BASE_ADDRESS = "http://127.0.0.1:8552";

        [SetUp]
        public void Setup()
        {
            HttpSelfHostConfiguration httpSelfHostConfiguration = new HttpSelfHostConfiguration(BASE_ADDRESS);
            httpSelfHostConfiguration.MessageHandlers.Add(new ContentHandler());
            m_SelfHostServer = new HttpSelfHostServer(httpSelfHostConfiguration);
            Task task = m_SelfHostServer.OpenAsync();
            task.Wait();
        }

        [TearDown]
        public void TearDown()
        {
            if (m_SelfHostServer != null)
            {
                m_SelfHostServer.CloseAsync().Wait();
                m_SelfHostServer.Dispose();
            }
        }

        [Test]
        public void GetLastModifiedDate()
        {
            const string content = "JavaScript1.js";
            string remoteFileAddress = "{0}/{1}".FormatWith(BASE_ADDRESS, content);
            DateTime lastModifiedDate = HttpResourceReader.GetLastModifiedDate(remoteFileAddress);
            DateTime expectedLastModifiedDate = GetLastWriteTime(content);
            AssertLastWriteTime(expectedLastModifiedDate, lastModifiedDate);
        }

        [Test]
        public void GetPhysicalFile()
        {
            Mock<IRemoteFileTempFolderProvider> remoteFileTempFolderProviderMock = new Mock<IRemoteFileTempFolderProvider>();
            
            const string content = "JavaScript1.js";
            string remoteFileAddress = "{0}/{1}".FormatWith(BASE_ADDRESS, content);
            string tempFile = Path.Combine(GetContentsFolder(), "JavaScript1.temp.js");
            remoteFileTempFolderProviderMock.Setup(x => x.GetTempFilePath(remoteFileAddress)).Returns(tempFile);

            HttpResourceReader httpResourceReader = new HttpResourceReader(remoteFileTempFolderProviderMock.Object, null);
            string fileName;
            string fileContent;
            httpResourceReader.GetPhysicalFile(remoteFileAddress, out fileName, out fileContent);

            Assert.IsTrue(File.Exists(fileName));
            Assert.AreEqual(tempFile, fileName);
            Assert.AreEqual(File.ReadAllText(Path.Combine(GetContentsFolder(), content)), fileContent);
            Assert.AreEqual(File.ReadAllText(tempFile), fileContent);
        }

        [Test]
        public void GetPhysicalFile_ThrowsExceptionWhenPathIsNull()
        {
            HttpResourceReader httpResourceReader = new HttpResourceReader(null, null);
            string fileName;
            string fileContent;
            Assert.Throws<ArgumentNullException>(() => httpResourceReader.GetPhysicalFile(null, out fileName, out fileContent));
        }

        [Test]
        public void GetPhysicalFile_ThrowsExceptionWhenNotFoundRemoteFile()
        {
            const string content = "JavaScript2.js";
            string remoteFileAddress = "{0}/{1}".FormatWith(BASE_ADDRESS, content);
            HttpResourceReader httpResourceReader = new HttpResourceReader(null, null);
            string fileName;
            string fileContent;
            Assert.Throws<HttpResourceReaderException>(() => httpResourceReader.GetPhysicalFile(remoteFileAddress, out fileName, out fileContent));
        }

        [Test]
        [TestCase("http://www.xyz.com/xyz.js", "http://www.abc.com/home/index", "http://www.xyz.com/xyz.js")]
        [TestCase("https://www.abc.com/abc.js", "http://www.abc.com/home/index", "https://www.abc.com/abc.js")]
        [TestCase("~/abc.js", "http://www.abc.com/home/index", "http://www.abc.com/abc.js")]
        [TestCase("~/Content/abc.js", "http://www.abc.com/home/index", "http://www.abc.com/Content/abc.js")]
        [TestCase("/abc.js", "http://www.abc.com/home/index", "http://www.abc.com/abc.js")]
        [TestCase("/Content/abc.js", "http://www.abc.com/home/index", "http://www.abc.com/Content/abc.js")]
        [TestCase("abc.js", "http://www.abc.com/home/index", "http://www.abc.com/abc.js")]
        [TestCase("Content/abc.js", "http://www.abc.com/home/index", "http://www.abc.com/Content/abc.js")]
        public void GetAbsoluteUrl(string relativeUrl, string currentUrl, string expectedUrl)
        {
            Mock<HttpContextBase> mock = new Mock<HttpContextBase>();
            mock.Setup(x => x.Request.Url).Returns(new Uri(currentUrl));
            Utility.HttpContextWrapper.Context = mock.Object;

            Mock<IVirtualPathProvider> mockVirtualPathProvider = new Mock<IVirtualPathProvider>();
            string absoluteUrl = relativeUrl.Replace("~", string.Empty);
            if (!absoluteUrl.StartsWith("/"))
            {
                absoluteUrl = absoluteUrl.Insert(0, "/");
            }
            mockVirtualPathProvider.Setup(x => x.ToAbsoluteUrl(It.IsAny<string>())).Returns(absoluteUrl);

            HttpResourceReader httpResourceReader = new HttpResourceReader(null, mockVirtualPathProvider.Object);
            Assert.AreEqual(expectedUrl, httpResourceReader.GetAbsoluteUrl(relativeUrl));
        }

        [Test]
        public void ReadResource()
        {
            Mock<IRemoteFileTempFolderProvider> remoteFileTempFolderProviderMock = new Mock<IRemoteFileTempFolderProvider>();

            const string content = "JavaScript1.js";
            string remoteFileAddress = "{0}/{1}".FormatWith(BASE_ADDRESS, content);
            string contentPath = Path.Combine(GetContentsFolder(), content);
            string tempFile = Path.Combine(GetContentsFolder(), "JavaScript2.temp.js");
            remoteFileTempFolderProviderMock.Setup(x => x.GetTempFilePath(remoteFileAddress)).Returns(tempFile);

            HttpResourceReader httpResourceReader = new HttpResourceReader(remoteFileTempFolderProviderMock.Object, null);
            ResourceInfo resourceInfo = httpResourceReader.ReadResource(remoteFileAddress);

            Assert.IsTrue(File.Exists(resourceInfo.DependentFile));
            Assert.AreEqual(tempFile, resourceInfo.DependentFile);
            Assert.AreEqual(File.ReadAllText(contentPath), resourceInfo.Content);
            Assert.AreEqual(File.ReadAllText(tempFile), resourceInfo.Content);
            AssertLastWriteTime(File.GetLastWriteTime(contentPath), resourceInfo.ModifyDate);
        }

        private static void AssertLastWriteTime(DateTime expectedLastModifiedDate, DateTime lastModifiedDate)
        {
            Assert.AreEqual(
                new DateTime(expectedLastModifiedDate.Year, expectedLastModifiedDate.Month, expectedLastModifiedDate.Day,
                             expectedLastModifiedDate.Hour, expectedLastModifiedDate.Minute, expectedLastModifiedDate.Second),
                new DateTime(lastModifiedDate.Year, lastModifiedDate.Month, lastModifiedDate.Day, lastModifiedDate.Hour,
                             lastModifiedDate.Minute, lastModifiedDate.Second));
        }

        private static DateTime GetLastWriteTime(string content)
        {
            return File.GetLastWriteTime(Path.Combine(GetContentsFolder(), content));
        }

        private static string GetContentsFolder()
        {
            string assemblyLocation = Path.Combine(Environment.CurrentDirectory, Path.GetDirectoryName(Path.GetFileName(Assembly.GetAssembly(typeof(HttpResourceReaderTestFixture)).CodeBase)));
            return Path.Combine(assemblyLocation, "ResourceManagement\\_TestData");
        }
    }
}
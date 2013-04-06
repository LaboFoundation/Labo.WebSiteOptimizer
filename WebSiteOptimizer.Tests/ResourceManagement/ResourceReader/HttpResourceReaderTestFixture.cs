using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;

using Labo.WebSiteOptimizer.Extensions;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;

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
                m_SelfHostServer.Dispose();
            }
        }

        [Test]
        public void GetLastModifiedDate()
        {
            const string content = "JavaScript1.js";
            DateTime lastModifiedDate = HttpResourceReader.GetLastModifiedDate("{0}/{1}".FormatWith(BASE_ADDRESS, content));
            DateTime expectedLastModifiedDate = GetLastWriteTime(content);
            Assert.AreEqual(new DateTime(expectedLastModifiedDate.Year, expectedLastModifiedDate.Month, expectedLastModifiedDate.Day, expectedLastModifiedDate.Hour, expectedLastModifiedDate.Minute, expectedLastModifiedDate.Second),
                            new DateTime(lastModifiedDate.Year, lastModifiedDate.Month, lastModifiedDate.Day, lastModifiedDate.Hour, lastModifiedDate.Minute, lastModifiedDate.Second));
        }

        private static DateTime GetLastWriteTime(string content)
        {
            return File.GetLastWriteTime(Path.Combine(GetContentsFolder(), content));
        }

        private static string GetContentsFolder()
        {
            string assemblyLocation = Path.Combine(Environment.CurrentDirectory, Path.GetDirectoryName(Path.GetFileName(Assembly.GetAssembly(typeof(HttpResourceReaderTestFixture)).CodeBase)));
            string baseFolder = Path.Combine(assemblyLocation, "ResourceManagement\\_TestData");
            return baseFolder;
        }
    }
}

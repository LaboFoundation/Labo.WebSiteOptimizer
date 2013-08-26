using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using Labo.WebSiteOptimizer.Extensions;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.ResourceManagement.VirtualPath;
using HttpContextWrapper = Labo.WebSiteOptimizer.Utility.HttpContextWrapper;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal sealed class HttpResourceReader : IResourceReader
    {
        private readonly IRemoteFileTempFolderProvider m_RemoteFileTempFolderProvider;
        private readonly IVirtualPathProvider m_VirtualPathProvider;

        public HttpResourceReader(IRemoteFileTempFolderProvider remoteFileTempFolderProvider, IVirtualPathProvider virtualPathProvider)
        {
            m_RemoteFileTempFolderProvider = remoteFileTempFolderProvider;
            m_VirtualPathProvider = virtualPathProvider;
        }

        public ResourceInfo ReadResource(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            string fileUrl = GetAbsoluteUrl(path);
            string filePath, content;

            GetPhysicalFile(fileUrl, out filePath, out content);

            return new ResourceInfo
                       {
                           Content = content,
                           DependentFile = filePath,
                           ModifyDate = GetLastModifiedDate(fileUrl)
                       };
        }

        internal string GetAbsoluteUrl(string relativeUrl)
        {
            if (relativeUrl == null)
            {
                throw new ArgumentNullException("relativeUrl");
            }

            if (relativeUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || relativeUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return relativeUrl;
            }

            HttpContextBase context = HttpContextWrapper.Context;
            if (context == null)
            {
                throw new InvalidOperationException("Cannot resolve url '{0}' because HttpContext is null.".FormatWith(relativeUrl));
            }

            if (relativeUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                relativeUrl = relativeUrl.Insert(0, "~");
            }

            Page page = context.Handler as Page;
            if (page != null)
            {
                relativeUrl = page.ResolveUrl(relativeUrl);
            }
            else
            {
                if (!relativeUrl.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    relativeUrl = relativeUrl.Insert(0, "~/");
                }

                relativeUrl = m_VirtualPathProvider.ToAbsoluteUrl(relativeUrl);
            }

            Uri url = context.Request.Url;
            string port = url.Port != 80 ? (":" + url.Port) : string.Empty;

            //return absolute url
            return string.Format(CultureInfo.CurrentCulture, "{0}://{1}{2}{3}", url.Scheme, url.Host, port, relativeUrl);
        }

        internal void GetPhysicalFile(string path, out string fileName, out string fileContent)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            HttpWebResponse webResponse = null;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(path);
                webResponse = (HttpWebResponse)webRequest.GetResponse();

                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    fileContent = streamReader.ReadToEnd();
                }

                fileName = m_RemoteFileTempFolderProvider.GetTempFilePath(path);

                using (StreamWriter streamWriter = new StreamWriter(fileName))
                {
                    streamWriter.Write(fileContent);
                }
            }
            catch(Exception ex)
            {
                throw new HttpResourceReaderException("Error occured while downloading resource '{0}'".FormatWith(path));
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }            
        }

        internal static DateTime GetLastModifiedDate(string path)
        {
            HttpWebResponse webResponse = null;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(path);
                webRequest.Method = "HEAD";
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse.LastModified;
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
        }
    }
}

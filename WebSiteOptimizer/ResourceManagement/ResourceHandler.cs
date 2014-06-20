using System.Globalization;
using System.Web;
using System;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.Extensions;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class ResourceHandler : IResourceHandler
    {
        private readonly IResourceProcessor m_ResourceProcessor;
        private readonly IResourceConfigurationProvider m_WebResourceConfiguration;
        private readonly IHttpResponseCacher m_HttpResponseCacher;
        private readonly IHttpResponseCompressor m_HttpResponseCompressor;

        public ResourceHandler(IResourceProcessor resourceProcessor, IResourceConfigurationProvider webResourceConfiguration, IHttpResponseCacher httpResponseCacher, IHttpResponseCompressor httpResponseCompressor)
        {
            if (resourceProcessor == null)
            {
                throw new ArgumentNullException("resourceProcessor");
            }
            if (webResourceConfiguration == null)
            {
                throw new ArgumentNullException("webResourceConfiguration");
            }
            if (httpResponseCacher == null)
            {
                throw new ArgumentNullException("httpResponseCacher");
            }
            if (httpResponseCompressor == null)
            {
                throw new ArgumentNullException("httpResponseCompressor");
            }
            m_ResourceProcessor = resourceProcessor;
            m_WebResourceConfiguration = webResourceConfiguration;
            m_HttpResponseCacher = httpResponseCacher;
            m_HttpResponseCompressor = httpResponseCompressor;
        }

        public void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName)
        {
            CompressionType compressionType;
            ProcessedResourceGroupInfo resourceInfo = ProcessResource(httpContext, resourceType, resourceGroupName, out compressionType);

            HandleResource(httpContext, resourceType, compressionType, resourceInfo);
        }

        public void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string fileName, bool isEmbeddedResource, bool isHttpResponse, bool minify, bool compress)
        {
            Utility.HttpContextWrapper.Context = httpContext;

            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            CompressionType compressionType = GetRequestCompressionType(httpContext, compress);
            ProcessedResourceInfo resourceInfo = m_ResourceProcessor.ProcessResource(resourceType, fileName, isEmbeddedResource, isHttpResponse, minify, compressionType);

            HandleResource(httpContext, resourceType, compressionType, resourceInfo);
        }

        public void HandleResource(HttpContextBase httpContextBase, ResourceType resourceType, string resourceGroupName, string fileName, bool minify, bool compress)
        {
            ResourceElementGroup resourceElementGroup = m_WebResourceConfiguration.GetResourceElementGroup(resourceType, resourceGroupName);
            ResourceElement resourceElement = resourceElementGroup.GetResourceElementByFileName(fileName);
            HandleResource(httpContextBase, resourceType, resourceElement.FileName, resourceElement.IsEmbeddedResource, resourceElement.IsHttpResource, minify, compress);
        }

        public void HandleResource(HttpContextBase httpContext, ResourceType resourceType, CompressionType compressionType, IProcessedResourceContentInfo resourceInfo)
        {
            SetContentTypeHeader(resourceType, httpContext);
            m_HttpResponseCompressor.Compress(httpContext, compressionType);
            m_HttpResponseCacher.Cache(httpContext, resourceInfo.LastModifyDate);
            SetResponseCharset(httpContext);

            httpContext.Response.BinaryWrite(resourceInfo.Content);
        }

        public ProcessedResourceGroupInfo ProcessResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName, out CompressionType compressionType)
        {
            Utility.HttpContextWrapper.Context = httpContext;

            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            if (resourceGroupName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("resourceGroupName");
            }

            ResourceElementGroup resourceElementGroup = m_WebResourceConfiguration.GetResourceElementGroup(resourceType, resourceGroupName);
            compressionType = GetRequestCompressionType(httpContext, resourceElementGroup.Compress);
            ProcessedResourceGroupInfo resourceInfo = m_ResourceProcessor.ProcessResource(resourceElementGroup, compressionType);
            return resourceInfo;
        }

        private CompressionType GetRequestCompressionType(HttpContextBase httpContext, bool compress)
        {
            return compress ? m_HttpResponseCompressor.GetRequestCompressionType(httpContext) : CompressionType.None;
        }

        private static void SetContentTypeHeader(ResourceType resourceType, HttpContextBase context)
        {
            context.Response.ContentType = GetContentType(resourceType);
        }

        private static string GetContentType(ResourceType resourceType)
        {
            string contentType;

            switch (resourceType)
            {
                case ResourceType.Css:
                    contentType = "text/css";
                    break;
                case ResourceType.Js:
                    contentType = "text/javascript";
                    break;
                default:
                    throw new ResourceHandlerException(string.Format(CultureInfo.CurrentCulture, "resource type '{0}' not supported", resourceType));
            }
            return contentType;
        }

        private static void SetResponseCharset(HttpContextBase context)
        {
            context.Response.Charset = "utf-8";
        }
    }
}
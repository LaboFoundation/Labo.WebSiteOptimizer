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
        private readonly IWebResourceConfigurationProvider m_WebResourceConfiguration;
        private readonly IDateTimeProvider m_DateTimeProvider;

        public ResourceHandler(IResourceProcessor resourceProcessor, IWebResourceConfigurationProvider webResourceConfiguration, IDateTimeProvider dateTimeProvider)
        {
            if (resourceProcessor == null)
            {
                throw new ArgumentNullException("resourceProcessor");
            }
            if (webResourceConfiguration == null)
            {
                throw new ArgumentNullException("webResourceConfiguration");
            }
            if (dateTimeProvider == null)
            {
                throw new ArgumentNullException("dateTimeProvider");
            }
            m_ResourceProcessor = resourceProcessor;
            m_WebResourceConfiguration = webResourceConfiguration;
            m_DateTimeProvider = dateTimeProvider;
        }

        public void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName)
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
            CompressionType compressionType = ClientCompressionHelper.GetCompressionType(httpContext, resourceElementGroup);
            ProcessedResourceGroupInfo resourceInfo = m_ResourceProcessor.ProcessResource(resourceElementGroup, compressionType);

            SetContentTypeHeader(resourceType, httpContext);
            SetContentEncodingHeader(httpContext, compressionType);
            SetCachingHeaders(resourceInfo, httpContext);
            SetResponseCharset(httpContext);

            httpContext.Response.BinaryWrite(resourceInfo.Content);
        }

        public void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string fileName, bool isEmbeddedResource, bool minify, bool compress)
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
            CompressionType compressionType = ClientCompressionHelper.GetCompressionType(httpContext, compress);
            ProcessedResourceInfo resourceInfo = m_ResourceProcessor.ProcessResource(resourceType, fileName, isEmbeddedResource, minify, compressionType);

            SetContentTypeHeader(resourceType, httpContext);
            SetContentEncodingHeader(httpContext, compressionType);
            SetCachingHeaders(resourceInfo.LastModifyDate, httpContext);
            SetResponseCharset(httpContext);

            httpContext.Response.BinaryWrite(resourceInfo.Content);
        }

        public void HandleResource(HttpContextBase httpContextBase, ResourceType resourceType, string resourceGroupName, string fileName, bool minify, bool compress)
        {
            ResourceElementGroup resourceElementGroup = m_WebResourceConfiguration.GetResourceElementGroup(resourceType, resourceGroupName);
            ResourceElement resourceElement = resourceElementGroup.GetResourceElementByFileName(fileName);
            HandleResource(httpContextBase, resourceType, resourceElement.FileName, resourceElement.IsEmbeddedResource, minify, compress);
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
                    throw new ResourceHandlerException(String.Format(CultureInfo.CurrentCulture, "resource type '{0}' not supported", resourceType));
            }
            return contentType;
        }

        private static void SetContentEncodingHeader(HttpContextBase context, CompressionType compressionType)
        {
            switch (compressionType)
            {
                case CompressionType.Deflate:
                    context.Response.AppendHeader("Content-Encoding", "deflate");
                    break;
                case CompressionType.Gzip:
                    context.Response.AppendHeader("Content-Encoding", "gzip");
                    break;
            }
        }

        private static void SetResponseCharset(HttpContextBase context)
        {
            context.Response.Charset = "utf-8";
        }

        private void SetCachingHeaders(ProcessedResourceGroupInfo resourceGroupInfo, HttpContextBase context)
        {
            SetCachingHeaders(resourceGroupInfo.LastModifyDate, context);
        }

        private void SetCachingHeaders(DateTime lastModifyDate, HttpContextBase context)
        {
            HttpCachePolicyBase cache = context.Response.Cache;

            cache.SetLastModified(lastModifyDate);
            cache.SetVaryByCustom("Accept-Encoding");
            cache.SetValidUntilExpires(true);
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(m_DateTimeProvider.UtcNow.AddYears(1));
            cache.SetMaxAge(TimeSpan.FromDays(365));
        }
    }
}
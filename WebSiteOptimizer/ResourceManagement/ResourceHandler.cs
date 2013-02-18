using System.Globalization;
using System.Web;
using System;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.Utility;

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
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (resourceGroupName.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("resourceGroupName");
            }

            ResourceElementGroup resourceElementGroup = m_WebResourceConfiguration.GetResourceElementGroup(resourceType, resourceGroupName);
            ProcessedResourceGroupInfo resourceInfo = m_ResourceProcessor.ProcessResource(httpContext, resourceElementGroup);

            SetContentTypeHeader(resourceType, httpContext);
            SetContentEncodingHeader(resourceElementGroup, httpContext);
            SetCachingHeaders(resourceInfo, httpContext);

            httpContext.Response.BinaryWrite(resourceInfo.Content);
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

        private static void SetContentEncodingHeader(ResourceElementGroup resourceInfo, HttpContextBase context)
        {
            if (resourceInfo == null)
            {
                throw new ArgumentNullException("resourceInfo");
            }
            if (resourceInfo.Compress)
            {
                CompressionType compressionType = HttpRequestUtils.GetRequestCompressionType(context.Request);
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
            context.Response.Charset = "utf-8";
        }

        private void SetCachingHeaders(ProcessedResourceGroupInfo resourceGroupInfo, HttpContextBase context)
        {
            HttpCachePolicyBase cache = context.Response.Cache;

            cache.SetLastModified(resourceGroupInfo.LastModifyDate);
            cache.SetVaryByCustom("Accept-Encoding");
            cache.SetValidUntilExpires(true);
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(m_DateTimeProvider.UtcNow.AddYears(1));
            cache.SetMaxAge(TimeSpan.FromDays(365));
        }
    }
}
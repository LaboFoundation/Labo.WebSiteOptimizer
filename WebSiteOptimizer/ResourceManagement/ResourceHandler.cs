using System.Globalization;
using System.Text;
using System.Web;
using System;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement.Cacher;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.ResourceManagement.Hasher;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Labo.WebSiteOptimizer.Utility;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class ResourceHandler : IResourceHandler
    {
        private readonly IResourceCacher m_ResourceCacher;
        private readonly IResourceReader m_ResourceReader;
        private readonly ICompressionFactory m_CompressionFactory;
        private readonly IResourceHasher m_ResourceHasher;

        public ResourceHandler(IResourceCacher resourceCacher, IResourceReader resourceReader, ICompressionFactory compressionFactory, IResourceHasher resourceHasher)
        {
            m_ResourceCacher = resourceCacher;
            m_ResourceReader = resourceReader;
            m_CompressionFactory = compressionFactory;
            m_ResourceHasher = resourceHasher;
        }

        public ResourceGroupInfo ProcessResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName)
        {
            ResourceElementGroup resourceElementGroup = GetResourceElementGroup(resourceType, resourceGroupName);

            return ProcessResource(httpContext, resourceType, resourceGroupName, resourceElementGroup);
        }

        private ResourceGroupInfo ProcessResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName, ResourceElementGroup resourceElementGroup)
        {
            CompressionType compressionType = CompressionType.None;
            if (resourceElementGroup.Compress)
            {
                compressionType = HttpRequestUtils.GetRequestCompressionType(httpContext.Request);
            }
            int cacheDuration = resourceElementGroup.CacheDuration > 0 ? resourceElementGroup.CacheDuration : 60;
            return m_ResourceCacher.GetOrAddCachedResource(resourceType, resourceGroupName, compressionType,
                                                           () =>
                                                           ProcessResource(compressionType, resourceElementGroup, resourceType),
                                                           TimeSpan.FromMinutes(cacheDuration));
        }

        private static ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName)
        {
            ResourceElementGroup resourceElementGroup = null;
            if (resourceType == ResourceType.Js)
            {
                resourceElementGroup = WebResourceConfiguration.Instance.WebResources.JavascriptResources.GetResourceGroupByName(resourceGroupName);
            }
            else if (resourceType == ResourceType.Css)
            {
                resourceElementGroup = WebResourceConfiguration.Instance.WebResources.CssResources.GetResourceGroupByName(resourceGroupName);
            }
            return resourceElementGroup;
        }

        private ResourceGroupInfo ProcessResource(CompressionType compressionType, ResourceElementGroup resourceElementGroup, ResourceType resourceType)
        {
            ResourceGroupInfo resourceGroupInfo = new ResourceGroupInfo();
            StringBuilder stringBuilder = new StringBuilder();
            ResourceElementCollection resources = resourceElementGroup.Resources;
            for (int i = 0; i < resources.Count; i++)
            {
                ResourceElement resourceElement = resources[i];
                ResourceInfo resourceInfo = m_ResourceReader.ReadResource(resourceElement.FileName);
                resourceGroupInfo.DependentFiles.Add(resourceInfo.DependentFile);
                if(resourceInfo.ModifyDate > resourceGroupInfo.LastModifyDate)
                {
                    resourceGroupInfo.LastModifyDate = resourceInfo.ModifyDate;
                }

                string content = resourceInfo.Content;
                if ((resourceElement.Minify && resourceElement.Minify) || 
                    (!resourceElement.Minify && resourceElementGroup.Minify))
                {
                    if (resourceType == ResourceType.Js)
                    {
                        content = MinifyManager.JsMinifier.Minify(content, true);
                    }
                    else if (resourceType == ResourceType.Css)
                    {
                        content = MinifyManager.CssMinifier.Minify(content);                        
                    }
                }
                stringBuilder.Append(content);
            }

            string combinedContent = stringBuilder.ToString();
            resourceGroupInfo.Hash = m_ResourceHasher.HashContent(combinedContent);
            switch (compressionType)
            {
                case CompressionType.Gzip:
                    resourceGroupInfo.Content = m_CompressionFactory.CreateCompressor(CompressionType.Gzip).Compress(Encoding.UTF8.GetBytes(combinedContent));
                    break;
                case CompressionType.Deflate:
                    resourceGroupInfo.Content = m_CompressionFactory.CreateCompressor(CompressionType.Deflate).Compress(Encoding.UTF8.GetBytes(combinedContent));
                    break;
                default:
                    resourceGroupInfo.Content = Encoding.UTF8.GetBytes(combinedContent);
                    break;
            }
            return resourceGroupInfo;
        }

        public void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName)
        {
            ResourceElementGroup resourceElementGroup = GetResourceElementGroup(resourceType, resourceGroupName);
            ResourceGroupInfo resourceInfo = ProcessResource(httpContext, resourceType, resourceGroupName, resourceElementGroup);

            SetContentEncodingHeader(resourceElementGroup, httpContext);
            SetCachingHeaders(resourceInfo, httpContext);
            SetContentTypeHeader(resourceType, httpContext);

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
                    throw new ResourceManagerException(String.Format(CultureInfo.CurrentCulture, "resource type '{0}' not supported", resourceType));
            }
            return contentType;
        }

        private static void SetContentEncodingHeader(ResourceElementGroup resourceInfo, HttpContextBase context)
        {
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

        private static void SetCachingHeaders(ResourceGroupInfo resourceGroupInfo, HttpContextBase context)
        {
            HttpCachePolicyBase cache = context.Response.Cache;

            cache.SetLastModified(resourceGroupInfo.LastModifyDate);
            cache.SetVaryByCustom("Accept-Encoding");
            cache.SetValidUntilExpires(true);
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.UtcNow.AddYears(1));
            cache.SetMaxAge(TimeSpan.FromDays(365));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.Extensions;
using Labo.WebSiteOptimizer.ResourceManagement.Cacher;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.ResourceManagement.Hasher;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Labo.WebSiteOptimizer.Utility;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class ResourceProcessor : IResourceProcessor
    {
        private readonly IResourceCacher m_ResourceCacher;
        private readonly IResourceReaderManager m_ResourceReader;
        private readonly ICompressionFactory m_CompressionFactory;
        private readonly IResourceHasher m_ResourceHasher;
        private readonly IJsMinifier m_JsMinifier;
        private readonly ICssMinifier m_CssMinifier;

        public ResourceProcessor(IResourceCacher resourceCacher, IResourceReaderManager resourceReader, ICompressionFactory compressionFactory, IResourceHasher resourceHasher, IJsMinifier jsMinifier, ICssMinifier cssMinifier)
        {
            m_ResourceCacher = resourceCacher;
            m_ResourceReader = resourceReader;
            m_CompressionFactory = compressionFactory;
            m_ResourceHasher = resourceHasher;
            m_JsMinifier = jsMinifier;
            m_CssMinifier = cssMinifier;
        }

        public ProcessedResourceGroupInfo ProcessResource(ResourceElementGroup resourceElementGroup, CompressionType compressionType)
        {
            int cacheDuration = resourceElementGroup.CacheDuration > 0 ? resourceElementGroup.CacheDuration : 60;
            return m_ResourceCacher.GetOrAddCachedResource(resourceElementGroup.ResourceType, resourceElementGroup.Name, compressionType,
                                                           () => ProcessResource(compressionType, resourceElementGroup, resourceElementGroup.ResourceType),
                                                           TimeSpan.FromMinutes(cacheDuration));
        }

        internal ProcessedResourceGroupInfo ProcessResource(CompressionType compressionType, ResourceElementGroup resourceElementGroup, ResourceType resourceType)
        {
            ProcessedResourceGroupInfo resourceGroupInfo = new ProcessedResourceGroupInfo();
            IList<ResourceReadInfo> resources = ReadResources(resourceElementGroup.Resources);

            string combinedContent = MinifyAndCombineResources(resourceElementGroup, resourceType, resources);
            resourceGroupInfo.Hash = m_ResourceHasher.HashContent(combinedContent);
            resourceGroupInfo.DependentFiles = CalculateDependentFiles(resources);
            resourceGroupInfo.LastModifyDate = CalculateLastModifyDate(resources);
            resourceGroupInfo.Content = CompressContent(compressionType, combinedContent);
            return resourceGroupInfo;
        }

        public ProcessedResourceInfo ProcessResource(ResourceType resourceType, string fileName, bool isEmbeddedResource, bool minify, CompressionType compressionType)
        {
            ProcessedResourceInfo processedResourceInfo = new ProcessedResourceInfo();
            ResourceInfo resourceInfo = ReadResource(fileName, isEmbeddedResource);

            string content = minify ? MinifyContent(resourceType, resourceInfo.Content) : resourceInfo.Content;
            processedResourceInfo.Hash = m_ResourceHasher.HashContent(content);
            processedResourceInfo.DependentFile = resourceInfo.DependentFile;
            processedResourceInfo.LastModifyDate = resourceInfo.ModifyDate;
            processedResourceInfo.Content = CompressContent(compressionType, content);
            return processedResourceInfo;
        }

        internal static DateTime CalculateLastModifyDate(IList<ResourceReadInfo> resources)
        {
            if (resources.Count == 0)
            {
                throw new ResourceProcessorException("Resource count must be bigger than 0");
            }
            return resources.Select(x => x.ResourceInfo.ModifyDate).Max();
        }

        internal static HashSet<string> CalculateDependentFiles(IEnumerable<ResourceReadInfo> resources)
        {
            return new HashSet<string>(resources.Select(x => x.ResourceInfo.DependentFile).Distinct());
        }

        internal IList<ResourceReadInfo> ReadResources(ResourceElementCollection resources)
        {
            List<ResourceReadInfo> resourceReadInfos = new List<ResourceReadInfo>(resources.Count);
            for (int i = 0; i < resources.Count; i++)
            {
                ResourceElement resourceElement = resources[i];
                ResourceInfo resourceInfo = ReadResource(resourceElement);
                resourceReadInfos.Add(new ResourceReadInfo
                    {
                        ResourceElement = resourceElement,
                        ResourceInfo = resourceInfo
                    });
            }
            return resourceReadInfos;
        }

        private ResourceInfo ReadResource(ResourceElement resourceElement)
        {
            return ReadResource(resourceElement.FileName, resourceElement.IsEmbeddedResource);
        }

        private ResourceInfo ReadResource(string fileName, bool isEmbeddedResource)
        {
            return m_ResourceReader.ReadResource(new ResourceReadOptions { FileName = fileName, IsEmbeddedResource = isEmbeddedResource });
        }

        internal string MinifyAndCombineResources(IResourceElementGroupConfiguration resourceElementGroupConfiguration, ResourceType resourceType, IList<ResourceReadInfo> resources)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < resources.Count; i++)
            {
                ResourceReadInfo resource = resources[i];
                string content = MustMinify(resourceElementGroupConfiguration, resource.ResourceElement) ? MinifyContent(resourceType, resource.ResourceInfo.Content) : resource.ResourceInfo.Content;
                stringBuilder.Append(content);
            }

            return stringBuilder.ToString();
        }

        internal byte[] CompressContent(CompressionType compressionType, string content)
        {
            switch (compressionType)
            {
                case CompressionType.Gzip:
                    return  m_CompressionFactory.CreateCompressor(CompressionType.Gzip).Compress(Encoding.UTF8.GetBytes(content));
                case CompressionType.Deflate:
                    return m_CompressionFactory.CreateCompressor(CompressionType.Deflate).Compress(Encoding.UTF8.GetBytes(content));
                default:
                    return Encoding.UTF8.GetBytes(content);
            }
        }

        internal static bool MustMinify(IResourceElementGroupConfiguration resourceElementGroupConfiguration, ResourceElement resourceElement)
        {
            return (resourceElement.Minify.HasValue && resourceElement.Minify.Value) ||
                   (!resourceElement.Minify.HasValue && resourceElementGroupConfiguration.Minify);
        }

        internal string MinifyContent(ResourceType resourceType, string content)
        {
            switch (resourceType)
            {
                case ResourceType.Js:
                    content = m_JsMinifier.Minify(content, true);
                    break;
                case ResourceType.Css:
                    content = m_CssMinifier.Minify(content);
                    break;
                default:
                    throw new ResourceProcessorException("Unsupported resource type '{0}'".FormatWith(resourceType));
            }
            return content;
        }
    }
}
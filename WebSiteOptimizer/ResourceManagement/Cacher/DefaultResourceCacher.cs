using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Labo.WebSiteOptimizer.Caching;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.Extensions;

namespace Labo.WebSiteOptimizer.ResourceManagement.Cacher
{
    internal sealed class DefaultResourceCacher : IResourceCacher
    {
        private readonly ICacheProvider m_CacheProvider;
        private readonly HashSet<string> m_DependentFiles = new HashSet<string>(); 
        private static readonly object s_LockObject = new object();

        public DefaultResourceCacher(ICacheProvider cacheProvider)
        {
            m_CacheProvider = cacheProvider;
        }

        public ProcessedResourceGroupInfo GetOrAddCachedResource(ResourceType resourceType, string resourceGroupName, CompressionType compressionType, Func<ProcessedResourceGroupInfo> funcContent, TimeSpan expiration)
        {
            return GetOrAddByResourceCache(GetCacheKey(resourceType, resourceGroupName, compressionType), funcContent, expiration);
        }

        private ProcessedResourceGroupInfo GetOrAddByResourceCache(string key, Func<ProcessedResourceGroupInfo> func, TimeSpan expiration)
        {
            object data = m_CacheProvider.Get(key);
            if (data == null)
            {
                lock (s_LockObject)
                {
                    ProcessedResourceGroupInfo resourceGroupInfo = func();
                    if (resourceGroupInfo != null)
                    {
                        List<string> dependentFiles = resourceGroupInfo.DependentFiles.Union(m_DependentFiles).ToList();
                        m_CacheProvider.Set(key, resourceGroupInfo, expiration, dependentFiles);
                        return resourceGroupInfo;
                    }
                }
            }
            return (ProcessedResourceGroupInfo)data;
        }

        private static string GetCacheKey(ResourceType resourceType, string resourceGroupName, CompressionType compressionType)
        {
            return "ResourceGroupCache:t={0},g={1},c={2}".FormatWith(CultureInfo.InvariantCulture, resourceType, resourceGroupName, compressionType);
        }

        public void AddDependentFile(string dependentFile)
        {
            m_DependentFiles.Add(dependentFile);
        }
    }
}
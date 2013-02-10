using System;
using System.Linq;
using System.Globalization;
using Labo.WebSiteOptimizer.Caching;
using Labo.WebSiteOptimizer.Compression;

namespace Labo.WebSiteOptimizer.ResourceManagement.Cacher
{
    internal sealed class ResourceCacher : IResourceCacher
    {
        private readonly ICacheProvider m_CacheProvider;
        private static readonly object s_LockObject = new object();

        public ResourceCacher(ICacheProvider cacheProvider)
        {
            m_CacheProvider = cacheProvider;
        }

        public ResourceGroupInfo GetOrAddCachedResource(ResourceType resourceType, string resourceGroupName, CompressionType compressionType, Func<ResourceGroupInfo> funcContent, TimeSpan expiration)
        {
            return GetOrAddByResourceCache(GetCacheKey(resourceType, resourceGroupName, compressionType), funcContent, expiration);
        }

        private ResourceGroupInfo GetOrAddByResourceCache(string key, Func<ResourceGroupInfo> func, TimeSpan expiration)
        {
            object data = m_CacheProvider.Get(key);
            if (data == null)
            {
                lock (s_LockObject)
                {
                    ResourceGroupInfo resourceGroupInfo = func();
                    if (resourceGroupInfo != null)
                    {
                        m_CacheProvider.Set(key, resourceGroupInfo, expiration, resourceGroupInfo.DependentFiles.ToList());
                        return resourceGroupInfo;
                    }
                }
            }
            return (ResourceGroupInfo)data;
        }

        private static string GetCacheKey(ResourceType resourceType, string resourceGroupName, CompressionType compressionType)
        {
            return "ResourceCache:t={0},g={1},c={2}".FormatWith(CultureInfo.InvariantCulture, resourceType, resourceGroupName, compressionType);
        }
    }
}
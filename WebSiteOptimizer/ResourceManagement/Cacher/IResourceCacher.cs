using System;
using Labo.WebSiteOptimizer.Compression;

namespace Labo.WebSiteOptimizer.ResourceManagement.Cacher
{
    public interface IResourceCacher
    {
        ResourceGroupInfo GetOrAddCachedResource(ResourceType resourceType, string resourceGroupName, CompressionType compressionType, Func<ResourceGroupInfo> funcContent, TimeSpan expiration);
    }
}

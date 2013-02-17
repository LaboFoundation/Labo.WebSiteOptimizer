using System;
using Labo.WebSiteOptimizer.Compression;

namespace Labo.WebSiteOptimizer.ResourceManagement.Cacher
{
    public interface IResourceCacher
    {
        ProcessedResourceGroupInfo GetOrAddCachedResource(ResourceType resourceType, string resourceGroupName, CompressionType compressionType, Func<ProcessedResourceGroupInfo> funcContent, TimeSpan expiration);
    }
}

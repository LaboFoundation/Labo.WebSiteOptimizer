using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IResourceProcessor
    {
        ProcessedResourceGroupInfo ProcessResource(ResourceElementGroup resourceElementGroup, CompressionType compressionType);

        ProcessedResourceInfo ProcessResource(ResourceType resourceType, string fileName, bool isEmbeddedResource, bool minify, CompressionType compressionType);
    }
}
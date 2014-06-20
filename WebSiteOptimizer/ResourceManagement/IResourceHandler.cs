using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    using Labo.WebSiteOptimizer.Compression;

    public interface IResourceHandler
    {
        void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName);

        void HandleResource(HttpContextBase httpContext, ResourceType resourceType, string fileName, bool isEmbeddedResource, bool isHttpResource, bool minify, bool compress);

        void HandleResource(HttpContextBase httpContextBase, ResourceType resourceType, string resourceGroupName, string fileName, bool minify, bool compress);

        void HandleResource(HttpContextBase httpContext, ResourceType resourceType, CompressionType compressionType, IProcessedResourceContentInfo resourceInfo);

        ProcessedResourceGroupInfo ProcessResource(HttpContextBase httpContext, ResourceType resourceType, string resourceGroupName, out CompressionType compressionType);
    }
}
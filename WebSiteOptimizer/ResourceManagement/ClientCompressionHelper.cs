using System.Web;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.Utility;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    internal static class ClientCompressionHelper
    {
        internal static CompressionType GetCompressionType(HttpContextBase httpContext, ResourceElementGroup resourceElementGroup)
        {
            return GetCompressionType(httpContext, resourceElementGroup.Compress);
        }

        internal static CompressionType GetCompressionType(HttpContextBase httpContext, bool compress)
        {
            CompressionType compressionType = CompressionType.None;
            if (compress)
            {
                compressionType = HttpRequestUtils.GetRequestCompressionType(httpContext.Request);
            }
            return compressionType;
        }
    }
}

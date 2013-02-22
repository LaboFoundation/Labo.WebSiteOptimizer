using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement.VirtualPath
{
    public sealed class VirtualPathProvider : IVirtualPathProvider
    {
        public string ToAbsoluteUrl(string url)
        {
            return VirtualPathUtility.ToAbsolute(url);
        }

        public string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        public string GetDirectory(string virtualPath)
        {
            return VirtualPathUtility.GetDirectory(virtualPath);
        }
    }
}
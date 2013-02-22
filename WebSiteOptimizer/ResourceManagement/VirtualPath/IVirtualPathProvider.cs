namespace Labo.WebSiteOptimizer.ResourceManagement.VirtualPath
{
    public interface IVirtualPathProvider
    {
        string ToAbsoluteUrl(string url);

        string CombinePaths(string basePath, string relativePath);

        string GetDirectory(string virtualPath);
    }
}

using System.Web.Mvc;
using System.Web.WebPages.Razor;
using Labo.WebSiteOptimizer.ResourceManagement;

namespace Labo.WebSiteOptimizer.Mvc4
{
    public sealed class HtmlMinifierWebRazorHostFactory : MvcWebRazorHostFactory
    {
        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);
            if (host.IsSpecialPage || host.DesignTimeMode)
            {
                return host;
            }
            return new HtmlMinifierMvcWebPageRazorHost(ResourceManagerRuntime.HtmlMinifier, ResourceManagerRuntime.DebugStatusReader, virtualPath, physicalPath);
        }
    }
}
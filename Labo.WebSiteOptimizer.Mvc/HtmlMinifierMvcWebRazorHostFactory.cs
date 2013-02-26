using System.Web.Mvc;
using System.Web.WebPages.Razor;
using Labo.WebSiteOptimizer.ResourceManagement;

namespace Labo.WebSiteOptimizer.Mvc
{
    public sealed class HtmlMinifierMvcWebRazorHostFactory : MvcWebRazorHostFactory
    {
        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);

            if (host.IsSpecialPage)
            {
                return host;
            }

            return new HtmlMinifierMvcWebPageRazorHost(virtualPath, physicalPath, ResourceManagerRuntime.HtmlMinifier, ResourceManagerRuntime.DebugStatusReader);
        }
    }
}
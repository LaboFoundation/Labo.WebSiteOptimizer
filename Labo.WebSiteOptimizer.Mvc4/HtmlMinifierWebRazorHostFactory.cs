namespace Labo.WebSiteOptimizer.Mvc4
{
    using System.Web.Mvc;
    using System.Web.WebPages.Razor;

    using Labo.WebSiteOptimizer.ResourceManagement;
    using Labo.WebSiteOptimizer.ResourceManagement.Minify;

    public sealed class HtmlMinifierWebRazorHostFactory : MvcWebRazorHostFactory
    {
        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);
            if (host.IsSpecialPage || host.DesignTimeMode)
            {
                return host;
            }

            DefaultHtmlPageMinifier htmlPageMinifier = new DefaultHtmlPageMinifier(new SimpleHtmlMinifier(), new DefaultInlineJavascriptMinifier(new YahooJsMinifier()), new DefaultInlineCssMinifier(new YahooCssMinifier()));
            
            return new HtmlMinifierMvcWebPageRazorHost(htmlPageMinifier, new ConditionalCompilationDebugStatusReader(), virtualPath, physicalPath);
        }
    }
}
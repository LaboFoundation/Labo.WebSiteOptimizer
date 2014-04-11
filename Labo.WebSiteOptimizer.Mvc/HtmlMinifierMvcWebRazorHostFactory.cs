namespace Labo.WebSiteOptimizer.Mvc
{
    using System.Web.Mvc;
    using System.Web.WebPages.Razor;

    using Labo.WebSiteOptimizer.ResourceManagement;
    using Labo.WebSiteOptimizer.ResourceManagement.Minify;

    public sealed class HtmlMinifierMvcWebRazorHostFactory : MvcWebRazorHostFactory
    {
        public override WebPageRazorHost CreateHost(string virtualPath, string physicalPath)
        {
            WebPageRazorHost host = base.CreateHost(virtualPath, physicalPath);

            if (host.IsSpecialPage)
            {
                return host;
            }

            DefaultHtmlPageMinifier htmlPageMinifier = new DefaultHtmlPageMinifier(new SimpleHtmlMinifier(), new DefaultInlineJavascriptMinifier(new YahooJsMinifier()), new DefaultInlineCssMinifier(new YahooCssMinifier()));

            return new HtmlMinifierMvcWebPageRazorHost(virtualPath, physicalPath, htmlPageMinifier, new ConditionalCompilationDebugStatusReader());
        }
    }
}
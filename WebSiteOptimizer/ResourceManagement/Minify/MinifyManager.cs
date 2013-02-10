namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public static class MinifyManager
    {
        private static readonly IJsMinifier s_JsMinifier = new YahooJsMinifier();
        public static IJsMinifier JsMinifier
        {
            get { return s_JsMinifier; }
        }

        private static readonly ICssMinifier s_CssMinifier = new YahooCssMinifier();
        public static ICssMinifier CssMinifier
        {
            get { return s_CssMinifier; }
        }

        private static readonly IHtmlMinifier s_HtmlMinifier = null;
        public static IHtmlMinifier HtmlMinifier
        {
            get { return s_HtmlMinifier; }
        }
    }
}
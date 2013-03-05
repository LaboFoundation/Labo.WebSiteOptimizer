namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    internal sealed class DefaultHtmlPageMinifier : IHtmlPageMinifier
    {
        private readonly IHtmlMinifier m_HtmlMinifier;
        private readonly IInlineJavascriptMinifier m_InlineJsMinifier;
        private readonly IInlineCssMinifier m_InlineCssMinifier;

        public DefaultHtmlPageMinifier(IHtmlMinifier htmlMinifier, IInlineJavascriptMinifier inlineJsMinifier, IInlineCssMinifier inlineCssMinifier)
        {
            m_HtmlMinifier = htmlMinifier;
            m_InlineJsMinifier = inlineJsMinifier;
            m_InlineCssMinifier = inlineCssMinifier;
        }

        public string Minify(string content, bool minifyInlineCss = false, bool minifyInlineJavascript = false)
        {
            if (minifyInlineCss)
            {
                content = m_InlineCssMinifier.MinifyInlineCss(content);
            }
            if (minifyInlineJavascript)
            {
                content = m_InlineJsMinifier.MinifyInlineScripts(content);                
            }
            return m_HtmlMinifier.Minify(content);
        }
    }
}
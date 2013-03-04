using System.Text.RegularExpressions;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public sealed class SimpleHtmlMinifier : IHtmlMinifier
    {
        private readonly IJsMinifier m_JsMinifier;
        private readonly ICssMinifier m_CssMinifier;

        public SimpleHtmlMinifier(IJsMinifier jsMinifier, ICssMinifier cssMinifier)
        {
            m_JsMinifier = jsMinifier;
            m_CssMinifier = cssMinifier;
        }

        public string Minify(string content, bool mififyInlineCss = false, bool minifyInlineJavascript = false)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            content = Regex.Replace(content, @"\n|\t", string.Empty);
            content = Regex.Replace(content, @">\s+<", "><").Trim();
            content = Regex.Replace(content, @"\s{2,}", " ");

            return content;
        }
    }
}
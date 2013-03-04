using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public sealed class SimpleHtmlMinifier : IHtmlMinifier
    {
        private readonly IJsMinifier m_JsMinifier;
        private readonly ICssMinifier m_CssMinifier;

        private const string SINGLE_SPACE = " ";
        private static readonly Regex s_NewLineWhiteSpaceRegex = new Regex(@"\s*\n\s*", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex s_RemoveMultipleWhiteSpaceRegex = new Regex(@"\s{2,}", RegexOptions.Compiled | RegexOptions.Multiline);

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

            content = s_NewLineWhiteSpaceRegex.Replace(content, "\n");
            content = s_RemoveMultipleWhiteSpaceRegex.Replace(content, SINGLE_SPACE);

            return content;
        }
    }
}
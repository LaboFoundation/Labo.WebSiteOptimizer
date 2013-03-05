using System.Text.RegularExpressions;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    internal sealed class DefaultInlineCssMinifier : IInlineCssMinifier
    {
        private readonly ICssMinifier m_CssMinifier;

        private readonly static Regex s_InlineCssRegex = new Regex("<style\\s?[^>]*?>+([^<]*)</style>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

        public DefaultInlineCssMinifier(ICssMinifier cssMinifier)
        {
            m_CssMinifier = cssMinifier;
        }

        public string MinifyInlineCss(string htmlContent)
        {
            MatchCollection matches = s_InlineCssRegex.Matches(htmlContent);
            foreach (Match match in matches)
            {
                string css = match.Groups[1].Value;
                if (!string.IsNullOrEmpty(css))
                {
                    htmlContent = htmlContent.Replace(css, m_CssMinifier.Minify(css));
                }
            }
            return htmlContent;
        }
    }
}
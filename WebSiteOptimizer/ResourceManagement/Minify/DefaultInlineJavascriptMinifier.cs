using System.Text.RegularExpressions;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public sealed class DefaultInlineJavascriptMinifier : IInlineJavascriptMinifier
    {
        private readonly IJsMinifier m_JsMinifier;

        private readonly static Regex s_InlineJavascriptRegex = new Regex("<script[^>]*>([\\S\\s]*?)</script>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

        public DefaultInlineJavascriptMinifier(IJsMinifier jsMinifier)
        {
            m_JsMinifier = jsMinifier;
        }

        public string MinifyInlineScripts(string htmlContent)
        {
            MatchCollection matches = s_InlineJavascriptRegex.Matches(htmlContent);
            foreach (Match match in matches)
            {
                string script = match.Groups[1].Value;
                if (!string.IsNullOrEmpty(script))
                {
                    htmlContent = htmlContent.Replace(script, m_JsMinifier.Minify(script));
                }
            }
            return htmlContent;
        }
    }
}
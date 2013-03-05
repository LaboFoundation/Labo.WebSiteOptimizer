using System.Text.RegularExpressions;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public sealed class SimpleHtmlMinifier : IHtmlMinifier
    {
        private const string SINGLE_SPACE = " ";
        private static readonly Regex s_NewLineWhiteSpaceRegex = new Regex(@"\s*\n\s*", RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly Regex s_RemoveMultipleWhiteSpaceRegex = new Regex(@"\s{2,}", RegexOptions.Compiled | RegexOptions.Multiline);

        public string Minify(string content)
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
namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public interface IHtmlMinifier
    {
        string Minify(string content, bool mififyInlineCss = false, bool minifyInlineJavascript = false);
    }
}
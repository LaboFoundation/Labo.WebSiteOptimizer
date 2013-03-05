namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public interface IHtmlPageMinifier
    {
        string Minify(string content, bool minifyInlineCss = false, bool minifyInlineJavascript = false);
    }
}
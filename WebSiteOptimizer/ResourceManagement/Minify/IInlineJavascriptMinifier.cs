namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public interface IInlineJavascriptMinifier
    {
        string MinifyInlineScripts(string htmlContent);
    }
}

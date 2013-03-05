namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public interface IInlineCssMinifier
    {
        string MinifyInlineCss(string htmlContent);
    }
}
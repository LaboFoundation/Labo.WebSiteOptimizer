namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public interface ICssMinifier
    {
        byte[] Minify(byte[] content);
        string Minify(string content);
    }
}
using System.Globalization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public interface IJsMinifier
    {
        byte[] Minify(byte[] content);

        string Minify(string content, bool enableOptimizations = false, bool obfuscate = false, CultureInfo culture = null);
    }
}
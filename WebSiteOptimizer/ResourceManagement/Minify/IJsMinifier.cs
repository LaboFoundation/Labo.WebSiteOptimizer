using System.Globalization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public interface IJsMinifier
    {
        string Minify(string content, bool enableOptimizations = false, bool obfuscate = false, CultureInfo culture = null);
    }
}
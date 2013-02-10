using System.Text;
using System.Globalization;
using Yahoo.Yui.Compressor;

namespace Labo.WebSiteOptimizer.ResourceManagement.Minify
{
    public sealed class YahooJsMinifier : IJsMinifier
    {
        public string Minify(string content, bool enableOptimizations = false, bool obfuscate = false, CultureInfo culture = null)
        {
            JavaScriptCompressor javaScriptCompressor = new JavaScriptCompressor();
            javaScriptCompressor.CompressionType = CompressionType.Standard;
            javaScriptCompressor.ThreadCulture = culture ?? CultureInfo.InvariantCulture;
            javaScriptCompressor.Encoding = Encoding.UTF8;
            javaScriptCompressor.DisableOptimizations = !enableOptimizations;
            javaScriptCompressor.IgnoreEval = true;
            javaScriptCompressor.LineBreakPosition = -1;
            javaScriptCompressor.ObfuscateJavascript = obfuscate;
            javaScriptCompressor.PreserveAllSemicolons = false;
            return javaScriptCompressor.Compress(content);
        }
    }
}

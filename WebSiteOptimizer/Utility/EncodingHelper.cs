using System.Text;

namespace Labo.WebSiteOptimizer.Utility
{
    public static class EncodingHelper
    {
        public static Encoding GetCurrentCultureEncoding()
        {
            return Encoding.GetEncoding(System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.ANSICodePage);
        }
    }
}
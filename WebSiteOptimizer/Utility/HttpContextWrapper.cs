using System.Web;

namespace Labo.WebSiteOptimizer.Utility
{
    internal static class HttpContextWrapper
    {
        private static HttpContextBase s_ContextBase;
        internal static HttpContextBase Context
        {
            get
            {
                return s_ContextBase ?? (HttpContext.Current == null ? null : new System.Web.HttpContextWrapper(HttpContext.Current));
            }

            set { s_ContextBase = value; }
        }
    }
}
using System;
using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class HttpResponseCacher : IHttpResponseCacher
    {
        private readonly IDateTimeProvider m_DateTimeProvider;

        public HttpResponseCacher(IDateTimeProvider dateTimeProvider)
        {
            if (dateTimeProvider == null)
            {
                throw new ArgumentNullException("dateTimeProvider");
            }
            m_DateTimeProvider = dateTimeProvider;
        }

        public void Cache(HttpContextBase context, DateTime lastModifyDate)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpCachePolicyBase cache = context.Response.Cache;

            cache.SetLastModified(lastModifyDate);
            cache.SetVaryByCustom("Accept-Encoding");
            cache.SetValidUntilExpires(true);
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(m_DateTimeProvider.UtcNow.AddYears(1));
            cache.SetMaxAge(TimeSpan.FromDays(365));
        }
    }
}
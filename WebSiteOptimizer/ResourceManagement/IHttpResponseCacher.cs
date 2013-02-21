using System;
using System.Web;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IHttpResponseCacher
    {
        void Cache(HttpContextBase context, DateTime lastModifyDate);
    }
}

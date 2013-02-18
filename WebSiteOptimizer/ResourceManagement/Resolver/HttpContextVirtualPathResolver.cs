using System;
using System.Web;
using Labo.WebSiteOptimizer.Extensions;
using HttpContextWrapper = Labo.WebSiteOptimizer.Utility.HttpContextWrapper;

namespace Labo.WebSiteOptimizer.ResourceManagement.Resolver
{
    internal sealed class HttpContextVirtualPathResolver : IVirtualPathResolver
    {
        public string Resolve(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            HttpContextBase context = HttpContextWrapper.Context;
            if (context == null)
            {
                throw new InvalidOperationException("Cannot resolve path '{0}' because HttpContext is null.".FormatWith(path));
            }
            return context.Server.MapPath(path);
        }
    }
}
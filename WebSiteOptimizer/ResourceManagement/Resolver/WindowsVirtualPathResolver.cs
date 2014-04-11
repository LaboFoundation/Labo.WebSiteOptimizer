using System;
using System.IO;

namespace Labo.WebSiteOptimizer.ResourceManagement.Resolver
{
    public sealed class WindowsVirtualPathResolver : IVirtualPathResolver
    {
        public string Resolve(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.StartsWith("~\\", StringComparison.OrdinalIgnoreCase) || path.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Remove(0, 2);
            }

            return Path.Combine(Environment.CurrentDirectory, path);
        }
    }
}
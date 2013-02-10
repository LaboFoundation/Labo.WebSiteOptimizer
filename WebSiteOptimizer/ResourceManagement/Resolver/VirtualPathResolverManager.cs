using System;
using Labo.WebSiteOptimizer.Utility;

namespace Labo.WebSiteOptimizer.ResourceManagement.Resolver
{
    internal sealed class VirtualPathResolverManager : IVirtualPathResolver
    {
        private readonly Lazy<IVirtualPathResolver> m_HttpContextVirtualPathResolver;
        private readonly Lazy<IVirtualPathResolver> m_WindowsVirtualPathResolver;

        public VirtualPathResolverManager(Func<IVirtualPathResolver> funcHttpContextVirtualPathResolver, Func<IVirtualPathResolver> funcWebVirtualPathResolver)
        {
            m_HttpContextVirtualPathResolver = new Lazy<IVirtualPathResolver>(funcHttpContextVirtualPathResolver, true);
            m_WindowsVirtualPathResolver = new Lazy<IVirtualPathResolver>(funcWebVirtualPathResolver, true);
        }

        public string Resolve(string path)
        {
            return HttpContextWrapper.Context == null ? m_WindowsVirtualPathResolver.Value.Resolve(path) : m_HttpContextVirtualPathResolver.Value.Resolve(path);
        }
    }
}

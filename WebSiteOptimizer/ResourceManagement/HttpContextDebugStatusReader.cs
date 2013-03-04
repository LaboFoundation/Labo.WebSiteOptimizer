using Labo.WebSiteOptimizer.Utility;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    internal sealed class HttpContextDebugStatusReader : IDebugStatusReader
    {
        public bool IsDebuggingEnabled()
        {
            return HttpContextWrapper.Context.IsDebuggingEnabled;
        }
    }
}
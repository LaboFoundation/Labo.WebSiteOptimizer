namespace Labo.WebSiteOptimizer.ResourceManagement
{
    using System.Configuration;
    using System.Web.Configuration;

    public sealed class WebConfigDebugStatusReader : IDebugStatusReader
    {
        public bool IsDebuggingEnabled()
        {
            object section = ConfigurationManager.GetSection("system.web/compilation");
            if (section == null)
            {
                return false;
            }

            CompilationSection cfg = section as CompilationSection;
            return cfg != null && cfg.Debug;
        }
    }
}
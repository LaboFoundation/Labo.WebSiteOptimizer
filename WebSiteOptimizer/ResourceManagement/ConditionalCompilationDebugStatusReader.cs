namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class ConditionalCompilationDebugStatusReader : IDebugStatusReader
    {
        public bool IsDebuggingEnabled()
        {
            bool isDebuggingEnabled;

#if DEBUG
            isDebuggingEnabled = true;
#else
            isDebuggingEnabled = false;
#endif
            return isDebuggingEnabled;
        }
    }
}
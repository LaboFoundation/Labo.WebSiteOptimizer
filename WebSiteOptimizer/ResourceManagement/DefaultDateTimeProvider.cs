using System;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    internal sealed class DefaultDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow { get { return DateTime.UtcNow; } }
    }
}

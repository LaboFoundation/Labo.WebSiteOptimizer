using System;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
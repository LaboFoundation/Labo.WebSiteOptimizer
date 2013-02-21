using System;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public interface IProcessedResourceContentInfo
    {
        byte[] Content { get; set; }

        string Hash { get; set; }

        DateTime LastModifyDate { get; set; }
    }
}
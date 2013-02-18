using System;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    [Serializable]
    public sealed class ProcessedResourceInfo
    {
        public byte[] Content { get; set; }

        public string Hash { get; set; }

        public DateTime LastModifyDate { get; set; }

        public string DependentFile { get; set; }
    }
}
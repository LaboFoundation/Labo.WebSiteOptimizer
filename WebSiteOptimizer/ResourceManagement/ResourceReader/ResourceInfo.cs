using System;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    public sealed class ResourceInfo
    {
        public string Content { get; set; }

        public string DependentFile { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
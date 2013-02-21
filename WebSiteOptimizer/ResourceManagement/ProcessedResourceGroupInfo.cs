using System;
using System.Collections.Generic;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    [Serializable]
    public sealed class ProcessedResourceGroupInfo : IProcessedResourceContentInfo
    {
        public byte[] Content { get; set; }

        public string Hash { get; set; }

        public DateTime LastModifyDate { get; set; }

        private HashSet<string> m_DependentFiles;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public HashSet<string> DependentFiles
        {
            get { return m_DependentFiles ?? (m_DependentFiles = new HashSet<string>()); }
            set { m_DependentFiles = value; }
        }
    }
}
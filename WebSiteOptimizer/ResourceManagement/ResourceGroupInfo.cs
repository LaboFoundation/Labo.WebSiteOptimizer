using System;
using System.Collections.Generic;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    [Serializable]
    public sealed class ResourceGroupInfo
    {
        public byte[] Content { get; set; }

        public string Hash { get; set; }

        public DateTime LastModifyDate { get; set; }

        private HashSet<string> m_DependentFiles;
        public HashSet<string> DependentFiles
        {
            get { return m_DependentFiles ?? (m_DependentFiles = new HashSet<string>()); }
            set { m_DependentFiles = value; }
        }

    }
}
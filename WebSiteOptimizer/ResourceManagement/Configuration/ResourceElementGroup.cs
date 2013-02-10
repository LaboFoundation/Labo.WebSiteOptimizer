using System;
using System.Xml.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "resourceGroup")]
    public class ResourceElementGroup
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("minify")]
        public bool Minify { get; set; }

        [XmlAttribute("compress")]
        public bool Compress { get; set; }

        [XmlAttribute("cacheDuration")]
        public int CacheDuration { get; set; }

        private ResourceElementCollection m_Resources;
        [XmlElement("resource")]
        public ResourceElementCollection Resources
        {
            get { return m_Resources ?? (m_Resources = new ResourceElementCollection()); }
            set { m_Resources = value; }
        }
    }
}
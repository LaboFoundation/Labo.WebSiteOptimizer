using System;
using System.Linq;
using System.Xml.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "resources")]
    public class CssResources
    {
        private ResourceElementGroupCollection m_ResourceGroups;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly"), XmlElement("resourceGroup")]
        public ResourceElementGroupCollection ResourceGroups
        {
            get { return m_ResourceGroups ?? (m_ResourceGroups = new ResourceElementGroupCollection()); }
            set { m_ResourceGroups = value; }
        }

        public ResourceElementGroup GetResourceGroupByName(string name)
        {
            return ResourceGroups.Single(x => x.Name == name);
        }
    }
}
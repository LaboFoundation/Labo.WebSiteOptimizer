using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "resourceGroups")]
    public sealed class ResourceElementGroupCollection : List<ResourceElementGroup>
    {
        public ResourceElementGroupCollection()
            : base()
        {
        }

        public ResourceElementGroupCollection(IEnumerable<ResourceElementGroup> collection)
            : base(collection)
        {
        }

        public ResourceElementGroupCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "resources")]
    public sealed class ResourceElementCollection : List<ResourceElement>
    {
        public ResourceElementCollection()
            : base()
        {
        }

        public ResourceElementCollection(IEnumerable<ResourceElement> collection)
            : base(collection)
        {
        }

        public ResourceElementCollection(int capacity)
            : base(capacity)
        {
        }
    }
}
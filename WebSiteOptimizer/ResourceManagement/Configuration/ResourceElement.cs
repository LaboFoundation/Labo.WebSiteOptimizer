using System;
using System.Xml.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "resource")]
    public sealed class ResourceElement
    {
        [XmlAttribute("filename")]
        public string FileName { get; set; }

        [XmlAttribute("minify")]
        public bool? Minify { get; set; }

        [XmlAttribute("isEmbeddedResource")]
        public bool IsEmbeddedResource { get; set; }
    }
}
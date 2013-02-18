using System;
using System.Xml.Serialization;
using Labo.WebSiteOptimizer.Extensions;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    [Serializable]
    [XmlRoot(ElementName = "resource")]
    public sealed class ResourceElement
    {
        [XmlAttribute("filename")]
        public string FileName { get; set; }

        [XmlIgnore]
        public bool? Minify { get; set; }

        [XmlAttribute("minify")]
        public string MinifyString
        {
            get { return Minify.ToStringInvariant().ToLowerInvariant(); }
            set
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    Minify = bool.Parse(value);
                }
            }
        }


        [XmlAttribute("isEmbeddedResource")]
        public bool IsEmbeddedResource { get; set; }
    }
}
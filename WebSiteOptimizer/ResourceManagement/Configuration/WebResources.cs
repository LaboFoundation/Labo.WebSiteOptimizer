using System;
using System.Xml.Serialization;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    [Serializable]
    [XmlRoot("webResources")]
    public class WebResources
    {
        private JavascriptResources m_JavascriptResources;
        [XmlElement("js")]
        public JavascriptResources JavascriptResources
        {
            get { return m_JavascriptResources ?? (m_JavascriptResources = new JavascriptResources()); }
            set { m_JavascriptResources = value; }
        }

        private CssResources m_CssResources;
        [XmlElement("css")]
        public CssResources CssResources
        {
            get { return m_CssResources ?? (m_CssResources = new CssResources()); }
            set { m_CssResources = value; }
        }
    }
}

using System;
using System.Globalization;
using System.Xml.Serialization;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;

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

        public ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName)
        {
            ResourceElementGroup resourceElementGroup;
            switch (resourceType)
            {
                case ResourceType.Js:
                    resourceElementGroup = JavascriptResources.GetResourceGroupByName(resourceGroupName);
                    resourceElementGroup.ResourceType = resourceType;
                    break;
                case ResourceType.Css:
                    resourceElementGroup = CssResources.GetResourceGroupByName(resourceGroupName);
                    resourceElementGroup.ResourceType = resourceType;
                    break;
                default:
                    throw new ResourceConfigurationException(String.Format(CultureInfo.CurrentCulture, "resource type '{0}' not supported", resourceType));
            }
            return resourceElementGroup;
        }
    }
}

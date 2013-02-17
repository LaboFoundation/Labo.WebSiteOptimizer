using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Labo.WebSiteOptimizer.Caching;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    public sealed class WebResourceXmlConfiguration : IWebResourceConfiguration
    {
        private readonly ICacheProvider m_CacheProvider;
        private readonly IVirtualPathResolver m_VirtualPathResolver;
        private readonly string m_XmlConfigurationPath;

        private WebResources WebResources
        {
            get
            {
                return m_CacheProvider.GetOrAdd("ResourceManagement:Labo.WebResources.Configuration", () => LoadWebResourcesConfig(m_XmlConfigurationPath), TimeSpan.FromHours(1), () => new List<string> { m_XmlConfigurationPath });
            }
        }

        public WebResourceXmlConfiguration(ICacheProvider cacheProvider, IVirtualPathResolver virtualPathResolver)
        {
            m_CacheProvider = cacheProvider;
            m_VirtualPathResolver = virtualPathResolver;
            m_XmlConfigurationPath = m_VirtualPathResolver.Resolve("~/App_Data/WebResources.xml");
        }

        public ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName)
        {
            ResourceElementGroup resourceElementGroup;
            if (resourceType == ResourceType.Js)
            {
                resourceElementGroup = WebResources.JavascriptResources.GetResourceGroupByName(resourceGroupName);
                resourceElementGroup.ResourceType = resourceType;
            }
            else if (resourceType == ResourceType.Css)
            {
                resourceElementGroup = WebResources.CssResources.GetResourceGroupByName(resourceGroupName);
                resourceElementGroup.ResourceType = resourceType;
            }
            else
            {
                throw new ResourceConfigurationException(String.Format(CultureInfo.CurrentCulture, "resource type '{0}' not supported", resourceType));
            }
            return resourceElementGroup;
        }

        private static WebResources LoadWebResourcesConfig(string xmlPath)
        {
            string xml = File.ReadAllText(xmlPath);
            return new XmlSerializer().Deserialize<WebResources>(xml);
        }
    }
}

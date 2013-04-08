using System;
using System.Collections.Generic;
using System.IO;

using Labo.WebSiteOptimizer.Caching;
using Labo.WebSiteOptimizer.ResourceManagement.Cacher;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    public sealed class ResourceXmlConfigurationProvider : IResourceConfigurationProvider
    {
        private readonly ICacheProvider m_CacheProvider;
        private readonly IResourceCacher m_ResourceCacher;
        private readonly string m_XmlConfigurationPath;
        private static readonly XmlSerializer s_XmlSerializer = new XmlSerializer();

        private WebResources WebResources
        {
            get
            {
                return m_CacheProvider.GetOrAdd("ResourceManagement:Labo.WebResources.Configuration", () => LoadWebResourcesConfig(m_XmlConfigurationPath), TimeSpan.FromHours(1), () => new List<string> { m_XmlConfigurationPath });
            }
        }

        public ResourceXmlConfigurationProvider(ICacheProvider cacheProvider, IVirtualPathResolver virtualPathResolver, IResourceCacher resourceCacher)
            : this(cacheProvider, virtualPathResolver.Resolve("~/App_Data/WebResources.xml"), resourceCacher)
        {
        }

        public ResourceXmlConfigurationProvider(ICacheProvider cacheProvider, string configurationPath, IResourceCacher resourceCacher)
        {
            m_CacheProvider = cacheProvider;
            m_XmlConfigurationPath = configurationPath;
            m_ResourceCacher = resourceCacher;

            m_ResourceCacher.AddDependentFile(configurationPath);
        }

        public ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName)
        {
            return WebResources.GetResourceElementGroup(resourceType, resourceGroupName);
        }

        private static WebResources LoadWebResourcesConfig(string xmlPath)
        {
            string xml = File.ReadAllText(xmlPath);
            return s_XmlSerializer.Deserialize<WebResources>(xml);
        }
    }
}

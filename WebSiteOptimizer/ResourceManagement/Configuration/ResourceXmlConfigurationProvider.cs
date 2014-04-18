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
        private readonly IVirtualPathResolver m_VirtualPathResolver;
        private readonly Lazy<string> m_XmlConfigurationPathProvider; 

        private WebResources WebResources
        {
            get
            {
                return m_CacheProvider.GetOrAdd(
                    "ResourceManagement:Labo.WebResources.Configuration",
                    () => LoadWebResourcesConfig(m_XmlConfigurationPathProvider.Value), 
                    TimeSpan.FromHours(1), 
                    () => new List<string> { m_XmlConfigurationPathProvider.Value });
            }
        }

        public ResourceXmlConfigurationProvider(ICacheProvider cacheProvider, IVirtualPathResolver virtualPathResolver, IResourceCacher resourceCacher)
            : this(cacheProvider, null, virtualPathResolver, resourceCacher)
        {
        }

        public ResourceXmlConfigurationProvider(ICacheProvider cacheProvider, string configurationPath, IVirtualPathResolver virtualPathResolver, IResourceCacher resourceCacher)
        {
            if (cacheProvider == null)
            {
                throw new ArgumentNullException("cacheProvider");
            }

            if (resourceCacher == null)
            {
                throw new ArgumentNullException("resourceCacher");
            }

            if (string.IsNullOrEmpty(configurationPath) && virtualPathResolver == null)
            {
                throw new ArgumentNullException("configurationPath");                    
            }

            m_CacheProvider = cacheProvider;
            m_VirtualPathResolver = virtualPathResolver;
            m_ResourceCacher = resourceCacher;

            m_XmlConfigurationPathProvider = new Lazy<string>(
                () =>
                    {
                        if (string.IsNullOrWhiteSpace(configurationPath))
                        {
                            configurationPath = m_VirtualPathResolver.Resolve("~/App_Data/WebResources.xml");
                        }

                        m_ResourceCacher.AddDependentFile(configurationPath);

                        return configurationPath;
                    },
                true);
        }

        public ResourceElementGroup GetResourceElementGroup(ResourceType resourceType, string resourceGroupName)
        {
            return WebResources.GetResourceElementGroup(resourceType, resourceGroupName);
        }

        private static WebResources LoadWebResourcesConfig(string xmlPath)
        {
            string xml = File.ReadAllText(xmlPath);
            return XmlSerializer.Deserialize<WebResources>(xml);
        }
    }
}

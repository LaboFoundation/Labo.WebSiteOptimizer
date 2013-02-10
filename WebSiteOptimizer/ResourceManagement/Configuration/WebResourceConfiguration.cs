using System;
using System.Collections.Generic;
using System.IO;
using Labo.WebSiteOptimizer.Caching;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;

namespace Labo.WebSiteOptimizer.ResourceManagement.Configuration
{
    public sealed class WebResourceConfiguration
    {
        private readonly ICacheProvider m_CacheProvider;
        private readonly IVirtualPathResolver m_VirtualPathResolver;
        private readonly string m_XmlConfigurationPath;
        private static readonly WebResourceConfiguration s_Instance = new WebResourceConfiguration(null, null);
        public static WebResourceConfiguration Instance
        {
            get { return s_Instance; }
        }

        public WebResources WebResources
        {
            get
            {
                return m_CacheProvider.GetOrAdd("ResourceManagement:Labo.WebResources.Configuration", () => LoadWebResourcesConfig(m_XmlConfigurationPath), TimeSpan.FromHours(1), () => new List<string> { m_XmlConfigurationPath });
            }
        }

        private WebResourceConfiguration(ICacheProvider cacheProvider, IVirtualPathResolver virtualPathResolver)
        {
            m_CacheProvider = cacheProvider;
            m_VirtualPathResolver = virtualPathResolver;
            m_XmlConfigurationPath = m_VirtualPathResolver.Resolve("~/App_Data/WebResources.xml");
        }

        private static WebResources LoadWebResourcesConfig(string xmlPath)
        {
            string xml = File.ReadAllText(xmlPath);
            return new XmlSerializer().Deserialize<WebResources>(xml);
        }
    }
}

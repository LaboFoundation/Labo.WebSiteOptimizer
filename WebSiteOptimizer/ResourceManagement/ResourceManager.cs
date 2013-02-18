using System;
using System.Web;
using System.Globalization;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class ResourceManager : IResourceManager
    {
        private readonly IResourceProcessor m_ResourceProcessor;
        private readonly IWebResourceConfigurationProvider m_WebResourceConfiguration;

        public ResourceManager(IResourceProcessor resourceProcessor, IWebResourceConfigurationProvider webResourceConfiguration)
        {
            m_ResourceProcessor = resourceProcessor;
            m_WebResourceConfiguration = webResourceConfiguration;
        }

        public string RenderJavascriptInclude(HttpContextBase httpContext, string resourceGroupName)
        {
            ResourceElementGroup resourceElementGroup = m_WebResourceConfiguration.GetResourceElementGroup(ResourceType.Js, resourceGroupName);
            ProcessedResourceGroupInfo resourceGroupInfo = m_ResourceProcessor.ProcessResource(httpContext, resourceElementGroup);
            return "<script src='/resource/js/{0}/{1}' type='text/javascript'></script>".FormatWith(CultureInfo.InvariantCulture, resourceGroupInfo.Hash, resourceGroupName);
        }

        public string RenderCssInclude(HttpContextBase httpContext, string resourceGroupName)
        {
            ResourceElementGroup resourceElementGroup = m_WebResourceConfiguration.GetResourceElementGroup(ResourceType.Css, resourceGroupName);
            ProcessedResourceGroupInfo resourceGroupInfo = m_ResourceProcessor.ProcessResource(httpContext, resourceElementGroup);
            return "<link rel='stylesheet' type='text/css' href='/resource/css/{0}/{1}' />".FormatWith(CultureInfo.InvariantCulture, resourceGroupInfo.Hash, resourceGroupName);
        }
    }
}

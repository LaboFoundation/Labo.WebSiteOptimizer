using System;
using System.Web;
using System.Globalization;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public sealed class ResourceManager : IResourceManager
    {
        private readonly IResourceHandler m_ResourceHandler;

        public ResourceManager(IResourceHandler resourceHandler)
        {
            m_ResourceHandler = resourceHandler;
        }

        public string RenderJavascriptInclude(HttpContextBase httpContext, string resourceGroupName)
        {
            ResourceGroupInfo resourceGroupInfo = m_ResourceHandler.ProcessResource(httpContext, ResourceType.Js, resourceGroupName);
            return "<script src='/resource/js/{0}/{1}' type='text/javascript'></script>".FormatWith(CultureInfo.InvariantCulture, resourceGroupInfo.Hash, resourceGroupName);
        }

        public string RenderCssInclude(HttpContextBase httpContext, string resourceGroupName)
        {
            ResourceGroupInfo resourceGroupInfo = m_ResourceHandler.ProcessResource(httpContext, ResourceType.Css, resourceGroupName);
            return "<link rel='stylesheet' type='text/css' href='/resource/css/{0}/{1}' />".FormatWith(CultureInfo.InvariantCulture, resourceGroupInfo.Hash, resourceGroupName);
        }
    }
}

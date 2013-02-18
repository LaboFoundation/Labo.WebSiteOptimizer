using System;
using System.Text;
using System.Web;
using System.Globalization;
using Labo.WebSiteOptimizer.Extensions;
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
            const string embeddedDebugFileFormat = "<script src='/resource/debugjs/{0}/{1}' type='text/javascript'></script>";
            const string fsDebugFileFormat = "<script src='{0}' type='text/javascript'></script>";
            const string fsFileFormat = "<script src='/resource/js/{0}/{1}' type='text/javascript'></script>";

            return RenderIncludeScript(httpContext, resourceGroupName, ResourceType.Js, embeddedDebugFileFormat, fsDebugFileFormat, fsFileFormat);
        }

        public string RenderCssInclude(HttpContextBase httpContext, string resourceGroupName)
        {
            const string embeddedDebugFileFormat = "<link rel='stylesheet' type='text/css' href='/resource/debugcss/{0}/{1}' />";
            const string fsDebugFileFormat = "<link rel='stylesheet' type='text/css' href='{0}' />";
            const string fsFileFormat = "<link rel='stylesheet' type='text/css' href='/resource/css/{0}/{1}' />";

            return RenderIncludeScript(httpContext, resourceGroupName, ResourceType.Css, embeddedDebugFileFormat, fsDebugFileFormat, fsFileFormat);
        }

        private static string GetDebugScript(ResourceElementGroup resourceElementGroup, string embeddedFileFormat, string fsFileFormat)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < resourceElementGroup.Resources.Count; i++)
            {
                ResourceElement resourceElement = resourceElementGroup.Resources[i];
                if (resourceElement.IsEmbeddedResource)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, embeddedFileFormat, resourceElementGroup.Name, resourceElement.FileName);
                }
                else
                {
                    string path = VirtualPathUtility.ToAbsolute(resourceElement.FileName);
                    sb.AppendFormat(CultureInfo.CurrentCulture, fsFileFormat, path);
                }
            }
            return sb.ToString();
        }

        private string RenderIncludeScript(HttpContextBase httpContext, string resourceGroupName, ResourceType resourceType, string embeddedDebugFileFormat, string fsDebugFileFormat, string fsFileFormat)
        {
            ResourceElementGroup resourceElementGroup = m_WebResourceConfiguration.GetResourceElementGroup(resourceType, resourceGroupName);
            if (resourceElementGroup.Debug)
            {
                return GetDebugScript(resourceElementGroup, embeddedDebugFileFormat, fsDebugFileFormat);
            }
            ProcessedResourceGroupInfo resourceGroupInfo = m_ResourceProcessor.ProcessResource(resourceElementGroup, ClientCompressionHelper.GetCompressionType(httpContext, resourceElementGroup));
            return fsFileFormat.FormatWith(CultureInfo.InvariantCulture, resourceGroupInfo.Hash, resourceGroupName);
        }
    }
}

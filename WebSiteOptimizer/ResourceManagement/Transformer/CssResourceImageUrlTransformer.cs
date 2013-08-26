using System;
using System.Text.RegularExpressions;
using Labo.WebSiteOptimizer.Extensions;
using Labo.WebSiteOptimizer.ResourceManagement.VirtualPath;

namespace Labo.WebSiteOptimizer.ResourceManagement.Transformer
{
    public sealed class CssResourceImageUrlTransformer : IResourceTransformer
    {
        private static readonly Regex s_UrlRegex = new Regex(@"url\s*\((\""|\')?(?<url>[^)]+)?(\""|\')?\)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
        private static readonly char[] s_Quotes = new[] { '\'', '\"' };

        private readonly IVirtualPathProvider m_VirtualPathProvider;

        public CssResourceImageUrlTransformer(IVirtualPathProvider virtualPathProvider)
        {
            m_VirtualPathProvider = virtualPathProvider;
        }

        public ResourceReadInfo Transform(ResourceReadInfo resourceReadInfo)
        {
            if (resourceReadInfo == null) throw new ArgumentNullException("resourceReadInfo");

            resourceReadInfo.ResourceInfo.Content = s_UrlRegex.Replace(resourceReadInfo.ResourceInfo.Content, match =>
                {
                    string url = match.Groups["url"].Value.Trim(s_Quotes);

                    if (!url.IsNullOrEmpty()
                        && !url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                        && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                        && !url.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    {
                        url = m_VirtualPathProvider.CombinePaths(m_VirtualPathProvider.GetDirectory(resourceReadInfo.ResourceElement.FileName), url);
                        return "url('{0}')".FormatWith(m_VirtualPathProvider.ToAbsoluteUrl(url));
                    }

                    return "url('{0}')".FormatWith(url);
                });
            return resourceReadInfo;
        }
    }
}
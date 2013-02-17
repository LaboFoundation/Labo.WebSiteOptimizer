using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement.Cacher;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Hasher;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public static class ResourceManagerRuntime
    {
        private static IResourceHandler s_ResourceHandler;
        private static IWebResourceConfiguration s_WebResourceConfiguration;
        private static IResourceProcessor s_ResourceProcessor;
        private static IResourceCacher s_ResourceCacher;
        private static IResourceReaderManager s_ResourceReader;
        private static ICompressionFactory s_CompressionFactory;
        private static IResourceHasher s_ResourceHasher;
        private static IJsMinifier s_JsMinifier;
        private static ICssMinifier s_CssMinifier;

        static ResourceManagerRuntime()
        {
            s_ResourceCacher = new ResourceCacher(null);
           
            VirtualPathResolverManager virtualPathResolverManager = new VirtualPathResolverManager(() => new HttpContextVirtualPathResolver(), () => new WindowsVirtualPathResolver());
            ResourceReaderManager resourceReaderManager = new ResourceReaderManager(() => new EmbeddedResourceResolver(), () => new FileSystemResourceReader(virtualPathResolverManager));
            s_ResourceReader = resourceReaderManager;

            s_CompressionFactory = new CompressionFactory();
            s_ResourceHasher = new Md5ResourceHasher();
            s_CssMinifier = new YahooCssMinifier();
            s_JsMinifier = new YahooJsMinifier();

            s_ResourceProcessor = new ResourceProcessor(s_ResourceCacher, s_ResourceReader, s_CompressionFactory, s_ResourceHasher, s_JsMinifier, s_CssMinifier);
            s_ResourceHandler = new ResourceHandler(s_ResourceProcessor, s_WebResourceConfiguration, new DefaultDateTimeProvider());
        }

        public static IResourceHandler ResourceHandler
        {
            get { return s_ResourceHandler; }
        }
    }
}

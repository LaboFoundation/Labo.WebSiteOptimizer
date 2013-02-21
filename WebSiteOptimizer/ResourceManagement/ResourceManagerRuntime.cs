using Labo.WebSiteOptimizer.Caching;
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
        private static IResourceManager s_ResourceManager;
        private static IResourceHandler s_ResourceHandler;
        private static IWebResourceConfigurationProvider s_WebResourceConfiguration;
        private static IResourceProcessor s_ResourceProcessor;
        private static IResourceCacher s_ResourceCacher;
        private static IResourceReaderManager s_ResourceReader;
        private static ICompressionFactory s_CompressionFactory;
        private static IResourceHasher s_ResourceHasher;
        private static IJsMinifier s_JsMinifier;
        private static ICssMinifier s_CssMinifier;
        private static ICacheProvider s_CacheProvider;
        private static IVirtualPathResolver s_VirtualPathResolverManager;
        private static IDateTimeProvider s_DateTimeProvider;
        private static IHttpResponseCacher s_HttpResponseCacher;
        private static IHttpResponseCompressor s_HttpResponseCompressor;

        static ResourceManagerRuntime()
        {
            s_VirtualPathResolverManager = new VirtualPathResolverManager(() => new HttpContextVirtualPathResolver(), () => new WindowsVirtualPathResolver());
            s_CacheProvider = new HttpRuntimeCacheProvider();
            s_CompressionFactory = new CompressionFactory();
            s_ResourceHasher = new Md5ResourceHasher();
            s_CssMinifier = new YahooCssMinifier();
            s_JsMinifier = new YahooJsMinifier();
            s_DateTimeProvider = new DefaultDateTimeProvider();
            s_HttpResponseCacher = new HttpResponseCacher(s_DateTimeProvider);
            s_HttpResponseCompressor = new HttpResponseCompressor();

            UpdateDependentObjects();
        }

        private static void UpdateDependentObjects()
        {
            s_ResourceCacher = new DefaultResourceCacher(s_CacheProvider);
            s_ResourceReader = new ResourceReaderManager(() => new EmbeddedResourceResolver(), () => new FileSystemResourceReader(s_VirtualPathResolverManager));
            s_WebResourceConfiguration = new WebResourceXmlConfiguration(s_CacheProvider, s_VirtualPathResolverManager);
            s_ResourceProcessor = new ResourceProcessor(s_ResourceCacher, s_ResourceReader, s_CompressionFactory, s_ResourceHasher, s_JsMinifier, s_CssMinifier);
            s_ResourceHandler = new ResourceHandler(s_ResourceProcessor, s_WebResourceConfiguration, s_HttpResponseCacher, s_HttpResponseCompressor);
            s_ResourceManager = new ResourceManager(s_ResourceProcessor, s_WebResourceConfiguration, s_HttpResponseCompressor);
        }

        public static IResourceHandler ResourceHandler
        {
            get { return s_ResourceHandler; }
        }

        public static IResourceManager ResourceManager
        {
            get { return s_ResourceManager; }
        }

        public static void SetCacheProvider(ICacheProvider cacheProvider)
        {
            s_CacheProvider = cacheProvider;

            UpdateDependentObjects();
        }

        public static void SetWebResourceConfigurationProvider(IWebResourceConfigurationProvider resourceConfigurationProvider)
        {
            s_WebResourceConfiguration = resourceConfigurationProvider;

            UpdateDependentObjects();
        }
    }
}

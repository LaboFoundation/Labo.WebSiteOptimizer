using Labo.WebSiteOptimizer.Caching;
using Labo.WebSiteOptimizer.Compression;
using Labo.WebSiteOptimizer.ResourceManagement.Cacher;
using Labo.WebSiteOptimizer.ResourceManagement.Configuration;
using Labo.WebSiteOptimizer.ResourceManagement.Hasher;
using Labo.WebSiteOptimizer.ResourceManagement.Minify;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;
using Labo.WebSiteOptimizer.ResourceManagement.ResourceReader;
using Labo.WebSiteOptimizer.ResourceManagement.VirtualPath;

namespace Labo.WebSiteOptimizer.ResourceManagement
{
    public static class ResourceManagerRuntime
    {
        private static IResourceManager s_ResourceManager;
        private static IResourceHandler s_ResourceHandler;
        private static IResourceConfigurationProvider s_WebResourceConfiguration;
        private static IResourceProcessor s_ResourceProcessor;
        private static IResourceCacher s_ResourceCacher;
        private static IResourceReaderManager s_ResourceReader;
        private static ICompressionFactory s_CompressionFactory;
        private static IResourceHasher s_ResourceHasher;
        private static IJsMinifier s_JsMinifier;
        private static ICssMinifier s_CssMinifier;
        private static IHtmlMinifier s_HtmlMinifier;
        private static ICacheProvider s_CacheProvider;
        private static IVirtualPathResolver s_VirtualPathResolverManager;
        private static IDateTimeProvider s_DateTimeProvider;
        private static IHttpResponseCacher s_HttpResponseCacher;
        private static IHttpResponseCompressor s_HttpResponseCompressor;
        private static IDebugStatusReader s_DebugStatusReader;
        private static IHtmlPageMinifier s_HtmlPageMinifier;
        private static IRemoteFileTempFolderProvider s_RemoteFileTempFolderProvider;
        private static IVirtualPathProvider s_VirtualPathProvider;

        static ResourceManagerRuntime()
        {
            s_VirtualPathResolverManager = new VirtualPathResolverManager(() => new HttpContextVirtualPathResolver(), () => new WindowsVirtualPathResolver());
            s_CacheProvider = new SystemRuntimeCacheProvider();
            s_CompressionFactory = new CompressionFactory();
            s_ResourceHasher = new Md5ResourceHasher();
            s_CssMinifier = new YahooCssMinifier();
            s_JsMinifier = new YahooJsMinifier();
            s_DebugStatusReader = new HttpContextDebugStatusReader();
            s_DateTimeProvider = new DefaultDateTimeProvider();
            s_HttpResponseCacher = new HttpResponseCacher(s_DateTimeProvider);
            s_HttpResponseCompressor = new HttpResponseCompressor();
            s_RemoteFileTempFolderProvider = new WindowsTempPathRemoteFileTempFolderProvider();
            s_VirtualPathProvider = new VirtualPathProvider();

            UpdateDependentObjects();
        }

        private static void UpdateDependentObjects()
        {
            s_HtmlMinifier = new SimpleHtmlMinifier();
            s_HtmlPageMinifier = new DefaultHtmlPageMinifier(s_HtmlMinifier, new DefaultInlineJavascriptMinifier(s_JsMinifier), new DefaultInlineCssMinifier(s_CssMinifier));
            s_ResourceCacher = new DefaultResourceCacher(s_CacheProvider);
            s_ResourceReader = new ResourceReaderManager(() => new EmbeddedResourceResolver(), () => new FileSystemResourceReader(s_VirtualPathResolverManager), () => new HttpResourceReader(s_RemoteFileTempFolderProvider, s_VirtualPathProvider));
            s_WebResourceConfiguration = new ResourceXmlConfigurationProvider(s_CacheProvider, s_VirtualPathResolverManager);
            s_ResourceProcessor = new ResourceProcessor(s_ResourceCacher, s_ResourceReader, s_CompressionFactory, s_ResourceHasher, s_JsMinifier, s_CssMinifier);
            s_ResourceHandler = new ResourceHandler(s_ResourceProcessor, s_WebResourceConfiguration, s_HttpResponseCacher, s_HttpResponseCompressor);
            s_ResourceManager = new ResourceManager(s_ResourceProcessor, s_WebResourceConfiguration, s_HttpResponseCompressor);
        }

        public static IDebugStatusReader DebugStatusReader
        {
            get { return s_DebugStatusReader; }
        }

        public static IHtmlMinifier HtmlMinifier
        {
            get { return s_HtmlMinifier; }
        }

        public static IResourceHandler ResourceHandler
        {
            get { return s_ResourceHandler; }
        }

        public static IResourceManager ResourceManager
        {
            get { return s_ResourceManager; }
        }

        public static IHtmlPageMinifier HtmlPageMinifier
        {
            get { return s_HtmlPageMinifier; }
        }

        public static void SetDebugStatusReader(IDebugStatusReader debugStatusReader)
        {
            s_DebugStatusReader = debugStatusReader;
        }

        public static void SetRemoteFileTempFolderProvider(IRemoteFileTempFolderProvider remoteFileTempFolderProvider)
        {
            s_RemoteFileTempFolderProvider = remoteFileTempFolderProvider;

            UpdateDependentObjects();
        }

        public static void SetCacheProvider(ICacheProvider cacheProvider)
        {
            s_CacheProvider = cacheProvider;

            UpdateDependentObjects();
        }

        public static void SetVirtualPathProvider(IVirtualPathProvider virtualPathProvider)
        {
            s_VirtualPathProvider = virtualPathProvider;

            UpdateDependentObjects();
        }

        public static void SetResourceConfigurationProvider(IResourceConfigurationProvider resourceConfigurationProvider)
        {
            s_WebResourceConfiguration = resourceConfigurationProvider;

            UpdateDependentObjects();
        }
    }
}

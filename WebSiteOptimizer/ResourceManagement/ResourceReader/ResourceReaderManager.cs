using System;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal sealed class ResourceReaderManager : IResourceReaderManager
    {
        private readonly Lazy<IResourceReader> m_EmbeddedResourceReader;
        private readonly Lazy<IResourceReader> m_FileSystemResourceReader;
        private readonly Lazy<IResourceReader> m_HttpResourceReader;

        public ResourceReaderManager(Func<IResourceReader> embeddedResourceReader, Func<IResourceReader> fileSystemResourceReader, Func<IResourceReader> httpResourceReader)
        {
            m_EmbeddedResourceReader = new Lazy<IResourceReader>(embeddedResourceReader, true);
            m_FileSystemResourceReader = new Lazy<IResourceReader>(fileSystemResourceReader, true);
            m_HttpResourceReader = new Lazy<IResourceReader>(httpResourceReader, true);
        }

        public ResourceInfo ReadResource(ResourceReadOptions resourceConfig)
        {
            if (resourceConfig == null) throw new ArgumentNullException("resourceConfig");

            if (resourceConfig.IsEmbeddedResource)
            {
                return m_EmbeddedResourceReader.Value.ReadResource(resourceConfig.FileName);
            }
            if (resourceConfig.IsHttpResource)
            {
                return m_HttpResourceReader.Value.ReadResource(resourceConfig.FileName);
            }
            return m_FileSystemResourceReader.Value.ReadResource(resourceConfig.FileName);
        }
    }
}

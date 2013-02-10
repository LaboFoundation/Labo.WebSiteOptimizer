using System;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal sealed class ResourceReaderManager : IResourceReaderManager
    {
        private readonly Lazy<IResourceReader> m_EmbeddedResourceReader;
        private readonly Lazy<IResourceReader> m_FileSystemResourceReader;

        public ResourceReaderManager(Func<IResourceReader> embeddedResourceReader, Func<IResourceReader> fileSystemResourceReader)
        {
            m_EmbeddedResourceReader = new Lazy<IResourceReader>(embeddedResourceReader, true);
            m_FileSystemResourceReader = new Lazy<IResourceReader>(fileSystemResourceReader, true);
        }

        public ResourceInfo ReadResource(ResourceReadOptions resourceConfig)
        {
            if (resourceConfig.IsEmbeddedResource)
            {
                return m_EmbeddedResourceReader.Value.ReadResource(resourceConfig.FileName);
            }
            return m_FileSystemResourceReader.Value.ReadResource(resourceConfig.FileName);
        }
    }
}

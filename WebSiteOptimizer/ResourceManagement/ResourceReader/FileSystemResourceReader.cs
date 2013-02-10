using System;
using System.IO;
using System.Text;
using Labo.WebSiteOptimizer.ResourceManagement.Resolver;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal sealed class FileSystemResourceReader : IResourceReader
    {
        private readonly IVirtualPathResolver m_VirtualPathResolver;

        public FileSystemResourceReader(IVirtualPathResolver virtualPathResolver)
        {
            m_VirtualPathResolver = virtualPathResolver;
        }

        public ResourceInfo ReadResource(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            string physicalPath = m_VirtualPathResolver.Resolve(path);
            string content = File.ReadAllText(physicalPath, Encoding.UTF8);
            return new ResourceInfo
                       {
                           Content = content,
                           DependentFile = physicalPath,
                           ModifyDate = File.GetLastWriteTime(physicalPath)
                       };
        }
    }
}
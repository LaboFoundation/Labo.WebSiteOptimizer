using System;
using System.IO;
using System.Reflection;
using System.Text;
using Labo.WebSiteOptimizer.ResourceManagement.Exceptions;
using Labo.WebSiteOptimizer.Utility;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal sealed class EmbeddedResourceResolver : IResourceReader
    {
        public ResourceInfo ReadResource(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!path.Contains("://"))
            {
                ThrowInvalidFormatException();
            }
            string[] parts = path.Split(new[] { "://" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                ThrowInvalidFormatException();
            }
            string assemblyName = parts[0];
            string resourceName = parts[1];
            Assembly assembly = LoadAssembly(assemblyName);
            string content = AssemblyUtils.GetEmbededResourceString(assembly, "{0}.{1}".FormatWith(assemblyName, resourceName), Encoding.UTF8);
            string filePath = new Uri(assembly.CodeBase).LocalPath;
            return new ResourceInfo
            {
                Content = content,
                DependentFile = filePath,
                ModifyDate = File.GetLastWriteTime(filePath)
            };
        }

        private static Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException ex)
            {
                throw new AssemblyNotFoundException(assemblyName, ex);
            }
        }

        private static void ThrowInvalidFormatException()
        {
            throw new InvalidPathFormatException("Embedded resource path format must be '{asseblyname}://{resourcename}'");
        }
    }
}
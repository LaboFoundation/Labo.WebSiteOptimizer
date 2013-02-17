using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using Labo.WebSiteOptimizer.Utility.Exceptions;

namespace Labo.WebSiteOptimizer.Utility
{
    public static class AssemblyUtils
    {
        [FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
        public static DateTime GetAssemblyTime(Assembly assembly)
        {
            AssemblyName assemblyName = assembly.GetName();
            return File.GetLastWriteTime(new Uri(assemblyName.CodeBase).LocalPath);
        }

        #region GetEmbededResourceString
        public static string GetEmbededResourceString(string resourceName)
        {
            return GetEmbededResourceString(Assembly.GetExecutingAssembly(), resourceName, EncodingHelper.GetCurrentCultureEncoding());
        }
        public static string GetEmbededResourceString(string resourceName, Encoding encoding)
        {
            return GetEmbededResourceString(Assembly.GetExecutingAssembly(), resourceName, encoding);
        }
        public static string GetEmbededResourceString(Assembly assembly, string resourceName)
        {
            return GetEmbededResourceString(assembly, resourceName, EncodingHelper.GetCurrentCultureEncoding());
        }
        public static string GetEmbededResourceString(Assembly assembly, string resourceName, Encoding encoding)
        {
            string text = null;
            StreamReader streamReader = null;
            try
            {
                Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName);
                if (manifestResourceStream != null)
                {
                    streamReader = new StreamReader(manifestResourceStream, encoding);
                    text = streamReader.ReadToEnd();
                }
                else
                {
                    ThrowEmbeddedResourceNotFoundException(assembly, resourceName, null);
                }
            }
            catch(FileNotFoundException ex)
            {
                ThrowEmbeddedResourceNotFoundException(assembly, resourceName, ex);
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Dispose();                
                    streamReader = null;
                }
            }
            return text;
        }

        private static void ThrowEmbeddedResourceNotFoundException(Assembly assembly, string resourceName, FileNotFoundException ex)
        {
            EmbeddedResourceNotFoundException assemblyUtilsException = new EmbeddedResourceNotFoundException(String.Format(CultureInfo.CurrentCulture, "embedded resource '{0}' not found", resourceName), ex);
            assemblyUtilsException.Data.Add("ASSEMBLY", assembly.FullName);
            throw assemblyUtilsException;
        }

        #endregion

        #region GetEmbededResourceBinary
        public static byte[] GetEmbededResourceBinary(string resourceName)
        {
            return GetEmbededResourceBinary(Assembly.GetExecutingAssembly(), resourceName);
        }
        public static byte[] GetEmbededResourceBinary(Assembly assembly, string resourceName)
        {
            byte[] resource = null;

            using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (manifestResourceStream != null)
                {
                    resource = new byte[manifestResourceStream.Length];
                    manifestResourceStream.Read(resource, 0, resource.Length);
                }
            }

            return resource;
        }
        #endregion
    }
}

using System.IO;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal sealed class WindowsTempPathRemoteFileTempFolderProvider : IRemoteFileTempFolderProvider
    {
        public string GetTempFolder()
        {
            return Path.GetTempPath();
        }
    }
}
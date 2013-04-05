using System.IO;

namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    internal sealed class WindowsTempPathRemoteFileTempFolderProvider : IRemoteFileTempFolderProvider
    {
        public string GetTempFilePath(string remoteFilePath)
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }
    }
}
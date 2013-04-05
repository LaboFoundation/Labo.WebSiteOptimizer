namespace Labo.WebSiteOptimizer.ResourceManagement.ResourceReader
{
    public interface IRemoteFileTempFolderProvider
    {
        string GetTempFilePath(string remoteFilePath);
    }
}

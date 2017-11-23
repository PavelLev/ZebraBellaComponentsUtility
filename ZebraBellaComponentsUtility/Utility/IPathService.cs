using System.Collections.Generic;

namespace ZebraBellaComponentsUtility.Utility
{
    public interface IPathService
    {
        string GetExecutableDirectoryPath(string componentName);
        string GetExecutableFileName();
        string GetStorageDirectoryPath(string componentName);
        string GetLogsDirectoryPath(string componentName);
        IEnumerable<string> EnumerateComponents();

        string GetComponentDirectory(string componentName);
        string GetAlternativeFileTreeDirectoryPath();
        string GetGitIgnoreAlternativeFileTreeDirectoryPath();
    }
}
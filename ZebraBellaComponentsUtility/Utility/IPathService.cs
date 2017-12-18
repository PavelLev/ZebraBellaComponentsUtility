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
        string GetGitExcludeAlternativeFileTreeLine();
        string GetGitExcludePath();

        string GetChildDirectoryPath(string parentDirectoryPath, string childDirectoryName);
        string GetDirectoryName(string path);

        string Normalize(string path);

        string GetRepositoryAbsolutePath(string repositoryRelativePath);
    }
}
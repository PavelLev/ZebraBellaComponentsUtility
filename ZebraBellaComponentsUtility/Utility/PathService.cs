using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements;

namespace ZebraBellaComponentsUtility.Utility
{
    public class PathService : IPathService
    {
        private readonly ComponentRelativePaths _componentRelativePaths;
        private readonly IDirectoryService _directoryService;
        private readonly string _componentsFolderPath;
        private readonly string _alternativeFileTreeDirectoryPath;
        private readonly string _gitIgnoreAlternativeFileTreeDirectoryPath;
        private readonly string _domainAbsolutePath;
        private readonly string _gitExcludePath;

        public PathService(ComponentRelativePaths componentRelativePaths, ApplicationRelativePaths applicationRelativePaths, RepositoryRelativePaths repositoryRelativePaths, IDirectoryService directoryService)
        {
            _componentRelativePaths = componentRelativePaths;
            _directoryService = directoryService;

            _domainAbsolutePath = Normalize(applicationRelativePaths.DomainRoot);

            _componentsFolderPath = Normalize
                (
                    _domainAbsolutePath, 
                    repositoryRelativePaths.ComponentsFolder
                );


            _alternativeFileTreeDirectoryPath = Normalize
                (
                    _domainAbsolutePath,
                    repositoryRelativePaths.AlternativeFileTreeFolder
                );

            _gitIgnoreAlternativeFileTreeDirectoryPath = repositoryRelativePaths.AlternativeFileTreeFolder;

            if (_gitIgnoreAlternativeFileTreeDirectoryPath.StartsWith("."))
            {
                _gitIgnoreAlternativeFileTreeDirectoryPath = _gitIgnoreAlternativeFileTreeDirectoryPath.Remove(0, 2);
            }


            _gitExcludePath = Normalize(_domainAbsolutePath, ".git\\info\\exclude");
        }

        public string GetExecutableDirectoryPath(string componentName)
        {
            var executableDirectoryPath = Normalize
                (
                    GetComponentDirectory(componentName),
                    _componentRelativePaths.ExecutableDirectory
                );

            return executableDirectoryPath;
        }

        public string GetExecutableFileName()
        {
            return _componentRelativePaths.ExecutableFile;
        }

        public string GetStorageDirectoryPath(string componentName)
        {
            var storageDirectoryPath = Normalize
                (
                    GetComponentDirectory(componentName),
                    _componentRelativePaths.StorageDirectory
                );

            return storageDirectoryPath;
        }

        public string GetLogsDirectoryPath(string componentName)
        {
            var logsDirectoryPath = Normalize
                (
                    GetComponentDirectory(componentName),
                    _componentRelativePaths.LogsDirectory
                );

            return logsDirectoryPath;
        }

        public IEnumerable<string> EnumerateComponents()
        {
            return _directoryService.EnumerateDirectories(_componentsFolderPath)
                .Select(GetDirectoryName);
        }

        public string GetComponentDirectory(string componentName)
        {
            var componentDirectory = Normalize
                (
                    _componentsFolderPath, 
                    componentName
                );

            return componentDirectory;
        }

        public string GetAlternativeFileTreeDirectoryPath()
        {
            return _alternativeFileTreeDirectoryPath;
        }

        public string GetGitExcludeAlternativeFileTreeLine()
        {
            return _gitIgnoreAlternativeFileTreeDirectoryPath;
        }

        public string GetGitExcludePath()
        {
            return _gitExcludePath;
        }

        public string GetChildDirectoryPath(string parentDirectoryPath, string childDirectoryName)
        {
            if (!parentDirectoryPath.EndsWith("\\"))
            {
                parentDirectoryPath += "\\";
            }

            var childDirectoryPath = Normalize
                (
                    parentDirectoryPath, 
                    childDirectoryName
                );

            return childDirectoryPath;
        }

        public string GetDirectoryName(string path)
        {
            path = Normalize(path);

            var startIndex = path.LastIndexOf('\\', path.Length - 2) + 1;

            var length = path.Length - startIndex;

            var directoryName = path.Substring(startIndex, length);

            return directoryName;
        }

        public string Normalize(string value)
        {
            return value.Replace("/", "\\");
        }

        public string Normalize(params string[] values)
        {
            return Path.GetFullPath
                (
                    Path.Combine(values)
                );
        }

        public string GetRepositoryAbsolutePath(string repositoryRelativePath)
        {
            var absolutePath = _domainAbsolutePath + repositoryRelativePath;

            return absolutePath;
        }
    }
}
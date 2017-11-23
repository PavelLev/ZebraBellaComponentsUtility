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
        private readonly string _componentsFolderPath;
        private readonly string _alternativeFileTreeDirectoryPath;
        private readonly string _gitIgnoreAlternativeFileTreeDirectoryPath;

        public PathService(ComponentRelativePaths componentRelativePaths, ApplicationRelativePaths applicationRelativePaths, RepositoryRelativePaths repositoryRelativePaths)
        {
            _componentRelativePaths = componentRelativePaths;
            var currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            _componentsFolderPath = currentDirectoryPath + applicationRelativePaths.RepositoryRoot + repositoryRelativePaths.ComponentsFolder;

            if (!_componentsFolderPath.EndsWith("\\"))
            {
                _componentsFolderPath += "\\";
            }


            _alternativeFileTreeDirectoryPath = currentDirectoryPath + applicationRelativePaths.RepositoryRoot +
                                                repositoryRelativePaths.AlternativeFileTreeFolder;

            _gitIgnoreAlternativeFileTreeDirectoryPath = repositoryRelativePaths.AlternativeFileTreeFolder;

            if (_gitIgnoreAlternativeFileTreeDirectoryPath.StartsWith("."))
            {
                _gitIgnoreAlternativeFileTreeDirectoryPath = _gitIgnoreAlternativeFileTreeDirectoryPath.Remove(0, 2);
            }
        }

        public string GetExecutableDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _componentRelativePaths.ExecutableDirectory;
        }

        public string GetExecutableFileName()
        {
            return _componentRelativePaths.ExecutableFile;
        }

        public string GetStorageDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _componentRelativePaths.StorageDirectory;
        }

        public string GetLogsDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _componentRelativePaths.LogsDirectory;
        }

        public IEnumerable<string> EnumerateComponents()
        {
            return Directory.EnumerateDirectories(_componentsFolderPath).Select(componentDirectoryPath =>
            {
                var startIndex = componentDirectoryPath.LastIndexOf('\\', componentDirectoryPath.Length - 2) + 1;

                var length = componentDirectoryPath.Length - startIndex;

                return componentDirectoryPath.Substring(startIndex, length);
            });
        }

        public string GetComponentDirectory(string componentName)
        {
            return _componentsFolderPath + componentName + "\\";
        }

        public string GetAlternativeFileTreeDirectoryPath()
        {
            return _alternativeFileTreeDirectoryPath;
        }

        public string GetGitIgnoreAlternativeFileTreeDirectoryPath()
        {
            return _gitIgnoreAlternativeFileTreeDirectoryPath;
        }
    }
}
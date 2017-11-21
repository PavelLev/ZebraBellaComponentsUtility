using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZebraBellaComponentsUtility.Settings;

namespace ZebraBellaComponentsUtility.Utility
{
    public class PathService : IPathService
    {
        private readonly ComponentRelativePathSettings _componentRelativePathSettings;
        private readonly string _componentsFolderPath;

        public PathService(ComponentRelativePathSettings componentRelativePathSettings, ApplicationRelativePathSettings applicationRelativePathSettings, RepositoryRelativePathSettings repositoryRelativePathSettings)
        {
            _componentRelativePathSettings = componentRelativePathSettings;
            var currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            _componentsFolderPath = currentDirectoryPath + applicationRelativePathSettings.RepositoryRoot + repositoryRelativePathSettings.ComponentsFolder;

            if (!_componentsFolderPath.EndsWith("\\"))
            {
                _componentsFolderPath += "\\";
            }
        }

        public string GetExecutableDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _componentRelativePathSettings.ExecutableDirectory;
        }

        public string GetExecutableFileName()
        {
            return _componentRelativePathSettings.ExecutableFile;
        }

        public string GetStorageDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _componentRelativePathSettings.StorageDirectory;
        }

        public string GetLogsDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _componentRelativePathSettings.LogsDirectory;
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
    }
}
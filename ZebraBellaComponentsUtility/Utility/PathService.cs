using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZebraBellaComponentsUtility.Utility
{
    public class PathService : IPathService
    {
        private readonly Settings _settings;
        private readonly string _componentsFolderPath;

        public PathService(Settings settings)
        {
            _settings = settings;
            var currentDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

            _componentsFolderPath = currentDirectoryPath + settings.ComponentsFolderRelativePath;

            if (!_componentsFolderPath.EndsWith("\\"))
            {
                _componentsFolderPath += "\\";
            }
        }

        public string GetExecutableDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _settings.ExecutableDirectoryRelativePath;
        }

        public string GetExecutableFileName()
        {
            return _settings.ExecutableFileName;
        }

        public string GetStorageDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _settings.StorageDirectoryRelativePath;
        }

        public string GetLogsDirectoryPath(string componentName)
        {
            return _componentsFolderPath + componentName + "\\" + _settings.LogsDirectoryRelativePath;
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
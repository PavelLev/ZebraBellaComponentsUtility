using System;
using System.IO;
using System.Linq;
using System.Windows;
using NCode.ReparsePoints;
using ZebraBellaComponentsUtility.ConfigurationSections.UserData.ConfigurationElements;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.Components
{
    public class AlternativeFileTreeService : IAlternativeFileTreeService
    {
        private readonly IReparsePointProvider _reparsePointProvider;
        private readonly AlternativeFileTree _alternativeFileTree;
        private readonly IPathService _pathService;

        public AlternativeFileTreeService(IReparsePointProvider reparsePointProvider, AlternativeFileTree alternativeFileTree, IPathService pathService)
        {
            _reparsePointProvider = reparsePointProvider;
            _alternativeFileTree = alternativeFileTree;
            _pathService = pathService;
        }

        public void Create()
        {
            var alternativeFileTreeDirectoryPath = _pathService.GetAlternativeFileTreeDirectoryPath();

            EnsureDirectoryCorrectness(alternativeFileTreeDirectoryPath);

            CreateInternal(alternativeFileTreeDirectoryPath);
        }

        private void EnsureDirectoryCorrectness(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            var files = Directory.EnumerateFiles(directoryPath).ToList();

            if (files.Count > 0)
            {
                throw new ArgumentException("There are files in alternative file tree directory");
            }

            var componentDirectories = Directory.EnumerateDirectories(directoryPath).ToList();

            if (componentDirectories.Count == 0)
            {
                return;
            }

            foreach (var componentDirectory in componentDirectories)
            {
                files = Directory.EnumerateFiles(componentDirectory).ToList();

                if (files.Count > 0)
                {
                    throw new ArgumentException($"There are files in {componentDirectory}");
                }

                var directoryJunctions = Directory.EnumerateDirectories(componentDirectory);

                foreach (var directoryJunction in directoryJunctions)
                {
                    var reparseLinkType = _reparsePointProvider.GetLinkType(directoryJunction);

                    if (reparseLinkType != LinkType.Junction)
                    {
                        throw new ArgumentException($"Directory {directoryJunction} is not a junction");
                    }
                }
            }
        }

        private void CreateInternal(string directoryPath)
        {
            foreach (var componentName in _pathService.EnumerateComponents())
            {
                var componentDirectory = $"{directoryPath}{componentName}";
            }

            throw new NotImplementedException();
        }
    }
}
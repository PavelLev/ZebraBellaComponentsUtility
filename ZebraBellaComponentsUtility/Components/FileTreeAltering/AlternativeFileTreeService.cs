using System;
using System.Linq;
using NCode.ReparsePoints;
using ZebraBellaComponentsUtility.Utility;

namespace ZebraBellaComponentsUtility.Components.FileTreeAltering
{
    public class AlternativeFileTreeService : IAlternativeFileTreeService
    {
        private readonly IReparsePointProvider _reparsePointProvider;
        private readonly AlternativeFileTree _alternativeFileTree;
        private readonly IPathService _pathService;
        private readonly IDirectoryService _directoryService;
        private readonly IPathEqualityComparer _pathEqualityComparer;
        private readonly IFileService _fileService;

        public AlternativeFileTreeService
        (
            IReparsePointProvider reparsePointProvider,
            AlternativeFileTree alternativeFileTree, 
            IPathService pathService,
            IDirectoryService directoryService,
            IPathEqualityComparer pathEqualityComparer,
            IFileService fileService
        )
        {
            _reparsePointProvider = reparsePointProvider;
            _alternativeFileTree = alternativeFileTree;
            _pathService = pathService;
            _directoryService = directoryService;
            _pathEqualityComparer = pathEqualityComparer;
            _fileService = fileService;
        }

        public void Create()
        {
            var alternativeFileTreeDirectoryPath = _pathService.GetAlternativeFileTreeDirectoryPath();

            if (!_directoryService.Exists(alternativeFileTreeDirectoryPath))
            {
                _directoryService.CreateDirectory(alternativeFileTreeDirectoryPath);
            }

            var files = _directoryService.EnumerateFiles(alternativeFileTreeDirectoryPath).ToList();

            if (files.Count > 0)
            {
                throw new ArgumentException("There are files in alternative file tree directory");
            }

            var presentComponentPaths = _directoryService.EnumerateDirectories(alternativeFileTreeDirectoryPath).ToList();

            var requiredComponents = _pathService.EnumerateComponents()
                .Select(componentName => 
                    (
                        Name: componentName, 
                        AlternativeFileTreeDirectoryPath: _pathService.GetChildDirectoryPath(alternativeFileTreeDirectoryPath, componentName)
                    )
                )
                .ToList();

            var excessComponentPaths = presentComponentPaths.Except
                (
                    requiredComponents.Select
                    (
                        component =>
                            _pathService.GetChildDirectoryPath
                            (
                                alternativeFileTreeDirectoryPath, 
                                component.AlternativeFileTreeDirectoryPath
                            )
                    ),
                    _pathEqualityComparer
                );

            foreach (var excessComponentPath in excessComponentPaths)
            {
                _directoryService.Delete(excessComponentPath, true);
            }


            foreach (var component in requiredComponents)
            {
                CreateComponentAlternativeFileTreeDirectory(component);
            }


            var gitExcludePath = _pathService.GetGitExcludePath();

            var excludeAlternativeFileTreeLine = _pathService.GetGitExcludeAlternativeFileTreeLine();

            var allExcludeLines = _fileService.ReadLines(gitExcludePath);

            if (!allExcludeLines.Any(excludeLine =>
                _pathEqualityComparer.Equals(excludeLine, excludeAlternativeFileTreeLine)))
            {
                _fileService.AppendAllLines(gitExcludePath, new[] {excludeAlternativeFileTreeLine});
            }
        }

        private void CreateComponentAlternativeFileTreeDirectory((string Name, string AlternativeFileTreeDirectoryPath) component)
        {
            if (!_directoryService.Exists(component.AlternativeFileTreeDirectoryPath))
            {
                _directoryService.CreateDirectory(component.AlternativeFileTreeDirectoryPath);
            }


            var requiredJunctions = _alternativeFileTree.DirectoryJunctions;


            var presentJunctions = _directoryService.EnumerateDirectories(component.AlternativeFileTreeDirectoryPath)
                .ToList();

            var excessJunctions = presentJunctions.Except(presentJunctions).ToList();

            if (excessJunctions.Count > 0)
            {
                throw new InvalidOperationException($"Alternative file tree directory of component contains excess directory {excessJunctions.First()}");
            }


            foreach (var requiredJunction in requiredJunctions)
            {
                var requiredJunctionPath = _pathService.GetChildDirectoryPath
                    (
                        component.AlternativeFileTreeDirectoryPath,
                        requiredJunction.Name
                    );

                var requiredJunctionTarget = string.Format
                    (
                        _pathService.GetRepositoryAbsolutePath
                        (
                            requiredJunction.RepositoryRelativePath
                        ),
                        component.Name
                    );


                if (_directoryService.Exists(requiredJunctionPath))
                {
                    var reparseLink = _reparsePointProvider.GetLink(requiredJunctionPath);

                    if (reparseLink.Type != LinkType.Junction)
                    {
                        throw new InvalidOperationException($"Alternative file tree directory of component contains non junction directory {excessJunctions.First()}");
                    }


                    if (_pathEqualityComparer.Equals(reparseLink.Target, requiredJunctionTarget))
                    {
                        continue;
                    }


                    _directoryService.Delete(requiredJunctionPath);
                }


                _reparsePointProvider.CreateLink(requiredJunctionPath, requiredJunctionTarget, LinkType.Junction);
            }
        }
    }
}
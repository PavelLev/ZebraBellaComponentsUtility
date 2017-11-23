using System;
using System.IO;
using System.Linq;
using NCode.ReparsePoints;

namespace ZebraBellaComponentsUtility.Components
{
    public class AlternativeFileTreeService : IAlternativeFileTreeService
    {
        private readonly IReparsePointProvider _reparsePointProvider;

        public AlternativeFileTreeService(IReparsePointProvider reparsePointProvider)
        {
            _reparsePointProvider = reparsePointProvider;
        }

        public void Create()
        {
            try
            {
                //TODO 

                //EnsureDirectoryCorrectness();
            }
            catch 
            {
                
            }

            throw new System.NotImplementedException();
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
    }
}
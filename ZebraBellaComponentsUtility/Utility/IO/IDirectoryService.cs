using System;
using System.Collections.Generic;
using System.IO;

namespace ZebraBellaComponentsUtility.Utility
{
    public interface IDirectoryService
    {
        DirectoryInfo CreateDirectory(string path);
        void Delete(string path, bool recursive = false);
        IEnumerable<string> EnumerateDirectories(string path);
        IEnumerable<string> EnumerateFiles(string path);
        IEnumerable<string> EnumerateFileSystemEntries(string path);
        bool Exists(string path);
    }
}
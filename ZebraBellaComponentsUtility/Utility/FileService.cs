using System.Collections.Generic;
using System.IO;

namespace ZebraBellaComponentsUtility.Utility
{
    public class FileService : IFileService
    {
        public void AppendAllLines(string path, IEnumerable<string> contents)
        {
            File.AppendAllLines(path, contents);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public IEnumerable<string> ReadLines(string path)
        {
            return File.ReadLines(path);
        }
    }
}
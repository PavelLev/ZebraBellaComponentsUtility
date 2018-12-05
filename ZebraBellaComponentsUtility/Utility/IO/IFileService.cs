using System.Collections.Generic;

namespace ZebraBellaComponentsUtility.Utility
{
    public interface IFileService
    {
        void AppendAllLines(string path, IEnumerable<string> contents);
        void Delete(string path);
        bool Exists(string path);
        IEnumerable<string> ReadLines(string path);
        void WriteAllText(string path, string contents);
    }
}
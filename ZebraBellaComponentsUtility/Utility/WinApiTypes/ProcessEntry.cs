using System;
using System.Runtime.InteropServices;

namespace ZebraBellaComponentsUtility.Utility.WinApiTypes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessEntry
    {
        public uint StructureSize;
        public uint cntUsage;
        public int ProcessId;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public int ParentProcessId;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string ExecutableFileName;
    }
}
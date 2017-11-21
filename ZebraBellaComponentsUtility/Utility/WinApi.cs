using System;
using System.Runtime.InteropServices;
using ZebraBellaComponentsUtility.Utility.WinApiTypes;

namespace ZebraBellaComponentsUtility.Utility
{
    public class WinApi : IWinApi
    {
        bool IWinApi.SetForegroundWindow(IntPtr hWnd)
        {
            return SetForegroundWindow(hWnd);
        }

        IntPtr IWinApi.CreateToolhelp32Snapshot(SnapshotType dwFlags, uint processId)
        {
            return CreateToolhelp32Snapshot(dwFlags, processId);
        }

        bool IWinApi.Process32First(IntPtr snapshotHandle, ref ProcessEntry processEntry)
        {
            return Process32First(snapshotHandle, ref processEntry);
        }

        bool IWinApi.Process32Next(IntPtr snapshotHandle, ref ProcessEntry processEntry)
        {
            return Process32Next(snapshotHandle, ref processEntry);
        }

        bool IWinApi.PostMessage(IntPtr hWnd, WindowsMessageType Msg, int wParam, int lParam)
        {
            return PostMessage(hWnd, Msg, wParam, lParam);
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(SnapshotType dwFlags, uint processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Process32First(IntPtr snapshotHandle, ref ProcessEntry processEntry);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Process32Next(IntPtr snapshotHandle, ref ProcessEntry processEntry);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, WindowsMessageType Msg, int wParam, int lParam);
    }
}
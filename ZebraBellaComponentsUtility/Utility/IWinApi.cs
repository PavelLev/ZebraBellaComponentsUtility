using System;
using ZebraBellaComponentsUtility.Utility.WinApiTypes;

namespace ZebraBellaComponentsUtility.Utility
{
    public interface IWinApi
    {
        bool SetForegroundWindow(IntPtr hWnd);

        IntPtr CreateToolhelp32Snapshot(SnapshotType dwFlags, uint processId);
        bool Process32First(IntPtr snapshotHandle, ref ProcessEntry processEntry);
        bool Process32Next(IntPtr snapshotHandle, ref ProcessEntry processEntry);
        bool PostMessage(IntPtr hWnd, WindowsMessageType Msg, int wParam, int lParam);
    }
}
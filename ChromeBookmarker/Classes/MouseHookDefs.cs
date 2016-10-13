
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;

namespace MouseGlobalHook
{
    public delegate IntPtr LLMouseCallbackProcedure(int nCode, IntPtr wParam, IntPtr lParam);
    public delegate void LLMouseEvent(int nCode, IntPtr wParam, IntPtr lParam);
    public enum MouseMessages

    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

}
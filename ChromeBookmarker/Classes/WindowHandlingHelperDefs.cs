using System;
using System.Runtime.InteropServices;

namespace WindowHelper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
    public static class WindowsMessagingConstants
    {
        public const UInt32 WM_CLOSE = 0x0010;
    }

    public static class GDIConstants
    {
        public const int SRCCOPY = 0x00CC0020;
    }
}
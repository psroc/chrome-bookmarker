using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WindowHelper
{
    static class WindowHandlingHelper
    {
        /// <summary>
        /// Get window caption using winapi
        /// </summary>
        /// <param name="hwnd">handle of window</param>
        /// <returns></returns>
        public static String GetWindowCaption(IntPtr hwnd)
        {
            StringBuilder windowText = new StringBuilder(256);

            int nResult = GetWindowText(hwnd, windowText, windowText.Capacity);

            if (nResult != 0)
            {
                return windowText.ToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// Get window class using winapi
        /// </summary>
        /// <param name="hwnd">handle of window</param>
        /// <returns></returns>
        public static String GetWindowClassName(IntPtr hwnd)
        {
            StringBuilder className = new StringBuilder(256);
            
            int nResult = GetClassName(hwnd, className, className.Capacity);

            if (nResult != 0)
            {
                return className.ToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// Get handle of topmost window under point
        /// </summary>
        /// <param name="point">coordinates of window</param>
        /// <returns></returns>
        public static IntPtr GetWindowUnderPoint(Point point)
        {
            return WindowFromPoint(point);
        }


        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point point);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);
        [DllImport("User32.dll")]
        private static extern IntPtr GetParent(IntPtr hwnd);
        [DllImport("User32.dll")]
        private static extern IntPtr ChildWindowFromPoint(IntPtr hWndParent, Point point);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int  nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.Dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool CloseWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hDestObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }
}

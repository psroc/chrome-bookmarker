using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseGlobalHook
{
    public static class MouseHook

    { 
        private static IntPtr _MouseHook = IntPtr.Zero;
        private static LLMouseCallbackProcedure CallbackProc;

        public static event LLMouseEvent OnGlobalMouseEvent;
        public static event LLMouseEvent OnLeftMouseDown;
        public static event LLMouseEvent OnLeftMouseUp;
        public static event LLMouseEvent OnRightMouseDown;
        public static event LLMouseEvent OnRightMouseUp;
        public static event LLMouseEvent OnMouseMove;
        public static event LLMouseEvent OnMouseWheel;

        /// <summary>
        /// Procedure to set global mouse hook
        /// </summary>
        /// <param name="proc">Callback procedure to be called</param>
        /// <returns>Hook id, used to remove hook</returns>
        public static IntPtr SetHook()

        {

            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)

                {
                    //callback proc needs to be local static or garbage collector will collect local delegate and then we get an access violation
                    CallbackProc = new LLMouseCallbackProcedure(HookCallbackFunction);
                    _MouseHook = SetWindowsHookEx(WH_MOUSE_LL, CallbackProc, GetModuleHandle(curModule.ModuleName), 0);
                    return _MouseHook;
                }
            }

        }

        /// <summary>
        /// Removes global mouse hook
        /// </summary>
        /// <param name="proc"></param>
        public static void RemoveHook()
        {
            UnhookWindowsHookEx(_MouseHook);
        }
        /// <summary>
        /// Callback function to be called from global mouse handler
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static IntPtr HookCallbackFunction(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //call global event handler
            if (OnGlobalMouseEvent != null)
                OnGlobalMouseEvent(nCode, wParam, lParam);

            //call all possible event handlers
            if (nCode >= 0)
            {
                switch ((MouseMessages)wParam)
                {
                    case MouseMessages.WM_LBUTTONDOWN:
                        if (OnLeftMouseDown != null)
                            OnLeftMouseDown(nCode, wParam, lParam);
                        break;
                    case MouseMessages.WM_LBUTTONUP:
                        if (OnLeftMouseUp != null)
                            OnLeftMouseUp(nCode, wParam, lParam);
                        break;
                    case MouseMessages.WM_RBUTTONDOWN:
                        if (OnRightMouseDown != null)
                            OnRightMouseDown(nCode, wParam, lParam); 
                        break;
                    case MouseMessages.WM_RBUTTONUP:
                        if (OnRightMouseUp != null)
                            OnRightMouseUp(nCode, wParam, lParam);
                        break;
                    case MouseMessages.WM_MOUSEMOVE:
                        if (OnMouseMove != null)
                            OnMouseMove(nCode, wParam, lParam);
                        break;
                    case MouseMessages.WM_MOUSEWHEEL:
                        if (OnMouseWheel != null)
                            OnMouseWheel(nCode, wParam, lParam);
                        break;
                }
            }

            //call next hook in line
            return CallNextHookEx(_MouseHook, nCode, wParam, lParam);
        }


        private const int WH_MOUSE_LL = 14;


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr SetWindowsHookEx(int idHook,

            LLMouseCallbackProcedure lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        [return: MarshalAs(UnmanagedType.Bool)]

        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,

            IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern IntPtr GetModuleHandle(string lpModuleName);

    }
}

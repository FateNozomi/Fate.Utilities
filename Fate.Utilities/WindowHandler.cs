using System;
using System.Runtime.InteropServices;

namespace Fate.Utilities
{
    public static class WindowHandler
    {
        public static void BringToFront(string windowName)
        {
            IntPtr handle = FindWindow(null, windowName);

            if (handle != IntPtr.Zero)
            {
                SetForegroundWindow(handle);
            }
        }

        public static void BringToFront(IntPtr windowHandle)
        {
            SetForegroundWindow(windowHandle);
        }

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}

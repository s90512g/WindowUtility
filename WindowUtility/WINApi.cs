using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowUtility
{
    class WINApi
    {
        private const string DllName = "user32.dll";
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport(DllName)]
        public static extern void SwitchToThisWindow(IntPtr hwnd, bool bRestore);

        [DllImport(DllName)]
        public static extern int EnumWindows(EnumWindowsProc hWnd, IntPtr lParam);

        [DllImport(DllName, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpText, int nCount);

        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport(DllName)]
        public static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

        [DllImport(DllName)]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);
    }
}

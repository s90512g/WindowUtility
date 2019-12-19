using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;

namespace WindowUtility
{
    class WindowFinder
    {
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        private List<WindowInfo> ReturnList = new List<WindowInfo>();
        private WindowIcon GetWindowIcon = new WindowIcon();

        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsProc hWnd, IntPtr lParam);
        public List<WindowInfo> GetWindow()
        {
            ReturnList.Clear();
            EnumWindows(EnumWindowProc, IntPtr.Zero);
            return ReturnList;
        }

        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);
        private bool EnumWindowProc(IntPtr hWnd, IntPtr lParam)
        {
            IntPtr pHwnd = GetParent(hWnd);
            bool windowAttribute;
            DwmGetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.Cloaked, out windowAttribute, Marshal.SizeOf(typeof(bool)));
            if (IsWindowVisible(hWnd) && pHwnd == IntPtr.Zero)
            {
                if (!windowAttribute)
                {
                    PrintWindowInfo(hWnd);
                }
            }
            return true;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpText, int nCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        private void PrintWindowInfo(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            StringBuilder title = new StringBuilder((length + 1));
            GetWindowText(hWnd, title, title.Capacity);
            WindowInfo Info = new WindowInfo();
            Info.Name = title.ToString();
            Info.HWnd = hWnd;
            BitmapSource icon;
            icon = GetWindowIcon.GetIcon(hWnd);
            if (icon != null)
            {
                Info.Icon = icon;
                icon.Freeze();
            }
            ReturnList.Add(Info);
        }
    }
}

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
        private List<WindowData> ReturnList = new List<WindowData>();
        private WindowIcon GetWindowIcon = new WindowIcon();

        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsProc hWnd, IntPtr lParam);
        public List<WindowData> GetWindow()
        {
            ReturnList.Clear();
            EnumWindows(EnumWindowProc, IntPtr.Zero);
            return ReturnList;
        }

        [DllImport("user32.dll")]
        static extern bool IsChild(IntPtr hWndParent, IntPtr hWnd);

        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hwnd);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);
        private bool EnumWindowProc(IntPtr hWnd, IntPtr lParam)
        {
            bool windowCloakAttribute;
            DwmGetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.Cloaked, out windowCloakAttribute, Marshal.SizeOf(typeof(bool)));
            WINDOWINFO info = new WINDOWINFO();
            GetWindowInfo(hWnd, ref info);
            if (IsWindowVisible(hWnd))
            {
                if (!windowCloakAttribute)
                {
                    if ((info.dwStyle & 0x80000000) != 0x80000000 && (info.dwExStyle & 0x08000000) != 0x08000000)
                    {
                        PrintWindowInfo(hWnd);
                    }

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
            WindowData Info = new WindowData();
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

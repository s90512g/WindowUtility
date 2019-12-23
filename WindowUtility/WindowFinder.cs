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
        private List<WindowData> ReturnList = new List<WindowData>();
        private WindowIcon GetWindowIcon = new WindowIcon();

        public List<WindowData> GetWindow()
        {
            ReturnList.Clear();
            WINApi.EnumWindows(EnumWindowProc, IntPtr.Zero);
            return ReturnList;
        }

        private bool EnumWindowProc(IntPtr hWnd, IntPtr lParam)
        {
            bool windowCloakAttribute;
            DWMApi.DwmGetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.Cloaked, out windowCloakAttribute, Marshal.SizeOf(typeof(bool)));
            WINDOWINFO info = new WINDOWINFO();
            WINApi.GetWindowInfo(hWnd, ref info);
            if (WINApi.IsWindowVisible(hWnd))
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

        private void PrintWindowInfo(IntPtr hWnd)
        {
            int length = WINApi.GetWindowTextLength(hWnd);
            StringBuilder title = new StringBuilder((length + 1));
            WINApi.GetWindowText(hWnd, title, title.Capacity);
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

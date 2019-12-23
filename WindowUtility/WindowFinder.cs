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
                    PrintWindowInfo(hWnd);
                }
            }
            return true;
        }

        private void PrintWindowInfo(IntPtr hWnd)
        {
            int length = WINApi.GetWindowTextLength(hWnd);
            StringBuilder title;
            WindowData Info = new WindowData();
            try
            {
                title = new StringBuilder((length + 1));
                WINApi.GetWindowText(hWnd, title, title.Capacity);
                Info.Name = title.ToString();

            }
            catch (ArgumentOutOfRangeException) { Info.Name = ""; }

            Info.HWnd = hWnd;
            BitmapSource icon;
            icon = GetWindowIcon.GetIcon(hWnd);
            if (icon != null)
            {
                Info.Icon = icon;
                try
                {
                    icon.Freeze();
                }
                catch (InvalidOperationException) { Info.Icon = null; }
            }
            ReturnList.Add(Info);
        }
    }
}

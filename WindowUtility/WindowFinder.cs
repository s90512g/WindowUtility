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
        private List<WindowInfo> ReturnList = new List<WindowInfo>();
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsProc hWnd, IntPtr lParam);
        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpText, int nCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        private WindowIcon GetWindowIcon = new WindowIcon();
        public List<WindowInfo> GetWindow()
        {
            ReturnList.Clear();
            EnumWindows(PrintWindowInfo, IntPtr.Zero);
            return ReturnList;
        }
        private bool PrintWindowInfo(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd))
            {
                int length = GetWindowTextLength(hWnd);
                StringBuilder sb = new StringBuilder((length + 1));
                GetWindowText(hWnd, sb, sb.Capacity);
                if (sb.ToString() != String.Empty)
                {
                    WindowInfo Info = new WindowInfo();
                    Info.Name = sb.ToString();
                    Info.HWnd = hWnd;
                    var icon = GetWindowIcon.GetIcon(hWnd);
                    if (icon != null)
                    {
                        Info.Icon = icon;
                        icon.Freeze();
                    }
                    ReturnList.Add(Info);
                }
            }
            return true;
        }
    }
}

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
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
        private readonly int WS_EX_TOOLWINDOW = 0x0080;
        //private readonly int WS_EX_APPWINDOW = 0x40000;
        private readonly int WS_CHILD = 0x40000000;

        private WindowIcon GetWindowIcon = new WindowIcon();

        public enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        public List<WindowInfo> GetWindow()
        {
            ReturnList.Clear();
            EnumWindows(EnumWindowProc, IntPtr.Zero);
            return ReturnList;
        }
        private bool EnumWindowProc(IntPtr hWnd, IntPtr lParam)
        {
            if (IsWindowVisible(hWnd) && (GetWindowLongPtr(hWnd,(int)GWL.GWL_STYLE).ToInt32()& WS_CHILD) != WS_CHILD)
            {
                if ((GetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE).ToInt32() & WS_EX_TOOLWINDOW) != WS_EX_TOOLWINDOW)
                {
                    PrintWindowInfo(hWnd);
                }
                //else if ((GetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE).ToInt32() & WS_EX_APPWINDOW) != WS_EX_APPWINDOW)
                //{
                //   // PrintWindowInfo(hWnd);
                //}
            }
            return true;
        }
        private void PrintWindowInfo(IntPtr hWnd)
        {
            GetWindowLongPtr(hWnd, (int)GWL.GWL_EXSTYLE);
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder((length + 1));
            GetWindowText(hWnd, sb, sb.Capacity);
            //if (sb.ToString() != String.Empty)
            //{
            WindowInfo Info = new WindowInfo();
            Info.Name = sb.ToString();
            Info.HWnd = hWnd;
            BitmapSource icon;
            //if (IsImmersiveProcess(hWnd))
            //{
            //    icon = null;
            //}
            //else
            //{
            icon = GetWindowIcon.GetIcon(hWnd);
            //}

            if (icon != null)
            {
                Info.Icon = icon;
                icon.Freeze();
            }
            ReturnList.Add(Info);
        }
    }
}

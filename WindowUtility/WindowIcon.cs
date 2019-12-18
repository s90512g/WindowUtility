using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WindowUtility
{
    class WindowIcon
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg,
    int wParam, IntPtr lParam, SendMessageTimeoutFlags flags,
    uint timeout, out IntPtr result);
        private enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }
        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        private static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);
        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);
        private readonly int GCL_HICON = -14;
        private readonly uint WM_GETICON = 0x007f;
        private readonly int IDI_APPLICATION = 0x7F00;
        private readonly int ICON_BIG = 1;
        private static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size > 4)
                return GetClassLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
        }
        public BitmapSource GetIcon(IntPtr hwnd)
        {
            IntPtr hIcon = IntPtr.Zero;
            hIcon = GetClassLongPtr(hwnd, GCL_HICON);
            if (hIcon == IntPtr.Zero)
            {
                SendMessageTimeout(hwnd, WM_GETICON, ICON_BIG, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_ERRORONEXIT, 1000, out hIcon);
            }
            if (hIcon == IntPtr.Zero)
            {
                hIcon = LoadIcon(IntPtr.Zero, (IntPtr)IDI_APPLICATION);
            }
            if (hIcon != IntPtr.Zero)
            {
                return Imaging.CreateBitmapSourceFromHIcon(hIcon, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                return null;
            }
        }
    }
}

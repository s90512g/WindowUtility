using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WindowUtility
{
    class WindowContent
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        private static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        private static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        private static extern bool DeleteDC([In] IntPtr hdc);

        public BitmapSource GetContent(IntPtr hwnd)
        {
            var hdc = GetWindowDC(hwnd);
            RECT rect = new RECT();
            GetWindowRect(hwnd, out rect);
            int width = Math.Abs(rect.Right - rect.Left);
            int height = Math.Abs(rect.Bottom - rect.Top);
            IntPtr hbitmap = CreateCompatibleBitmap(hdc, width, height);
            IntPtr hmemdc = CreateCompatibleDC(hdc);
            SelectObject(hmemdc, hbitmap);
            var ans = PrintWindow(hwnd, hmemdc, 0x00000002);
            if (ans)
            {
                try
                {
                    BitmapSource img = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    img.Freeze();
                    return img;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    DeleteDC(hdc);
                    DeleteDC(hmemdc);
                }
            }
            DeleteDC(hdc);
            DeleteDC(hmemdc);
            return null;
        }
    }

}

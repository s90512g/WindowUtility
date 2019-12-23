using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowUtility
{
    class DWMApi
    {
        private const string DllName = "dwmapi.dll";

        public const int DWM_TNP_RECTDESTINATION = 0x00000001;
        public const int DWM_TNP_VISIBLE = 0x00000008;
        public const int DWM_TNP_OPACITY = 0x00000004;

        [DllImport(DllName)]
        public static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport(DllName)]
        public static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport(DllName)]
        public static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        [DllImport(DllName)]
        public static extern int DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out Size size);

        [DllImport(DllName)]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);
    }
}

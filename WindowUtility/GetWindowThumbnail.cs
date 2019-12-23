using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowUtility
{
    class GetWindowThumbnail
    {
        public IntPtr _thumb;
        public void GetThumbnail(IntPtr selfHwnd, IntPtr targethwnd, Rect renderRect)
        {
            if (_thumb != IntPtr.Zero)
            {
                DWMApi.DwmUnregisterThumbnail(_thumb);
            }
            int ans = DWMApi.DwmRegisterThumbnail(selfHwnd, targethwnd, out _thumb);
            UpdateThumbnail(renderRect);
        }

        private void UpdateThumbnail(Rect targetRect)
        {
            if (_thumb != IntPtr.Zero)
            {
                DWMApi.DwmQueryThumbnailSourceSize(_thumb, out Size size);
                DWM_THUMBNAIL_PROPERTIES dskThumbProps = new DWM_THUMBNAIL_PROPERTIES();

                dskThumbProps.dwFlags = DWMApi.DWM_TNP_RECTDESTINATION | DWMApi.DWM_TNP_VISIBLE | DWMApi.DWM_TNP_OPACITY;
                dskThumbProps.fVisible = true;
                dskThumbProps.opacity = 255;
                dskThumbProps.rcDestination = targetRect;
                if (size.Width < targetRect.Width)
                    dskThumbProps.rcDestination.Right = dskThumbProps.rcDestination.Left + size.Width;

                if (size.Height < targetRect.Height)
                    dskThumbProps.rcDestination.Bottom = dskThumbProps.rcDestination.Top + size.Height;
                var hr = DWMApi.DwmUpdateThumbnailProperties(_thumb, ref dskThumbProps);
            }

        }
    }
}

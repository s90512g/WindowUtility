using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WindowUtility
{
    public delegate void ImageUpdateHandler(ImageUpdateEventArgs e);
    public class ImageUpdateEventArgs : EventArgs
    {
        private BitmapSource _bitmap;
        public ImageUpdateEventArgs(BitmapSource bitmap)
        {
            _bitmap = bitmap;
        }
        public BitmapSource PreViewImage
        {
            get { return _bitmap; }
        }
    }
}

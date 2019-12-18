using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace WindowUtility
{
    public struct WindowInfo
    {
        public IntPtr HWnd { get; set; }
        public string Name { get; set; }
        public BitmapSource Icon { get; set; }
    }
}

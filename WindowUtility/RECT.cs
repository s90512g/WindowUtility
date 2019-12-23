using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowUtility
{
    public struct Rect
    {
        public int Left, Top, Right, Bottom;
        public int Width
        {
            get { return (Right - Left); }
        }
        public int Height
        {
            get { return (Bottom - Top); }
        }
    }
    

}

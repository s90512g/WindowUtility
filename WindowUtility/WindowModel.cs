using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace WindowUtility
{
    class WindowModel
    {
        public WindowModel()
        {
            WindowList = new ObservableCollection<WindowInfo>();
        }
        public ObservableCollection<WindowInfo> WindowList { get; private set; }
    }
}

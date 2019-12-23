using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace WindowUtility
{
    class WindowModel
    {
        public WindowModel()
        {
            WindowList = new ObservableCollection<WindowData>();          
        }
        public ObservableCollection<WindowData> WindowList { get; private set; }        
    }
}

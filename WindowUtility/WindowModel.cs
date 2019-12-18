using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace WindowUtility
{
    class WindowModel : INotifyPropertyChanged
    {
        private BitmapSource _PreviewImage;
        public event PropertyChangedEventHandler PropertyChanged;
        public WindowModel()
        {
            WindowList = new ObservableCollection<WindowInfo>();
        }
        public ObservableCollection<WindowInfo> WindowList { get; private set; }
        public BitmapSource PreviewImage
        {
            get { return _PreviewImage; }
            set
            {
                if (!Equals(_PreviewImage, value))
                {
                    _PreviewImage = value;
                    NotifyPropertyChanged("PreviewImage");
                }
            }
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

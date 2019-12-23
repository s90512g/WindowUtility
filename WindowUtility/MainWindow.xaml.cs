using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowUtility
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowControl controller = new WindowControl();
        private WindowData MouseTmp = new WindowData();
        WindowInteropHelper _wih;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = controller;
            controller.WinFindWorker();
            _wih = new WindowInteropHelper(this);
        }
        private void ExBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowListView.SelectedItem is WindowData selectItem)
            {
                controller.ShowWindow(selectItem.HWnd);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.StopWinFindWorker();
        }

        private void ListViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            var point = PreviewImgae.TranslatePoint(new Point(0, 0), this);
            var item = sender as ListViewItem;
            Rect renderRect = new Rect();
            renderRect.Top = (int)point.Y;
            renderRect.Bottom = (int)point.Y + (int)PreviewImgae.ActualHeight;
            renderRect.Left = (int)point.X;
            renderRect.Right = (int)point.X + (int)PreviewImgae.ActualWidth;
            if (item.Content is WindowData tmp)
            {
                if (!Equals(tmp, MouseTmp))
                {
                    MouseTmp = tmp;
                    controller.ShowPreView(_wih.Handle,MouseTmp.HWnd,renderRect);
                }
            }
        }
    }
}

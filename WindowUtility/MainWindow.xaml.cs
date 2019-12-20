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
        private Controller controller = new Controller();
        private WindowData MouseTmp = new WindowData();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = controller;
            controller.GetWindowWorker();
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

        }

        private void ListViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item.Content is WindowData tmp)
            {
                if (!Equals(tmp, MouseTmp))
                {
                    MouseTmp = tmp;
                    controller.GetPreView(MouseTmp.HWnd);
                }
            }
        }
    }
}

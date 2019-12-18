using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace WindowUtility
{
    public class ItemObservableCollection<T> : ObservableCollection<T>
    {
        public void Replace(int index, T item)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                 {
                     SetItem(index, item);
                 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

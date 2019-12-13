using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;
namespace WindowUtility
{
    class Controller
    {
        public WindowModel Model = new WindowModel();
        private WindowFinder FindWindow = new WindowFinder();
        private Thread GetWindowThread;
        public void GetWindowWorker()
        {
            GetWindowThread = new Thread(() =>
            {
                List<WindowInfo> newWindowList = FindWindow.GetWindow();
                List<WindowInfo> oldWindowList = new List<WindowInfo>();
                oldWindowList = Model.WindowList.ToList();
                Console.WriteLine(newWindowList.Count);
                var addList = newWindowList.Except(oldWindowList);
                var removeList = oldWindowList.Except(newWindowList);
                foreach (var item in addList)
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Model.WindowList.Add(item);
                    });
                }
                foreach (var item in removeList)
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Model.WindowList.Remove(item);
                    });
                }
                Console.WriteLine("Thread Finish");
            });
            Console.WriteLine("Thread Working");
            GetWindowThread.Start();

        }
    }
}

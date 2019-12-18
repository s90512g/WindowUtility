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
        private WindowFinder FindWindow = new WindowFinder();
        private WindowContent GetWindowContent = new WindowContent();
        private Thread GetWindowThread;
        public WindowModel Model { get; } = new WindowModel();
        public void GetWindowWorker()
        {
            GetWindowThread = new Thread(() =>
            {
                while (true)
                {
                    var newList = FindWindow.GetWindow();
                    ModelUpdate(newList);
                    Thread.Sleep(1);
                }
            })
            { IsBackground = true };
            Console.WriteLine("Thread Working");
            GetWindowThread.Start();
        }

        public void GetPreView(IntPtr hwnd)
        {
            lock (GetWindowContent)
            {
                Thread preThread = new Thread(() =>
                {
                    Console.WriteLine("start");
                    var img = GetWindowContent.GetContent(hwnd);
                    Model.PreviewImage = img;
                });
                preThread.Start();
            }
        }

        [DllImport("user32")]
        static extern void SwitchToThisWindow(IntPtr hwnd, bool bRestore);
        public void ShowWindow(IntPtr hwnd)
        {
            SwitchToThisWindow(hwnd, true);
        }

        private void ModelUpdate(List<WindowInfo> newWindowList)
        {
            List<string> oldHWNDList = new List<string>();
            foreach (var window in Model.WindowList)
            {
                oldHWNDList.Add(window.HWnd.ToString());
            }
            List<string> newHWNDList = new List<string>();
            foreach (var newWindowItem in newWindowList)
            {
                if (oldHWNDList.Contains(newWindowItem.HWnd.ToString()))
                {
                    foreach (var oldWindowItem in Model.WindowList)
                    {
                        if (Equals(oldWindowItem.HWnd.ToString(), newWindowItem.HWnd.ToString()) && !Equals(oldWindowItem.Name, newWindowItem.Name))
                        {
                            var index = Model.WindowList.IndexOf(oldWindowItem);
                            //WindowInfo newItem = Model.WindowList.ElementAt(index);
                            Model.WindowList.Replace(index, newWindowItem);
                            break;
                        }
                    }
                }
                else
                {
                    AddWindowMember(newWindowItem);
                }
                newHWNDList.Add(newWindowItem.HWnd.ToString());
            }
            var removeList = oldHWNDList.Except(newHWNDList);
            if (removeList.Count() != 0)
            {
                foreach (var removeHWND in removeList)
                {
                    RemoveWindowMember(removeHWND);
                }
            }
        }

        private void RemoveWindowMember(string HWND)
        {
            foreach (var removeWindowItem in Model.WindowList)
            {
                if (Equals(removeWindowItem.HWnd.ToString(), HWND))
                {
                    try
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Model.WindowList.Remove(removeWindowItem);
                        });
                    }
                    catch (NullReferenceException)
                    {

                    }
                    break;
                }
            }
        }

        private void AddWindowMember(WindowInfo AddWindow)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Model.WindowList.Add(AddWindow);
                });
            }
            catch (NullReferenceException)
            {
            }
        }

    }
}

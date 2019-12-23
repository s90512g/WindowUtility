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
        private Thread GetWindowThread;
        public WindowModel Model { get; } = new WindowModel();
        private GetWindowThumbnail GetWindowContent = new GetWindowThumbnail();
        public void GetWindowWorker()
        {
            GetWindowThread = new Thread(() =>
            {
                while (true)
                {
                    var newList = FindWindow.GetWindow();
                    ModelUpdate(newList);
                    Thread.Sleep(100);
                }
            })
            { IsBackground = true };
            Console.WriteLine("Thread Working");
            GetWindowThread.Start();
        }

        public void GetPreView(IntPtr selfHwnd,IntPtr sourcehwnd, Rect renderRect)
        {
            lock (GetWindowContent)
            {
                Thread preThread = new Thread(() =>
                {
                    GetWindowContent.GetThumbnail(selfHwnd, sourcehwnd, renderRect);
                });
                preThread.Start();
            }
        }


        public void ShowWindow(IntPtr hwnd)
        {
            WINApi.SwitchToThisWindow(hwnd, true);
        }

        private void ModelUpdate(List<WindowData> newWindowList)
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
                        if (Equals(oldWindowItem.HWnd.ToString(), newWindowItem.HWnd.ToString()))
                        {
                            var index = Model.WindowList.IndexOf(oldWindowItem);
                            WindowData item = Model.WindowList.ElementAt(index);
                            item.Name = newWindowItem.Name;
                            item.Icon = newWindowItem.Icon;
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
                    catch (NullReferenceException) { }
                    break;
                }
            }
        }

        private void AddWindowMember(WindowData AddWindow)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Model.WindowList.Add(AddWindow);
                });
            }
            catch (NullReferenceException) { }
        }

    }
}

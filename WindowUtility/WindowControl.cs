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
    class WindowControl
    {
        private WindowFinder WinFinder = new WindowFinder();
        private Thread WinFinderThread;
        public WindowModel Model { get; } = new WindowModel();
        private WindowThumbnail WinThumbnail = new WindowThumbnail();
        private bool WinFinderState;
        public void WinFindWorker()
        {
            WinFinderState = true;
            WinFinderThread = new Thread(() =>
            {
                while (WinFinderState)
                {
                    try
                    {
                        var newList = WinFinder.GetWindow();
                        ModelUpdate(newList);
                        Thread.Sleep(2000);
                    }
                    catch (Exception) { }
                }
            })
            { IsBackground = true };
            WinFinderThread.Start();
        }
        public void StopWinFindWorker()
        {
            WinFinderState = false;
            Console.WriteLine(WinFinderThread.ThreadState);
        }
        public void ShowPreView(IntPtr selfHwnd, IntPtr sourcehwnd, Rect renderRect)
        {
            WinThumbnail.PrintThumbnail(selfHwnd, sourcehwnd, renderRect);
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
                            try
                            {
                                WindowData item = Model.WindowList.ElementAt(index);
                                item.Name = newWindowItem.Name;
                                item.Icon = newWindowItem.Icon;
                            }
                            catch (Exception)
                            {

                            }
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

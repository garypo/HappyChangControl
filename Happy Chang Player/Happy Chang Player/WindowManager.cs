using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Happy_Chang_Player
{
    class WindowManager
    {
        private String _processName;
        private int _monitoringWidth=0;
        private int _monitoringHeight=0;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        private List<AppWin> _winList = new List<AppWin>();

        public WindowManager(String processName)
        {
            _processName = processName;
            _winList = new List<AppWin>();
        }

        public int getNumberOfWindows()
        {
            getRunningApps();
            return _winList.Count;
        }

        public AppWin getAppWindow(int index)
        {
            AppWin app = null;
            if (index < _winList.Count)
            {
                app = _winList[index];
            }

            return app;
        }

        public int getMonitoringWidth()
        {
            return _monitoringWidth;
        }

        public int getMonitoringHeight()
        {
            return _monitoringHeight;
        }

        static public List<Process> getRunningProcess(String _pName)
        {
            List<Process> list = new List<Process>();
            Process[] processes = Process.GetProcessesByName(_pName);

            foreach (Process p in processes)
            {
                AppWin w = new AppWin(p.MainWindowHandle);
                if (!w.isHidden())
                {
                    list.Add(p);
                }

            }

            return list;
        }

        public int getRunningApps()
        {
            _winList = new List<AppWin>();

            Process[] processes = Process.GetProcessesByName(_processName);

            foreach (Process p in processes)
            {
                AppWin w = new AppWin(p.MainWindowHandle);
                if (!w.isHidden())
                {
                    _winList.Add(w);
                }

            }
            _winList.Sort(delegate (AppWin w1, AppWin w2)
            {
                int result = 0;

                if (w1.getY() < w2.getY())
                {
                    result = -1;
                }
                else if (w1.getY() > w2.getY())
                {
                    result = 1;
                }
                else if (w1.getX() < w2.getX())
                {
                    result = -1;
                }
                else if (w1.getX() > w2.getX())
                {
                    result = 1;
                }

                return result;
            });
            //_winList.Sort(new WinPosComparer());

            foreach (AppWin w in _winList)
            {
                Console.WriteLine("Got 1 window - (" + w.getX() + ", " + w.getY() + ", " + w.getWidth() + ", " + w.getHeight() + ") ");
            }
            Console.WriteLine("Process Ended.");

            return _winList.Count;
        }

        public int getAppWinHeight()
        {
            int height = 0;
            if (_winList.Count > 0)
            {
                height = _winList[0].getHeight();
            }

            return height;
        }

        public int getAppWinWidth()
        {
            int width = 0;
            if (_winList.Count > 0)
            {
                width = _winList[0].getWidth();
            }

            return width;
        }

        public void autoArrangeByArea(int availableHWidth, int availableHeight, int initX, int initY, int hSep, int vSep)
        {
            int maxWidth = 0;
            int targetNRows = 0;
            int targetNCols = 0;
            int numWins = getNumberOfWindows();
            
            if (numWins > 0)
            {
                for(int numRows = 1; numRows <= numWins; numRows++)
                {
                    int targetHeight = _winList[0].getHeight();
                    int targetWidth = _winList[0].getWidth();

                    //fit height first
                    targetHeight = (availableHeight - (numRows - 1) * vSep) / numRows;
                    targetWidth = _winList[0].getWidth() * targetHeight / _winList[0].getHeight();
                    
                    int numCols = (int) Math.Ceiling((float)numWins / numRows);
                    if (availableHWidth < (numCols * targetWidth + (numCols - 1) * hSep)){
                        targetWidth = (availableHWidth - (numCols - 1) * hSep) / numCols;
                        targetHeight = _winList[0].getHeight() * targetWidth / _winList[0].getWidth();
                    }

                    if (maxWidth < targetWidth)
                    {
                        maxWidth = targetWidth;
                        targetNRows = numRows;
                        targetNCols = numCols;
                    }
                }
                Console.WriteLine("Rows x Cols: (" + targetNRows + ", " + targetNCols + ")");
                Console.WriteLine("No. of Windows: " + numWins);
                int x = initX;
                int y = initY;
                int col = 0;
                int row = 0;
                foreach (AppWin w in _winList)
                {
                    int newWidth = maxWidth;
                    int newHeight = maxWidth * w.getHeight() / w.getWidth();
                    SetWindowPos(w.getWindowhandle(), 0, x, y, newWidth, newHeight, 0);
                    Console.WriteLine("(" + x + ", " + y + ", " + newWidth + ", " + newHeight + ")");
                    col++;
                    if (col < targetNCols)
                    {
                        x += newWidth + hSep;
                    }
                    else
                    {
                        col = 0;
                        row++;
                        x = initX;
                        y += newHeight + vSep;
                    }
                }
                _monitoringHeight = targetNRows * _winList[0].getHeight() + (targetNRows - 1) * vSep;
                _monitoringWidth = targetNCols * _winList[0].getWidth() + (targetNCols - 1) * hSep;
            }
            else
            {
                Console.WriteLine("There is no window.");
            }
        }

        public void autoArrangeByColumns(int nColumns, int hSep, int vSep)
        {
            if (getNumberOfWindows() > 0)
            {
                Rectangle workingSize = Screen.PrimaryScreen.WorkingArea;

                int nRows = (int)Math.Ceiling((decimal)_winList.Count / nColumns);
                int height = _winList[0].getHeight();

                if (nRows * height > workingSize.Height)
                {
                    height = (workingSize.Height - ((nRows - 1) * vSep)) / nRows;
                }

                int x = workingSize.X;
                int y = workingSize.Y;
                int col = 0;
                int row = 0;

                foreach (AppWin w in _winList)
                {
                    int newWidth = height * w.getWidth() / w.getHeight();

                    SetWindowPos(w.getWindowhandle(), 0, x, y, newWidth, height, 0);
                    Console.WriteLine("(" + x + ", " + y + ", " + newWidth + ", " + height + ")");
                    col++;
                    if (col < nColumns)
                    {
                        x += newWidth + hSep;
                    }
                    else
                    {
                        col = 0;
                        row++;
                        x = workingSize.X;
                        y += height + vSep;
                    }
                    if (y + height > workingSize.Y + workingSize.Height)
                    {
                        break;
                    }
                }
            }
        }

        public void autoArrangeByRows(int nRows, int hSep, int vSep)
        {
            if (getNumberOfWindows() > 0)
            {
                Rectangle workingSize = Screen.PrimaryScreen.WorkingArea;

                int height = (workingSize.Height - ((nRows - 1) * vSep)) / nRows;
                int numPerRow = (int)Math.Ceiling((decimal)_winList.Count / nRows);

                int x = workingSize.X;
                int y = workingSize.Y;
                int col = 0;
                int row = 0;

                foreach (AppWin w in _winList)
                {
                    int newWidth = height * w.getWidth() / w.getHeight();

                    SetWindowPos(w.getWindowhandle(), 0, x, y, newWidth, height, 0);
                    Console.WriteLine("(" + x + ", " + y + ", " + newWidth + ", " + height + ")");
                    col++;
                    if (col < numPerRow)
                    {
                        x += newWidth + hSep;
                    }
                    else
                    {
                        col = 0;
                        row++;
                        x = workingSize.X;
                        y += height + vSep;
                    }
                    if (y + height > workingSize.Y + workingSize.Height)
                    {
                        break;
                    }
                }
            }
        }

        public void closeWindow(int winNo)
        {
            if (winNo > 0 && getNumberOfWindows() >= winNo)
            {
                _winList[winNo - 1].close();
            }
        }

        public void closeAll(TimeSpan delay, TimeSpan interdelay)
        {
            if (getNumberOfWindows() > 0)
            {
                System.Threading.Thread.Sleep(delay);
                
                foreach (AppWin w in _winList)
                {
                    w.close();
                    System.Threading.Thread.Sleep(interdelay);
                }
            }
        }
    }

}

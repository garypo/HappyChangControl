using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Happy_Chang_Player
{
    class AppWin
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        const UInt32 WM_CLOSE = 0x0010;
        private IntPtr _windowHandle;

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public AppWin(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
        }

        public IntPtr getWindowhandle()
        {
            return _windowHandle;
        }

        public int getWidth()
        {
            Rect winPos = new Rect();
            GetWindowRect(_windowHandle, ref winPos);
            return (winPos.Right - winPos.Left + 1);
        }

        public int getHeight()
        {
            Rect winPos = new Rect();
            GetWindowRect(_windowHandle, ref winPos);
            return (winPos.Bottom - winPos.Top + 1);
        }

        public int getX()
        {
            Rect winPos = new Rect();
            GetWindowRect(_windowHandle, ref winPos);
            return winPos.Left;
        }

        public int getY()
        {
            Rect winPos = new Rect();
            GetWindowRect(_windowHandle, ref winPos);
            return winPos.Top;
        }

        public Boolean isHidden()
        {
            Rect winPos = new Rect();
            GetWindowRect(_windowHandle, ref winPos);

            return (winPos.Left == 0 && winPos.Top == 0 && winPos.Right == 0 && winPos.Bottom == 0);
        }

        public void setPosition(int x, int y, int width, int height)
        {
            SetWindowPos(_windowHandle, 0, x, y, width, height, 0);
        }

        public void setPosition(int x, int y)
        {
            SetWindowPos(_windowHandle, 0, x, y, getWidth(), getHeight(), 0);
        }

        public void setScale(int width, int height)
        {
            SetWindowPos(_windowHandle, 0, getX(), getY(), width, height, 0);
        }

        public void close()
        {
            SendMessage(_windowHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        public Rectangle getAreaPeopleInRoom()
        {
            Rectangle area = new Rectangle();

            area.Height = getHeight() * 30 / 1326 * 3 / 2;
            area.Width = getWidth() * 65 / 802 * 3 / 2;
            area.X = getX() + 505 * getWidth() / 802 * 3/2;
            area.Y = getY() + 595 * getHeight() / 1326 *3/2;

            return area;
        }
    }

    public class WinPosComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            AppWin w1 = (AppWin)x;
            AppWin w2 = (AppWin)y;

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

        }
    }
}

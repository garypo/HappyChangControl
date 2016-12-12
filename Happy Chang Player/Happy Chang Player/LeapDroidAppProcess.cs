using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Happy_Chang_Player
{
    class LeapDroidAppProcess
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        private Process proc;
        protected float _screenScale;

        protected Point stdAppCaptureSize = new Point(823, 1338);
        protected Point stdAppScreenSize = new Point(820, 1325);
        // measured from bmp captured
        protected int borderLeftCapture = 11;
        protected int borderRightCapture = 12;
        protected int borderTopCapture = 45;
        protected int borderBottomCapture = 13;
        protected int borderLeftOnScreen = 0;
        protected int borderRightOnScreen = 0;
        protected int borderTopOnScreen = 45;
        protected int borderBottomOnScreen = 0;
        // net dimension: (800, 1280)

        // mesaured from content area
        protected Point[,] appsList = new Point[4, 4] {
            { new Point(202, 350), new Point(334, 350), new Point(466, 350), new Point(600, 350) },
            { new Point(202, 500), new Point(334, 500), new Point(466, 500), new Point(600, 500) },
            { new Point(202, 650), new Point(334, 650), new Point(466, 650), new Point(600, 650) },
            { new Point(202, 800), new Point(334, 800), new Point(466, 800), new Point(600, 800) }
            };
        protected Point backKey = new Point(120, 1246);
        protected Point homeKey = new Point(400, 1246);
        protected Point menuKey = new Point(680, 1246);

        public Process Proc
        {
            get
            {
                return proc;
            }

            set
            {
                proc = value;
            }
        }

        public LeapDroidAppProcess(ProcessStartInfo startInfo, float screenScale)
        {
            proc = Process.Start(startInfo);
            _screenScale = screenScale;
        }

        public LeapDroidAppProcess(Process openedProc, float screenScale)
        {
            proc = openedProc;
            _screenScale = screenScale;
        }

        public Size stdNetSize()
        {
            Size size = new Size();
            size.Width = stdAppCaptureSize.X - borderLeftCapture - borderRightCapture;
            size.Height = stdAppCaptureSize.Y - borderTopCapture - borderBottomCapture;

            return size;
        }

        public float getAppScale()
        {
            int netHeight = (int) (getPhysicalPosition().Height - borderTopCapture - borderBottomCapture);

            return (float) netHeight / (stdAppCaptureSize.Y - borderTopCapture - borderBottomCapture);
        }

        public float getScreenScale()
        {
            return _screenScale;
        }

        public void setScreenScale(float screenScale)
        {
            _screenScale = screenScale;
        }

        public Rectangle getPosition()
        {
            Rect winPos = new Rect();
            GetWindowRect(proc.MainWindowHandle, ref winPos);
            Rectangle r = new Rectangle(winPos.Left, winPos.Top, winPos.Right - winPos.Left + 1, winPos.Bottom - winPos.Top + 1);

            return r;
        }

        public Rectangle getPhysicalPosition()
        {
            Rect winPos = new Rect();
            GetWindowRect(proc.MainWindowHandle, ref winPos);
            int x = (int)(winPos.Left * _screenScale);
            int y = (int)(winPos.Top * _screenScale);
            int w = (int)((winPos.Right - winPos.Left + 1) * _screenScale);
            int h = (int)((winPos.Bottom - winPos.Top + 1) * _screenScale);
            Rectangle r = new Rectangle(x, y, w, h);

            return r;
        }

        public Point bitmapToApp(Point p)
        {
            float appScale = getAppScale();
            int x = (int)((p.X - borderLeftCapture) / appScale);
            int y = (int)((p.Y - borderTopCapture) / appScale) ;

            return new Point(x, y);
        }

        public Point appToBitmap(Point p)
        {
            float appScale = getAppScale();
            int x = (int) (Math.Max(1, p.X * appScale) + borderLeftCapture);
            int y = (int)(Math.Max(1, p.Y * appScale) + borderTopCapture);

            return new Point(x, y);
        }

        public Rectangle appToBitmap(Rectangle r)
        {
            float appScale = getAppScale();
            int x = (int)(Math.Max(1, r.X * appScale) + borderLeftCapture);
            int y = (int)(Math.Max(1, r.Y * appScale) + borderTopCapture);
            int w = (int) Math.Max(1, r.Width * appScale);
            int h = (int) Math.Max(1, r.Height * appScale);

            return new Rectangle(x, y, w, h);
        }

        public Point appToPhysicalScreen(Point p)
        {
            float appScale = getAppScale();
            
            int x = (int) Math.Max(1, p.X * appScale) + borderLeftCapture + getPhysicalPosition().X;
            int y = (int) Math.Max(1, p.Y * appScale) + borderTopCapture + getPhysicalPosition().Y;

            return new Point(x, y);
        }

        public Rectangle appToPhysicalScreen(Rectangle r)
        {
            Rect winPos = new Rect();
            GetWindowRect(proc.MainWindowHandle, ref winPos);
            float appScale = getAppScale();
            
            int x = r.X + borderLeftCapture + getPhysicalPosition().X;
            int y = r.Y + borderTopCapture + getPhysicalPosition().Y;
            int w = (int)Math.Max(1, r.Width * appScale);
            int h = (int)Math.Max(1, r.Height * appScale);

            return new Rectangle(x, y, w, h);
        }

        public Point appToLogicalScreen(Point p)
        {
            int x = (int)(appToPhysicalScreen(p).X / _screenScale);
            int y = (int)(appToPhysicalScreen(p).Y / _screenScale);

            return new Point(x, y);
        }

        //public int toBitmapX(int x)
        //{
        //    return x + borderLeftCapture;
        //}

        //public int toBitmapY(int y)
        //{
        //    return y + borderTopCapture;
        //}

        //public Rectangle toActualScaledRegion(Rectangle dim)
        //{
        //    // dim refers to X, Y, Width and Height

        //    Rectangle r = new Rectangle();

        //    Rectangle currentDimension = getPhysicalPosition();
        //    int stdNetWidth = stdAppCaptureSize.X - borderLeftCapture - borderRightCapture;
        //    int currentWidth = currentDimension.Width - borderLeftCapture - borderRightCapture;

        //    int stdNetHeight = stdAppCaptureSize.Y - borderTopCapture - borderBottomCapture;
        //    int currentHeight = currentDimension.Height - borderTopCapture - borderBottomCapture;

        //    r.X = Math.Max(1, dim.X * currentWidth / stdNetWidth) + borderLeftCapture;
        //    r.Y = Math.Max(1, dim.Y * currentHeight / stdNetHeight) + borderTopCapture;
        //    r.Width = Math.Max(1, dim.Width * currentWidth / stdNetWidth);
        //    r.Height = Math.Max(1, dim.Height * currentHeight / stdNetHeight);

        //    return r;
        //}

        public void moveMouseTo(Point p)
        {
            // Physical size calculation
            // getPosition returns logical position
            // p and borders in physical position
            // Cursor requires logical position

            // first calculate the net area ratio:
            //float hratio = getPosition().Width * getScreenScale() / stdAppCaptureSize.X; // getPosition().Width returns captured size, i.e. 549 * 1.5 = 823
            //float physicalX = getPosition().X * getScreenScale() + borderLeftCapture + p.X;
            //float physicalY = getPosition().Y * getScreenScale() + borderTopCapture + p.Y;
            //int logicalX = (int)(physicalX / getScreenScale());
            //int logicalY = (int)(physicalY / getScreenScale());

            //Point pt = new Point(logicalX, logicalY);
            //Console.WriteLine("getPosition (WxH): {0},{1}", getPosition().Width, getPosition().Height);
            //Console.WriteLine("getPosition (XY): {0},{1}", getPosition().X, getPosition().Y);
            //Console.WriteLine("Logical Loc (XY): {0},{1}", logicalX, logicalY);
            Cursor.Position = appToLogicalScreen(p);
        }

        public void click(Point p)
        {
            //float hratio = (float)getPosition().Width * getScreenScale() / stdAppCaptureSize.X;
            //float physicalX = getPosition().X * getScreenScale() + borderLeftCapture + p.X;
            //float physicalY = getPosition().Y * getScreenScale() + borderTopCapture + p.Y;
            //int logicalX = (int)(physicalX / getScreenScale());
            //int logicalY = (int)(physicalY / getScreenScale());

            //Point pt = new Point(logicalX, logicalY);
            //System.Windows.Forms.Cursor.Position = pt;
            SetForegroundWindow(proc.MainWindowHandle);
            Cursor.Position = appToLogicalScreen(p);
            MouseSimulator.ClickLeftMouseButton();
        }

        public void startApp(int row, int col)
        {
            click(homeKey);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            click(appsList[row-1, col-1]);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(20));
        }

        public void refresh()
        {
            // Clear cache for Main Windows Handle
            proc.Refresh();
        }

        public void relocate(Point dest)
        {
            proc.Refresh();
            Rectangle r = getPosition();
            SetWindowPos(proc.MainWindowHandle, 0, dest.X, dest.Y, r.Width, r.Height, 0);
        }

        public void relocate(int x, int y)
        {
            proc.Refresh();
            Rectangle r = getPosition();
            SetWindowPos(proc.MainWindowHandle, 0, x, y, r.Width, r.Height, 0);
        }

        public void resize(int width, int height)
        {
            proc.Refresh();
            Rectangle r = getPosition();
            SetWindowPos(proc.MainWindowHandle, 0, r.X, r.Y, (int) (width / getScreenScale()), (int) (height/getScreenScale()), 0);
        }

        public void resetSize()
        {
            resize(stdAppCaptureSize.X, stdAppCaptureSize.Y);
        }

        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public Bitmap captureScreen()
        {
            ScreenCapture sc = new ScreenCapture();
            Image img = sc.CaptureWindow(proc.MainWindowHandle, (int)(getPosition().Width), (int)(getPosition().Height));
            Bitmap bmp = new Bitmap(img);
            /* old
            Bitmap bmp = new Bitmap((int)(getPosition().Width * _screenScale), (int) (getPosition().Height * _screenScale), PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(proc.MainWindowHandle, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();
            */
            return bmp;
        }

        public Bitmap captureScreenWithoutBorder()
        {
            Bitmap raw = captureScreen();
            Rectangle cropArea = new Rectangle(new Point(borderLeftCapture, borderTopCapture), new Size(raw.Width - borderLeftCapture - borderRightCapture, raw.Height - borderTopCapture - borderBottomCapture));
            Bitmap bmp = raw.Clone(cropArea, raw.PixelFormat);

            return bmp;
        }

        public Point toPhysicalPosition(Point relativePos)
        {
            Point pp = new Point(relativePos.X + getPosition().X, relativePos.Y + getPosition().Y);

            return pp;
        }

        public bool close()
        {
            bool result = false;
            if (!proc.HasExited)
            {
                proc.Refresh();
                result = proc.CloseMainWindow();
            }

            return result;
        }

        public bool hasExited()
        {
            return proc.HasExited;
        }
    }
}

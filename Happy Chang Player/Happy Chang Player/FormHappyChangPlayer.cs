using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Happy_Chang_Player
{
    public partial class FormHappyChangPlayer : Form
    {
        private float screenScale;
        private Boolean monitoring = false;
        private ArrayList targets;
        private int defaultPocketSize = 60;
        private int pocketSize;
        private Rectangle monitorArea;
        private int captureDelay;
        private int clickDelay;
        private int clicksPerBatch;
        private int normalCaptureDelay = 100;
        private int normalClickDelay = 35;
        private int normalClicksPerBatch = 6;
        private Thread monitoringThread;

        public bool Monitoring
        {
            get
            {
                return monitoring;
            }

            set
            {
                monitoring = value;
            }
        }

        public int PocketSize
        {
            get
            {
                return pocketSize;
            }

            set
            {
                pocketSize = value;
            }
        }

        public float ScreenScale
        {
            get
            {
                return screenScale;
            }

            set
            {
                screenScale = value;
            }
        }

        public int CaptureDelay
        {
            get
            {
                return captureDelay;
            }

            set
            {
                captureDelay = value;
            }
        }

        public Rectangle MonitorArea
        {
            get
            {
                return monitorArea;
            }

            set
            {
                monitorArea = value;
            }
        }

        public FormHappyChangPlayer()
        {
            InitializeComponent();
            initValues();
        }

        private void initValues()
        {
            ScreenScale = getScalingFactor();
            Rectangle screenSize = Screen.PrimaryScreen.WorkingArea;
            int width = (int)(screenSize.Width * ScreenScale);
            int height = (int)(screenSize.Height * ScreenScale);
            textBoxWidth.Text = width.ToString();
            textBoxHeight.Text = height.ToString();
            textBoxOffsetX.Text = "1";
            textBoxOffsetY.Text = "1";
            pocketSize = 60;
            clicksPerBatch = 12;
            clickDelay = 50;
            trackBarWidth.Maximum = width;
            trackBarHeight.Maximum = height;
            trackBarWidth.Minimum = 100;
            trackBarHeight.Minimum = 100;
            trackBarWidth.Value = width;
            trackBarHeight.Value = height;
            trackBarAppScale.Value = 100;
            trackBarClickDelay.Value = clickDelay;
            trackBarClicksPerBatch.Value = clicksPerBatch;

            updateValuesFromControls();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void addPoint(Point p)
        {
            if (p != null)
            {
                targets.Add(p);
            }
        }

        private void updateValuesFromControls()
        {
            try
            {
                monitorArea.X = int.Parse(textBoxOffsetX.Text);
            }
            catch (Exception e) { }
            try
            {
                monitorArea.Y = int.Parse(textBoxOffsetY.Text);
            }
            catch (Exception e) { }
            try
            {
                monitorArea.Width = int.Parse(textBoxWidth.Text);
            } catch(Exception e) { }
            try
            {
                monitorArea.Height = int.Parse(textBoxHeight.Text);
            }
            catch (Exception e) { }
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }

        private float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }

        private void buttonBasicWin_Click(object sender, EventArgs e)
        {
            FormBasic w = new FormBasic();
            w.Show();
        }

        private void updatePictureControl()
        {
            ScreenCapture sc = new ScreenCapture();
            Rectangle workingSize = Screen.PrimaryScreen.WorkingArea;
            int w = trackBarWidth.Value;
            int h = trackBarHeight.Value;
            workingSize.Width = w;
            workingSize.Height = h;

            Bitmap bmp = sc.CaptureScreen(workingSize.Width, workingSize.Height) as Bitmap;

            pictureBoxScreenCap.Image = bmp;

        }

        private void buttonRegionCapture_Click(object sender, EventArgs e)
        {
            updatePictureControl();
        }

        private void monitorAndKill()
        {
            int count = 0;
            Console.WriteLine("Monitoring Started.");
            try
            {
                while (true)
                {
                    count++;
                    ScreenCapture sc = new ScreenCapture();
                    Image img = sc.CaptureScreen(MonitorArea.Width, MonitorArea.Height);
                    Bitmap bmp = new Bitmap(img);
                    FindTargets(bmp);
                    pictureBoxScreenCap.Image = img;
                    //if (targets.Count > 0)
                    //{
                    //    Console.WriteLine("DEBUG: Capture #" + count + ", Targets: " + targets.Count);
                    //}

                    bmp.Dispose();
                    if (!Monitoring)
                    {
                        break;
                    }

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.Threading.Thread.Sleep(CaptureDelay);
                    //break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Monitoring Stopped.");
        }

        private void FindTargets(Bitmap bmp)
        {
            Cursor.Current = Cursors.WaitCursor;
            targets = new ArrayList();
            //ClickTasks tasks = new ClickTasks(PocketSize, ScreenScale);

            unsafe
            {
                BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                int origXSteps = 3;
                int origYSteps = 3;
                int xsteps = origXSteps;
                int ysteps = origYSteps;

                Parallel.For(0, heightInPixels - PocketSize, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    byte* next5Lines = PtrFirstPixel + ((y + PocketSize) * bitmapData.Stride);

                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel * xsteps)
                    {
                        int blue = currentLine[x];
                        int green = currentLine[x + 1];
                        int red = currentLine[x + 2];

                        if (blue == 0x3a && green == 0x56 && red == 0xff &&
                            x + PocketSize < widthInBytes &&
                            currentLine[x + bytesPerPixel * PocketSize] == 0x2d &&
                            currentLine[x + bytesPerPixel * PocketSize + 1] == 0x43 &&
                            currentLine[x + bytesPerPixel * PocketSize + 2] == 0xf0 &&
                            next5Lines[x] == 0x3a &&
                            next5Lines[x + 1] == 0x56 &&
                            next5Lines[x + 2] == 0xff &&
                            next5Lines[x + bytesPerPixel * PocketSize] == 0x2d &&
                            next5Lines[x + bytesPerPixel * PocketSize + 1] == 0x43 &&
                            next5Lines[x + bytesPerPixel * PocketSize + 2] == 0xf0)
                        {
                            Point p = new Point(x / bytesPerPixel + (int)PocketSize / 2, y + (int)PocketSize / 2);
                            //tasks.addTarget(p);
                            //tasks.killTargets();
                            addPoint(p);
                            xsteps = PocketSize;
                        }
                        else
                        {
                            xsteps = origXSteps;
                            continue;
                        }
                        //currentLine[x] = (byte)oldBlue;
                        //currentLine[x + 1] = (byte)oldGreen;
                        //currentLine[x + 2] = (byte)oldRed;
                    }
                });
                bmp.UnlockBits(bitmapData);

                for (int i = targets.Count - 1; i > 0; i--)
                {
                    if (targets[i] == null)
                    {
                        targets.RemoveAt(i);
                        continue;
                    }
                    else
                    {
                        Point pi = (Point)targets[i];
                        for (int j = i - 1; j >= 0; j--)
                        {
                            Point pj = (Point)targets[j];
                            if (Math.Abs(pi.X - pj.X) < pocketSize && Math.Abs(pi.Y - pj.Y) < pocketSize)
                            {
                                targets.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }


                if (targets.Count > 0)
                {
                    Point oldPos = Cursor.Position;

                    for (int i = targets.Count - 1; i >= Math.Max(targets.Count - clicksPerBatch, 0); i--)
                    //foreach (Point p in targets)
                    {
                        Point p = (Point)targets[i];
                        Cursor.Position = new Point((int)(p.X / screenScale), (int)((p.Y) / screenScale));
                        //Cursor.Position = new Point((int)(p.X / screenScale), (int)((p.Y + pocketSize / 2) / screenScale));
                        Console.WriteLine("Target (x,y): {0}", targets[i]);
                        MouseSimulator.ClickLeftMouseButton();
                        System.Threading.Thread.Sleep(clickDelay);
                    }

                    //if (tasks.getNumOfTargets() > 10)
                    //{
                    //    CaptureDelay = (int)(normalCaptureDelay * 0);
                    //    clicksPerBatch = normalClicksPerBatch + 6;
                    //    clickDelay = (int)(normalClickDelay / 2);
                    //}
                    //else if (tasks.getNumOfTargets() > 8)
                    //{
                    //    CaptureDelay = (int)(normalCaptureDelay * 0.4);
                    //    clicksPerBatch = normalClicksPerBatch + 4;
                    //    clickDelay = (int)(normalClickDelay / 2);
                    //}
                    //else if (tasks.getNumOfTargets() > 4)
                    //{
                    //    CaptureDelay = (int)(normalCaptureDelay * 0.6);
                    //    clicksPerBatch = normalClicksPerBatch + 2;
                    //    clickDelay = normalClickDelay;
                    //}
                    //else
                    //{
                    //    CaptureDelay = normalCaptureDelay;
                    //    clicksPerBatch = normalClicksPerBatch;
                    //    clickDelay = normalClickDelay;
                    //}
                    //Console.WriteLine("Targets: " + tasks.getNumOfTargets() + ", Capture Delay: " + CaptureDelay + ", Clicks per batch: " + clicksPerBatch + ", Click Delay: " + clickDelay);
                    //Cursor.Position = oldPos;
                }

            }
            Cursor.Current = Cursors.Default;
        }

        private void buttonMonitoring_Click(object sender, EventArgs e)
        {
            if (Monitoring)
            {
                // Stop monitoring
                Monitoring = false;
                
                if (monitoringThread != null && monitoringThread.Join(normalCaptureDelay * 2) == false)
                {
                    monitoringThread.Abort();
                }

                buttonMonitoring.Text = "Start Monitoring";
            }
            else
            {
                // Start monitoring
                Console.WriteLine("Started a new thread.");
                monitoringThread = new Thread(monitorAndKill);
                Monitoring = true;
                buttonMonitoring.Text = "Stop Monitoring";
                monitoringThread.Start();
            }

        }

        private void textBoxOffsetX_TextChanged(object sender, EventArgs e)
        {
            updateValuesFromControls();
        }

        private void textBoxOffsetY_TextChanged(object sender, EventArgs e)
        {
            updateValuesFromControls();
        }

        private void textBoxWidth_TextChanged(object sender, EventArgs e)
        {
            updateValuesFromControls();
        }

        private void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            updateValuesFromControls();
        }

        private void trackBarWidth_Scroll(object sender, EventArgs e)
        {
            textBoxWidth.Text = trackBarWidth.Value.ToString();
            updatePictureControl();
        }

        private void trackBarHeight_Scroll(object sender, EventArgs e)
        {
            textBoxHeight.Text = trackBarHeight.Value.ToString();
            updatePictureControl();
        }

        private void trackBarAppScale_Scroll(object sender, EventArgs e)
        {
            labelAppScale.Text = trackBarAppScale.Value.ToString() + "%";
            pocketSize = trackBarAppScale.Value * defaultPocketSize / 100;
        }

        private void trackBarClickDelay_Scroll(object sender, EventArgs e)
        {
            labelClickDelay.Text = trackBarClickDelay.Value.ToString() + "ms";
            clickDelay = trackBarClickDelay.Value;
        }

        private void trackBarClicksPerBatch_Scroll(object sender, EventArgs e)
        {
            labelClicksPerBatch.Text = trackBarClicksPerBatch.Value.ToString();
            clicksPerBatch = trackBarClicksPerBatch.Value;
        }
    }
}

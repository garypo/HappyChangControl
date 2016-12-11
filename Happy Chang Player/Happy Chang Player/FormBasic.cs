using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Tesseract;

namespace Happy_Chang_Player
{
    public partial class FormBasic : Form
    {
        public const int PROFILE_2X2_75P = 1;
        public const int PROFILE_2X2_50P = 2;
        public const int PROFILE_2X1_75P = 3;
        public const int PROFILE_2X1_50P = 4;
        public const int PROFILE_1X2_75P = 5;
        public const int PROFILE_1X2_50P = 6;
        public const int PROFILE_1X1_100P = 7;
        public const int PROFILE_1X1_75P = 8;
        public const int PROFILE_1X1_50P = 9;

        private Boolean monitoring=false;
        private float screenScale;
        private ArrayList targets;
        private int normalCaptureDelay = 100;
        private int normalClickDelay = 35;
        private int normalClicksPerBatch = 6;
        private int defaultPocketSize = 60;
        private int captureDelay;
        private int clickDelay;
        private int clicksPerBatch;
        private int pocketSize;
        private int captureWidth = 520;
        private int captureHeight = 840;
        private int timeOutLoop = 5;
        private int horizontalSeparation = 35;
        private int verticalSeparation = 2;
        private WindowManager wm;

        private int numCaptures = 0;
        private DateTime buttonClickTime;

        private Thread monitoringThread;
        private Thread shutdownThread;
        private HappyChangHelperForm hcForm;

        public FormBasic()
        {
            InitializeComponent();
            monitoring = false;
            screenScale = getScalingFactor();
            // parameters for 3 windows
            captureWidth = 620; // for two
            //captureWidth = 900; // for three
            captureWidth = 420; // for one (75%)
            captureWidth = 900; // for two (75%)
            captureHeight = 900; // for two (50%)
            captureHeight = 1380; // for two (75%)
            timeOutLoop = 15;
            //loadProfile(1);

            wm = new WindowManager("Nox App Player");// LeapdroidVM");
            captureDelay = normalCaptureDelay;
            clicksPerBatch = normalClicksPerBatch;
            clickDelay = normalClickDelay;
            pocketSize = defaultPocketSize;

            textBoxScreenRatio.Text = screenScale.ToString();
            
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            int width = (int) (screenSize.Width * screenScale);
            int height = (int)(screenSize.Height * screenScale);
            textBoxScreenWidth.Text = width.ToString();
            textBoxScreenHeight.Text = height.ToString();

            Rectangle workingSize = Screen.PrimaryScreen.WorkingArea;
            width = (int)(workingSize.Width * screenScale);
            height = (int)(workingSize.Height * screenScale);
            textBoxMonitoringAreaWidth.Text = width.ToString();
            textBoxMonitoringAreaHeight.Text = height.ToString();

            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;
            int leftBorderWidth = screenRectangle.Left - this.Left;
            
            Console.WriteLine("Title heigh: {0}; Left border: {1}, sysinfo border: {2}", titleHeight, leftBorderWidth, SystemInformation.BorderSize.Width);
        }

        private void loadProfile(int id)
        {
            switch (id)
            {
                case PROFILE_2X2_75P:
                    captureWidth = 900; // for two (75%)
                    captureHeight = 1380; // for two (75%)
                    pocketSize = 45;
                    clicksPerBatch = 12;
                    clickDelay = 30;
                    break;
                case PROFILE_2X2_50P:
                    captureWidth = 620;
                    captureHeight = 900;
                    pocketSize = 30;
                    clicksPerBatch = 12;
                    clickDelay = 30;
                    break;
                case PROFILE_2X1_75P:
                    captureWidth = 900;
                    captureHeight = 700;
                    pocketSize = 45;
                    clicksPerBatch = 12;
                    clickDelay = 30;
                    break;
                case PROFILE_2X1_50P:
                    captureWidth = 620;
                    captureHeight = 450;
                    pocketSize = 30;
                    clicksPerBatch = 10;
                    clickDelay = 30;
                    break;
                case PROFILE_1X2_75P:
                    captureWidth = 450;
                    captureHeight = 1380;
                    pocketSize = 45;
                    clicksPerBatch = 10;
                    clickDelay = 30;
                    break;
                case PROFILE_1X2_50P:
                    captureWidth = 310;
                    captureHeight = 900;
                    pocketSize = 30;
                    clicksPerBatch = 10;
                    clickDelay = 30;
                    break;
                case PROFILE_1X1_75P:
                    captureWidth = 450;
                    captureHeight = 700;
                    pocketSize = 45;
                    clicksPerBatch = 6;
                    clickDelay = 50;
                    break;
                case PROFILE_1X1_50P:
                    captureWidth = 310;
                    captureHeight = 450;
                    pocketSize = 30;
                    clicksPerBatch = 6;
                    clickDelay = 50;
                    break;
                case PROFILE_1X1_100P:
                default:
                    captureWidth = 620;
                    captureHeight = 900;
                    pocketSize = 60;
                    clicksPerBatch = 6;
                    clickDelay = 50;
                    break;
            }
        }

        private Boolean isMonitorOn()
        {
            return monitoring;
        }

        private void setMonitorOn(Boolean monitor)
        {
            monitoring = monitor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void addPoint(Point p)
        {
            if (p != null)
            {
                targets.Add(p);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonClickTime = DateTime.Now;
            //this.Hide();
            //System.Threading.Thread.Sleep(100);
            ScreenCapture sc = new ScreenCapture();
            
            //img.Save("C:\\Users\\Gary\\Documents\\Temp\\test.png");
            //this.Show();
            numCaptures = 0;
            while (true)
            {
                Image img = sc.CaptureScreen(captureWidth, captureHeight);
                Bitmap bmp = new Bitmap(img);
                FindTargets(bmp);
                pictureBox.Image = img;
                numCaptures++;
                bmp.Dispose();
                System.Threading.Thread.Sleep(captureDelay);

                if (targets.Count == 0 || DateTime.Now.Subtract(buttonClickTime).Seconds >= timeOutLoop)
                //if (DateTime.Now.Subtract(buttonClickTime).Seconds >= 900)
                {
                    break;
                }
            }
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

        }

        private static Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
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
                    Image img = sc.CaptureScreen(captureWidth, captureHeight);
                    Bitmap bmp = new Bitmap(img);
                    FindTargets(bmp);
                    pictureBox.Image = img;
                    //if (targets.Count > 0)
                    //{
                    //    Console.WriteLine("DEBUG: Capture #" + count + ", Targets: " + targets.Count);
                    //}
                    
                    bmp.Dispose();
                    if (isMonitorOn() == false)
                    {
                        break;
                    }
                    //if (wm.getNumberOfWindows() > 0)
                    //{
                    //    TesseractEngine engine = new TesseractEngine(@"C:\Users\Gary\Documents\Development\Visual Studio 2015\Projects\AutoMouseClicks\AutoMouseClicks\tessdata", "eng", EngineMode.Default);
                    //    int cropWidth = wm.getAppWinWidth();
                    //    Rectangle cropArea = wm.getAppWindow(0).getAreaPeopleInRoom();
                    //    Image ppl = cropImage(img, cropArea);
                    //    ppl.Save("test.tiff", System.Drawing.Imaging.ImageFormat.Tiff);
                    //    Page page = engine.Process(Pix.LoadFromFile("test.tiff"));
                    //    ResultIterator iter = page.GetIterator();
                    //    iter.Begin();
                    //    String text = page.GetText();
                    //    Console.WriteLine("OCR Debug: " + text);
                    //    //do
                    //    //{
                    //    //} while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                    //}

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.Threading.Thread.Sleep(captureDelay);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            Console.WriteLine("Monitoring Stopped.");
        }

        private void FindTargets(Bitmap bmp)
        {
            Cursor.Current = Cursors.WaitCursor;
            //targets = new ArrayList();
            ClickTasks tasks = new ClickTasks(pocketSize, screenScale);

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

                Parallel.For(0, heightInPixels - pocketSize, y =>
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    byte* next5Lines = PtrFirstPixel + ((y + pocketSize) * bitmapData.Stride);

                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel*xsteps)
                    {
                        int blue = currentLine[x];
                        int green = currentLine[x + 1];
                        int red = currentLine[x + 2];

                        if (blue == 0x3a && green == 0x56 && red == 0xff && 
                            x + pocketSize < widthInBytes &&
                            currentLine[x + bytesPerPixel * pocketSize] == 0x2d &&
                            currentLine[x + bytesPerPixel * pocketSize + 1] == 0x43 &&
                            currentLine[x + bytesPerPixel * pocketSize + 2] == 0xf0 &&
                            next5Lines[x] == 0x3a &&
                            next5Lines[x + 1] == 0x56 &&
                            next5Lines[x + 2] == 0xff &&
                            next5Lines[x + bytesPerPixel * pocketSize] == 0x2d &&
                            next5Lines[x + bytesPerPixel * pocketSize + 1] == 0x43 &&
                            next5Lines[x + bytesPerPixel * pocketSize + 2] == 0xf0)
                        {
                            Point p = new Point(x / bytesPerPixel + (int)pocketSize/ 2, y + (int)pocketSize/ 2);
                            tasks.addTarget(p);
                            tasks.killTargets();
                            //addPoint(p);
                            xsteps = pocketSize;
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
                
                //for(int i = targets.Count - 1; i > 0; i--)
                //{
                //    if (targets[i] == null)
                //    {
                //        targets.RemoveAt(i);
                //        continue;
                //    }
                //    else
                //    {
                //        Point pi = (Point)targets[i];
                //        for (int j = i - 1; j >= 0; j--)
                //        {
                //            Point pj = (Point)targets[j];
                //            if (Math.Abs(pi.X - pj.X) < pocketSize && Math.Abs(pi.Y - pj.Y) < pocketSize)
                //            {
                //                targets.RemoveAt(i);
                //                break;
                //            }
                //        }
                //    }
                //}

                
                //if (targets.Count > 0)
                //{
                    //Point oldPos = Cursor.Position;

                    //for(int i=targets.Count-1; i>=Math.Max(targets.Count-clicksPerBatch, 0); i--)
                    ////foreach (Point p in targets)
                    //{
                    //    Point p = (Point)targets[i];
                    //    Cursor.Position = new Point((int) (p.X / screenScale), (int) ((p.Y + pocketSize/2) / screenScale));
                    //    MouseSimulator.ClickLeftMouseButton();
                    //    //MouseSimulator.ClickLeftMouseButton();
                    //    System.Threading.Thread.Sleep(clickDelay);
                    //}
                    if (tasks.getNumOfTargets() > 10)
                    {
                        captureDelay = (int) (normalCaptureDelay * 0);
                        clicksPerBatch = normalClicksPerBatch + 6;
                        clickDelay = (int) (normalClickDelay / 2);
                    }
                    else if (tasks.getNumOfTargets() > 8)
                    {
                        captureDelay = (int)(normalCaptureDelay * 0.4);
                        clicksPerBatch = normalClicksPerBatch + 4;
                        clickDelay = (int)(normalClickDelay / 2);
                    }
                    else if (tasks.getNumOfTargets() > 4)
                    {
                        captureDelay = (int)(normalCaptureDelay * 0.6);
                        clicksPerBatch = normalClicksPerBatch + 2;
                        clickDelay = normalClickDelay;
                    }
                    else
                    {
                        captureDelay = normalCaptureDelay;
                        clicksPerBatch = normalClicksPerBatch;
                        clickDelay = normalClickDelay;
                    }
                    Console.WriteLine("Targets: " + tasks.getNumOfTargets() + ", Capture Delay: " + captureDelay + ", Clicks per batch: " + clicksPerBatch + ", Click Delay: " + clickDelay);
                    //Cursor.Position = oldPos;
                //}
            }
            //Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int x = int.Parse(textBoxLocationX.Text);
            int y = int.Parse(textBoxLocationY.Text);
            
            p.moveMouseTo(new Point(x, y));
            //Point oldPos = Cursor.Position;
            //Cursor.Position = new Point(int.Parse(textBoxLocationX.Text), int.Parse(textBoxLocationY.Text));
            
            //ActionSimulator.ClickLeftMouseButton();

            /// return mouse 
            //Cursor.Position = oldPos;
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

        private void buttonMonitoring_Click(object sender, EventArgs e)
        {
            if (isMonitorOn())
            {
                // Stop monitoring
                setMonitorOn(false);
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
                setMonitorOn(true);
                buttonMonitoring.Text = "Stop Monitoring";
                monitoringThread.Start();
            }
                
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            setMonitorOn(false);
            Console.WriteLine("Killing thread...");
            if (monitoringThread != null && monitoringThread.Join(captureDelay * 2) == false)
            {
                monitoringThread.Abort();
            }

            buttonMonitoring.Text = "Start Monitoring";
        }

        private void buttonTestFunction_Click(object sender, EventArgs e)
        {
            //int x = int.Parse(textBoxLocationX.Text);
            //int y = int.Parse(textBoxLocationY.Text);
            //int c = int.Parse(textBoxTestInput.Text);

            //Cursor.Position = new Point(x, y);
            //for(int i = 0; i < c; i++)
            //{
            //    MouseSimulator.ClickLeftMouseButton();
            //    Console.WriteLine(i);
            //    Thread.Sleep(30);
            //}
            
            //WindowManager wm = new WindowManager("LeapdroidVM");
            //wm.closeWindow(3);
        }

        private void buttonArrangeWindow_Click(object sender, EventArgs e)
        {
            Boolean origState = isMonitorOn();

            if (origState)
            {
                // Stop monitoring
                setMonitorOn(false);
                if (monitoringThread != null && monitoringThread.Join(normalCaptureDelay * 2) == false)
                {
                    monitoringThread.Abort();
                }

                buttonMonitoring.Text = "Start Monitoring";
            }

            int availableWidth = (int)(int.Parse(textBoxMonitoringAreaWidth.Text) / screenScale);
            int availableHeight = (int)(int.Parse(textBoxMonitoringAreaHeight.Text) / screenScale);
            wm.autoArrangeByArea(availableWidth, availableHeight, 0, 0, horizontalSeparation, verticalSeparation);
            captureWidth = wm.getMonitoringWidth();
            captureHeight = wm.getMonitoringHeight();
            pocketSize = getRedPocketSize();
            //Console.WriteLine("Capture (wxh)(logical): " + captureWidth + ", " + captureHeight);
            //Console.WriteLine("Capture (wxh)(physical): " + captureWidth * screenScale + ", " + captureHeight * screenScale);
            //Console.WriteLine("New Pocket Size: " + pocketSize);

            if (origState)
            {
                // Start monitoring
                monitoringThread = new Thread(monitorAndKill);
                setMonitorOn(true);
                buttonMonitoring.Text = "Stop Monitoring";
                monitoringThread.Start();
            }
        }

        private int getRedPocketSize()
        {
            int size = defaultPocketSize;
            int height = wm.getAppWinHeight();
            if (height > 0)
            {
                size = (int)(defaultPocketSize * height / 1326);
            }

            return size;
        }
        private HappyChang p;
        private void buttonTest1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "C:\\Program Files\\Leapdroid\\VM\\LeapdroidVM.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.Arguments = "-vfiber -s vm2a";
            p = new HappyChang(startInfo, screenScale);

            //Thread.Sleep(15000);

            //p.relocate(1600, 700);
            //p.resize(p.getPosition().Width * 2 / 3, p.getPosition().Height * 2 / 3);
            //Thread.Sleep(12000);
            //p.close();
            //Thread.Sleep(2000);
            //if (p.HasExited)
            //{
            //    Console.WriteLine("Windows is closed");
            //    p.Start();
            //}
            //else
            //{
            //    Console.WriteLine("Windows is still open");
            //}
        }

        private void buttonTest2_Click(object sender, EventArgs e)
        {
            //SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\Gary\Music\HappyChang.wav");
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            SoundPlayer sp;

            sp = new SoundPlayer(assembly.GetManifestResourceStream("AutoMouseClicks.HappyChang.wav"));
            sp.Play();
        }

        private void buttonQueueSongs_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < 50; i++)
            {
                if (p.queueASong() == false)
                {
                    break;
                }
            }
        }

        private void textBoxLocationX_Enter(object sender, EventArgs e)
        {
            textBoxLocationX.SelectAll();
        }

        private void textBoxLocationY_Enter(object sender, EventArgs e)
        {
            textBoxLocationY.SelectAll();
        }

        private void textBoxSX_Enter(object sender, EventArgs e)
        {
            textBoxSX.SelectAll();
        }

        private void textBoxSY_Enter(object sender, EventArgs e)
        {
            textBoxSY.SelectAll();
        }

        private void buttonGotoScreenLocation_Click(object sender, EventArgs e)
        {
            Point oldPos = Cursor.Position;
            int x = int.Parse(textBoxSX.Text);
            int y = int.Parse(textBoxSY.Text);

            if (checkBoxScreenRatioApply.Checked)
            {
                x = (int) (x / screenScale);
                y = (int) (y / screenScale);
            }

            Cursor.Position = new Point(x, y);
        }

        private void buttonHappyChangHelper_Click(object sender, EventArgs e)
        {
            if (hcForm == null)
            {
                hcForm = new HappyChangHelperForm(p);
                hcForm.StartPosition = FormStartPosition.CenterParent;
            }
            hcForm.ShowDialog(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Tesseract;

namespace Happy_Chang_Player
{
    partial class HappyChangHelperForm : Form
    {
        public HappyChang mainProcess;
        private List<HappyChang> vmList;
        //private HappyChang[] procs;
        private Dictionary<Int32, String> vmInstances;

        public HappyChangHelperForm(HappyChang p)
        {
            init();
        }

        public HappyChangHelperForm()
        {
            init();
        }

        private void init()
        {
            InitializeComponent();
            vmList = new List<HappyChang>();
            listBoxVMs.DataSource = vmList;
            vmInstances = new Dictionary<int, string>();
            int i = -1;
            vmInstances.Add(i++, "-");
            vmInstances.Add(i++, "1");
            vmInstances.Add(i++, "1-1");
            vmInstances.Add(i++, "1-1a");
            vmInstances.Add(i++, "2");
            vmInstances.Add(i++, "2a");
            vmInstances.Add(i++, "2b");
            vmInstances.Add(i++, "2c");
            vmInstances.Add(i++, "2d");
            vmInstances.Add(i++, "3");
            vmInstances.Add(i++, "5");
            vmInstances.Add(i++, "5-1");
            vmInstances.Add(i++, "5-2");
            vmInstances.Add(i++, "5-3");
            vmInstances.Add(i++, "5-4");

            comboBoxVMInstances.DataSource = new BindingSource(vmInstances, null);
            comboBoxVMInstances.DisplayMember = "Value";
            comboBoxVMInstances.ValueMember = "Key";
            //procs = new HappyChang[vmInstances.Count];
        }

        public void setMainProcess(HappyChang p)
        {
            mainProcess = p;
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }

        private float getScreenScale()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }

        private HappyChang getSelectedProcess()
        {
            HappyChang hc = (HappyChang) listBoxVMs.SelectedItem;

            /* dropbox
            KeyValuePair<Int32, String> entry = (KeyValuePair<Int32, String>)comboBoxVMInstances.SelectedItem;

            if (entry.Key >= 0)
            {
                hc = procs[entry.Key];
            }
            */

            return hc;
        }

        private void buttonDim_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            if (getSelectedProcess().testIfDimmed(bmp))
            {
                Console.WriteLine("Screen is dimmed.");
            }
            else
            {
                Console.WriteLine("Screen is not dimmed.");
            }
        }

        private void buttonMsgBox_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            if (getSelectedProcess().testIfMessagePrompted(bmp))
            {
                Console.WriteLine("A message is prompted");
            }
            else
            {
                Console.WriteLine("No message prompted");
            }
        }

        private void buttonQuestion_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            if (getSelectedProcess().testIfQuestionPrompted(bmp))
            {
                Console.WriteLine("A question is prompted");
            }
            else
            {
                Console.WriteLine("No question prompted");
            }
        }

        private void buttonMusicPlaying_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            if (getSelectedProcess().testIfMusicIsPlaying(bmp))
            {
                Console.WriteLine("Music Detected");
            }
            else
            {
                Console.WriteLine("Music NOT Detected");
            }
        }

        private void buttonAwardPrompted_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            if (getSelectedProcess().testIfAwardMessagePrompted(bmp))
            {
                Console.WriteLine("Award Detected.");
            }
            else
            {
                Console.WriteLine("Award Screen NOT Detected.");
            }
        }

        private void buttonSinging_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            if (getSelectedProcess().testIfSingingDetected(bmp))
            {
                Console.WriteLine("Singing Detected");
            }
            else
            {
                Console.WriteLine("Singing NOT Detected");
            }
        }

        private void buttonScreenCap_Click(object sender, EventArgs e)
        {
            String filenamePrefix = "Capture";
            int sn = 0;
            Bitmap bmp = getSelectedProcess().captureScreenWithoutBorder();

            while (File.Exists(filenamePrefix + String.Format("{0:D3}", sn) + ".bmp"))
            {
                sn++;
            }
            bmp.Save(filenamePrefix + String.Format("{0:D3}", sn) + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

        }

        private void buttonWaitForMicUp_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => getSelectedProcess().waitForMicUp(TimeSpan.FromSeconds(300)));
            t.Start();
        }

        private void buttonTemp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch w = new System.Diagnostics.Stopwatch();
            w.Start();
            TimeSpan ts = TimeSpan.FromSeconds(5);
            System.Threading.Thread.Sleep(ts);
            Console.WriteLine("Time elapsed in s: Total: {0}, Second: {1}", w.Elapsed.TotalSeconds, w.Elapsed.Seconds);
            w.Stop();
            Console.WriteLine("Time elapsed in s: Total: {0}, Second: {1}", w.Elapsed.TotalSeconds, w.Elapsed.Seconds);
        }

        private void textBoxX_Enter(object sender, EventArgs e)
        {
            textBoxX.SelectAll();
        }

        private void textBoxY_Enter(object sender, EventArgs e)
        {
            textBoxY.SelectAll();
        }

        private void textBoxW_Enter(object sender, EventArgs e)
        {
            textBoxW.SelectAll();
        }

        private void textBoxH_Enter(object sender, EventArgs e)
        {
            textBoxH.SelectAll();
        }

        private void buttonSpecifcColorAnalysis_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            float t = 0.0f;
            
            if (int.TryParse(textBoxX.Text, out x) && 
                int.TryParse(textBoxY.Text, out y) &&
                int.TryParse(textBoxW.Text, out w) &&
                int.TryParse(textBoxH.Text, out h) &&
                float.TryParse(textBoxThreshold.Text, out t))
            {
                Rectangle targetDimension = new Rectangle(x, y, w, h);
                //Rectangle actualDimension = getSelectedProcess().toActualScaledRegion(targetDimension);
                Rectangle actualDimension = getSelectedProcess().appToBitmap(targetDimension);
                Bitmap bmp = getSelectedProcess().captureScreen();
                BitmapAnalysis.specificColorAnalysis(bmp, actualDimension, t);
            }
            else
            {
                MessageBox.Show("Please enter the region and threshold value.");
            }
        }

        private void buttonScanRedPockets_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            int count = getSelectedProcess().scanForRedPockets(bmp);
            Console.WriteLine("Targets: {0}", count);
        }

        private void buttonQueueSongs_Click(object sender, EventArgs e)
        {
            if (getSelectedProcess().isQueueingSongOn())
            {
                getSelectedProcess().stopQueueingSongs();
            }
            else
            {
                int count = (int)numericUpDownQueueTimes.Value;
                if (count > 0)
                {
                    getSelectedProcess().startQueueingSongs(count);
                }
            }
            refreshStatus();
            //if (queueSongThread != null && !queueSongThread.IsAlive)
            //{
            //    queueSongThread.Abort();
            //    buttonQueueSongs.Text = "Start Queueing Songs";
            //}
            //else
            //{
            //    ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(queueSong);
            //    updateQueueStatusCallback callback = new updateQueueStatusCallback(updateQueueStatus);
            //    queueSongThread = new Thread(parameterizedThreadStart);
            //    queueSongThread.Start(callback);
            //}
        }

        private void queueSong(Object callback)
        {
            updateQueueStatusCallback callbackFunction = (updateQueueStatusCallback) callback;
            int count = (int)numericUpDownQueueTimes.Value;
            for (int i = 0; i < count; i++)
            {
                callbackFunction(i + 1, count);
                if (!getSelectedProcess().queueASong())
                {
                    break;
                }
            }
        }

        public delegate void updateQueueStatusCallback(int num, int count);

        public void updateQueueStatus(int num, int count)
        {
            String text = String.Format("Stop Queueing Songs\r\n({0} of {1})", num, count);
            Console.WriteLine(text);
            //buttonQueueSongs.Text = text;
        }

        private void numericUpDownQueueTimes_Enter(object sender, EventArgs e)
        {
            
        }

        private void buttonRegionAverage_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;

            if (int.TryParse(textBoxX.Text, out x) &&
                int.TryParse(textBoxY.Text, out y) &&
                int.TryParse(textBoxW.Text, out w) &&
                int.TryParse(textBoxH.Text, out h) )
            {
                Rectangle targetDimension = new Rectangle(x, y, w, h);
                Rectangle actualDimension = getSelectedProcess().appToBitmap(targetDimension);
                Bitmap bmp = getSelectedProcess().captureScreen();

                BitmapAnalysis.averageColorAnalysis(bmp, actualDimension);
            }
            else
            {
                MessageBox.Show("Please enter the region.");
            }
        }

        private void buttonMonitor_Click(object sender, EventArgs e)
        {
            if (getSelectedProcess() != null)
            {
                if (getSelectedProcess().isMonitoringOn() == false)
                {
                    getSelectedProcess().startMonitoring();
                    buttonMonitor.Text = "Stop Monitoring";
                }
                else
                {
                    getSelectedProcess().stopMonitoring();
                    buttonMonitor.Text = "Start Monitoring";
                }
                refreshStatus();
            }
        }

        private void buttonStartVM_Click(object sender, EventArgs e)
        {
            //KeyValuePair<Int32, String> entry = (KeyValuePair<Int32, String>)comboBoxVMInstances.SelectedItem;
            
            //if (entry.Key >= 0 && (procs[entry.Key] == null || procs[entry.Key].hasExited()))
            //{
            //    ProcessStartInfo startInfo = new ProcessStartInfo();
            //    startInfo.CreateNoWindow = false;
            //    startInfo.UseShellExecute = false;
            //    startInfo.FileName = "C:\\Program Files\\Leapdroid\\VM\\LeapdroidVM.exe";
            //    startInfo.WindowStyle = ProcessWindowStyle.Normal;
            //    startInfo.Arguments = "-vfiber -s vm" + entry.Value;
            //    procs[entry.Key] = new HappyChang(startInfo, getScreenScale());

            //    setButtonsEnable(true);
                
            //    buttonStartVM.Text = "Close VM";
            //    buttonMonitor.Text = "Start Monitoring";
            //    buttonQueueSongs.Text = "Start Queueing Songs";
            //    buttonTestGiveRedPockets.Text = "Start Giving Red Pockets";
            //    buttonAutoExit.Text = "Start Auto Exit";
            //}
            //else
            //{
            //    procs[entry.Key].close();
            //    procs[entry.Key] = null;
            //    buttonStartVM.Text = "Start VM";
            //    setButtonsEnable(false);
            //}
        }

        private void setButtonsEnable(bool enable)
        {
            buttonAwardPrompted.Enabled = enable;
            buttonDim.Enabled = enable;
            buttonMsgBox.Enabled = enable;
            buttonQuestion.Enabled = enable;
            buttonMusicPlaying.Enabled = enable;
            buttonSinging.Enabled = enable;
            buttonWaitForMicUp.Enabled = enable;
            buttonMonitor.Enabled = enable;
            buttonScanRedPockets.Enabled = enable;
            buttonDownMic.Enabled = enable;
            buttonSpecifcColorAnalysis.Enabled = enable;
            buttonMonitor.Enabled = enable;
            buttonRegionAverage.Enabled = enable;
            buttonScreenCap.Enabled = enable;
            buttonQueueSongs.Enabled = enable;
            buttonResetAppSize.Enabled = enable;
            buttonTestAwardMsg.Enabled = enable;
            buttonTestGiveRedPockets.Enabled = enable;
            buttonAutoExit.Enabled = enable;
            buttonTestInactivity.Enabled = enable;
            buttonTestQueueNo.Enabled = enable;
            buttonOCR.Enabled = enable;
            buttonGiveRedPockets.Enabled = enable;
            buttonClickNTimes.Enabled = enable;
            buttonRefreshStatus.Enabled = enable;
        }

        private void updateImage()
        {
            ScreenCapture sc = new ScreenCapture();
            Rectangle workingSize = Screen.PrimaryScreen.WorkingArea;
            Bitmap bmp = sc.CaptureScreen(workingSize.Width, workingSize.Height) as Bitmap;
            
            HappyChang p = getSelectedProcess();
            if (p != null)
            {
                Rectangle pos = p.getPosition();
                for(int x = pos.X; x < pos.X + pos.Width; x++)
                {
                    bmp.SetPixel(x, pos.Y, Color.Yellow);
                }
            }
            
            pictureBoxScreenCap.Image = bmp;

        }

        private void comboBoxVMInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
            //updateImage();
            //KeyValuePair<Int32, String> entry = (KeyValuePair < Int32, String>) comboBoxVMInstances.SelectedItem;
            
            //if (entry.Key < 0 || procs[entry.Key] == null)
            //{
            //    setButtonsEnable(false);
            //    buttonStartVM.Text = "Start VM";
            //    if (entry.Key < 0)
            //    {
            //        buttonStartVM.Enabled = false;
            //    }
            //    else
            //    {
            //        buttonStartVM.Enabled = true;
            //    }
            //    labelQueueStatus.Text = "OFF";
            //}
            //else
            //{
            //    buttonStartVM.Text = "Close VM";

            //    setButtonsEnable(true);
            //    if (procs[entry.Key].isMonitoringOn())
            //    {
            //        buttonMonitor.Text = "Stop Monitoring";
            //    }
            //    else
            //    {
            //        buttonMonitor.Text = "Start Monitoring";
            //    }
            //    if (procs[entry.Key].isQueueingSongOn())
            //    {
            //        buttonQueueSongs.Text = "Stop Queueing Songs";
            //        labelQueueStatus.Text = String.Format("Queueing {0} of {1} songs.", procs[entry.Key].FinishedSongs, procs[entry.Key].NumSongsToQueue);
            //    }
            //    else
            //    {
            //        buttonQueueSongs.Text = "Start Queueing Songs";
            //        labelQueueStatus.Text = "OFF";
            //    }
            //    if (procs[entry.Key].GiveRedPocketsOn)
            //    {
            //        buttonTestGiveRedPockets.Text = "Stop Giving Red Pockets";
            //        labelGiveRedPocketsStatus.Text = String.Format("Giving {0} of {1} times.", procs[entry.Key].GiveRedPocketCompletedTimes, procs[entry.Key].GiveRedPocketTimes);
            //    }
            //    else
            //    {
            //        buttonTestGiveRedPockets.Text = "Start Giving Red Pockets";
            //        labelGiveRedPocketsStatus.Text = "OFF";
            //    }
            //    if (procs[entry.Key].InactivityAutoExitOn)
            //    {
            //        buttonAutoExit.Text = "Stop Auto Exit";
            //    }
            //    else
            //    {
            //        buttonAutoExit.Text = "Start Auto Exit";
            //    }
            //}
            //refreshStatus();
        }

        private void buttonGotoAppLocation_Click(object sender, EventArgs e)
        {
            try
            {
                int x = int.Parse(textBoxLocationX.Text);
                int y = int.Parse(textBoxLocationY.Text);

                getSelectedProcess().moveMouseTo(new Point(x, y));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid X and Y");
            }
        }

        private void buttonGotoScreenLocation_Click(object sender, EventArgs e)
        {
            Point oldPos = Cursor.Position;
            int x = int.Parse(textBoxSX.Text);
            int y = int.Parse(textBoxSY.Text);

            if (checkBoxScreenRatioApply.Checked)
            {
                x = (int)(x / getScreenScale());
                y = (int)(y / getScreenScale());
            }

            Cursor.Position = new Point(x, y);
        }

        private void buttonResetAppSize_Click(object sender, EventArgs e)
        {
            HappyChang p = getSelectedProcess();
            p.resetSize();
        }

        private void buttonTestAwardMsg_Click(object sender, EventArgs e)
        {
            Point pos = new Point();
            HappyChang p = getSelectedProcess();
            Bitmap bmp = p.captureScreen();
            p.testIfAwardMessagePrompted(bmp, ref pos);
        }

        private void buttonGiveRedPockets_Click(object sender, EventArgs e)
        {
            HappyChang p = getSelectedProcess();
            p.testGiveRedPockets(1245);
        }

        private void buttonDownMic_Click(object sender, EventArgs e)
        {
            Bitmap bmp = getSelectedProcess().captureScreen();
            if (getSelectedProcess().testIfMicDownButton(bmp))
            {
                Console.WriteLine("Mic Down Detected");
            }
            else
            {
                Console.WriteLine("Mic Down NOT Detected");
            }
        }

        private void buttonOCR_Click(object sender, EventArgs e)
        {
            
            //Bitmap raw = getSelectedProcess().captureScreen();
            //Point[] rooms = new Point[]
            //{
            //    new Point(14,510), new Point(413, 510),
            //    new Point(14,793), new Point(413, 793),
            //    new Point(14,1073), new Point(413, 1073)
            //};
            //Size size = new Size(100, 22); // 97, 22
            //foreach(Point room in rooms)
            //{
            //    TesseractEngine engine = new TesseractEngine(@"C:\Users\Gary\Documents\Development\Visual Studio 2015\Projects\AutoMouseClicks\AutoMouseClicks\tessdata", "eng", EngineMode.Default);
            //    Rectangle cropArea = new Rectangle(room, size);
            //    Bitmap ppl = raw.Clone(getSelectedProcess().appToBitmap(cropArea), raw.PixelFormat);
            //    for(int i = 0; i < ppl.Width; i++)
            //    {
            //        for(int j = 0; j < ppl.Height; j++)
            //        {
            //            Color c = ppl.GetPixel(i, j);
            //            if (c.R < 200 || c.G < 200 || c.B < 200)
            //            {
            //                ppl.SetPixel(i, j, Color.Black);
            //            }
            //        }
            //    }
            //    MemoryStream byteStream = new MemoryStream();
            //    ppl.Save(byteStream, System.Drawing.Imaging.ImageFormat.Tiff);
            //    ppl.Save("test.tiff", System.Drawing.Imaging.ImageFormat.Tiff);
            //    Page page = engine.Process(Pix.LoadFromFile("test.tiff"));
            //    Page page = engine.Process(Pix.LoadTiffFromMemory(byteStream.ToArray()));
            //    ResultIterator iter = page.GetIterator();
            //    iter.Begin();
            //    String text = page.GetText();
            //    Console.WriteLine("OCR Debug: " + text);
            //}
            //TesseractEngine engine = new TesseractEngine(@"C:\Users\Gary\Documents\Development\Visual Studio 2015\Projects\AutoMouseClicks\AutoMouseClicks\tessdata", "eng", EngineMode.Default);
            //int cropWidth = wm.getAppWinWidth();
            //Rectangle cropArea = wm.getAppWindow(0).getAreaPeopleInRoom();
            //Image ppl = cropImage(img, cropArea);
            //ppl.Save("test.tiff", System.Drawing.Imaging.ImageFormat.Tiff);
            //Page page = engine.Process(Pix.LoadFromFile("test.tiff"));
            //ResultIterator iter = page.GetIterator();
            //iter.Begin();
            //String text = page.GetText();
            //Console.WriteLine("OCR Debug: " + text);
            //do
            //{
            //} while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

        }

        private void buttonTestQueueNo_Click(object sender, EventArgs e)
        {
            int qn = getSelectedProcess().getQueueNumber();
            Console.WriteLine("No. of persons in queue: {0}", qn);
        }

        private void buttonGiveRedPocket_Click(object sender, EventArgs e)
        {
            int amount = 0;
            int seconds = 0;
            int numTimes = 0;
            HappyChang p = getSelectedProcess();

            if (getSelectedProcess().GiveRedPocketsOn == false)
            {
                if (int.TryParse(textBoxRedPocketAmount.Text, out amount) && int.TryParse(textBoxRedPocketTime.Text, out seconds) && int.TryParse(numericUpDownQueueTimes.Text, out numTimes))
                {
                    p.GiveRedPocketAmount = amount;
                    p.GiveRedPocketTimes = numTimes;
                    p.GiveRedPocketTimeSpan = TimeSpan.FromSeconds(seconds);

                    Console.WriteLine("Amount: {0}, Times: {1} and Timespan: {2}", p.GiveRedPocketAmount, p.GiveRedPocketTimes, p.GiveRedPocketTimeSpan);
                    p.GiveRedPocketsOn = true;
                    p.startGiveRedPockets();
                }
                else
                {
                    MessageBox.Show("Please specify the parameters for Red Pockets.");
                }
                buttonGiveRedPockets.Text = "Stop Giving Red Pockets";
            }
            else
            {
                p.stopMonitoring();
                buttonGiveRedPockets.Text = "Start Giving Red Pockets";
            }
        }

        private void textBoxRedPocketAmount_Enter(object sender, EventArgs e)
        {
            textBoxRedPocketAmount.SelectAll();
        }

        private void textBoxRedPocketTime_Enter(object sender, EventArgs e)
        {
            textBoxRedPocketTime.SelectAll();
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

        private void buttonTestInactivity_Click(object sender, EventArgs e)
        {
            HappyChang p = getSelectedProcess();
            if (p != null)
            {
                if (p.InactivityAutoExitOn)
                {
                    p.InactivityAutoExitOn = false;
                }
                else
                {
                    p.RedPocketInactivity = TimeSpan.FromMinutes(30);
                    Thread autoExit = new Thread(new ThreadStart(p.inactivityAutoExitWork));
                    autoExit.Start();
                    p.InactivityAutoExitOn = true;
                }
            }
        }

        private Point pictureBoxToScreenPos(Point coordinates)
        {
            float imageAspect = (float)pictureBoxScreenCap.Image.Width / pictureBoxScreenCap.Image.Height;
            float controlAspect = (float)pictureBoxScreenCap.Width / pictureBoxScreenCap.Height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                // This means that we are limited by width, 
                // meaning the image fills up the entire control from left to right
                float ratioWidth = (float)pictureBoxScreenCap.Image.Width / pictureBoxScreenCap.Width;
                newX *= ratioWidth;
                float scale = (float)pictureBoxScreenCap.Width / pictureBoxScreenCap.Image.Width;
                float displayHeight = scale * pictureBoxScreenCap.Image.Height;
                float diffHeight = pictureBoxScreenCap.Height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                // This means that we are limited by height, 
                // meaning the image fills up the entire control from top to bottom
                float ratioHeight = (float)pictureBoxScreenCap.Image.Height / pictureBoxScreenCap.Height;
                newY *= ratioHeight;
                float scale = (float)pictureBoxScreenCap.Height / pictureBoxScreenCap.Image.Height;
                float displayWidth = scale * pictureBoxScreenCap.Image.Width;
                float diffWidth = pictureBoxScreenCap.Width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            Point screenPos = new Point((int)newX, (int)newY);

            return screenPos;
        }

        private void pictureBoxScreenCap_DoubleClick(object sender, EventArgs e)
        {
            //HappyChang hcp = null;
            //MouseEventArgs me = (MouseEventArgs)e;
            //Point coordinates = me.Location;

            //if (pictureBoxScreenCap.Image != null && pictureBoxScreenCap.Width > 0 && pictureBoxScreenCap.Height > 0 &&
            //    pictureBoxScreenCap.Image.Width > 0 && pictureBoxScreenCap.Image.Height > 0)
            //{
            //    Point screenPos = pictureBoxToScreenPos(coordinates);
            //    Console.WriteLine("Location: {0}", screenPos);

            //    Process[] processes = Process.GetProcessesByName("LeapdroidVM");
            //    textBoxSX.Text = String.Format("{0}", screenPos.X);
            //    textBoxSY.Text = String.Format("{0}", screenPos.Y);
            //    foreach (Process p in processes)
            //    {
            //        hcp = new HappyChang(p, getScreenScale());
            //        Rectangle rect = hcp.getPosition();
            //        if (screenPos.X / getScreenScale() >= rect.X && screenPos.X / getScreenScale() < rect.X + rect.Width &&
            //            screenPos.Y / getScreenScale() >= rect.Y && screenPos.Y / getScreenScale() < rect.Y + rect.Height)
            //        {
            //            Size oldSize = new Size((int)(hcp.getPosition().Width * getScreenScale()), (int)(hcp.getPosition().Height * getScreenScale()));
            //            hcp.resize((int)(oldSize.Width * 0.95), (int)(oldSize.Height * 0.95));
            //            Thread.Sleep(100);
            //            hcp.resize(oldSize.Width, oldSize.Height);
            //            break;
            //        }
            //    }
            //    if (getSelectedProcess() == null)
            //    {
            //        KeyValuePair<Int32, String> entry = (KeyValuePair<Int32, String>)comboBoxVMInstances.SelectedItem;

            //        if (entry.Key >= 0 && (procs[entry.Key] == null || procs[entry.Key].hasExited()))
            //        {
            //            bool cont = true;
            //            foreach(HappyChang p in procs)
            //            {
            //                if (p != null && p.Proc.Id == hcp.Proc.Id)
            //                {
            //                    cont = false;
            //                    MessageBox.Show("It's already assigned.", "Claiming application", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                    break;
            //                }
            //            }
            //            if (cont)
            //            {
            //                String question = "Do you want to assign to " + entry.Value;
            //                DialogResult ans = MessageBox.Show(question, "Claiming existing application", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //                if (ans == DialogResult.Yes)
            //                {
            //                    procs[entry.Key] = hcp;
            //                    setButtonsEnable(true);

            //                    buttonStartVM.Text = "Close VM";
            //                    buttonMonitor.Text = "Start Monitoring";
            //                    buttonQueueSongs.Text = "Start Queueing Songs";
            //                    buttonTestGiveRedPockets.Text = "Start Giving Red Pockets";
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void pictureBoxScreenCap_Click(object sender, EventArgs e)
        {
            HappyChang hcp = null;
            MouseEventArgs me = (MouseEventArgs)e;
            Point coordinates = me.Location;

            if (pictureBoxScreenCap.Image != null && pictureBoxScreenCap.Width > 0 && pictureBoxScreenCap.Height > 0 &&
                pictureBoxScreenCap.Image.Width > 0 && pictureBoxScreenCap.Image.Height > 0)
            {
                Point screenPos = pictureBoxToScreenPos(coordinates);

                Process[] processes = Process.GetProcessesByName("NoxVMHandle");// LeapdroidVM");

                bool found = false;
                foreach (Process p in processes)
                {
                    hcp = new HappyChang(p, getScreenScale());
                    Rectangle rect = hcp.getPosition();
                    if (screenPos.X / getScreenScale() >= rect.X && screenPos.X / getScreenScale() < rect.X + rect.Width &&
                        screenPos.Y / getScreenScale() >= rect.Y && screenPos.Y / getScreenScale() < rect.Y + rect.Height)
                    {
                        Size oldSize = new Size((int)(hcp.getPosition().Width * getScreenScale()), (int)(hcp.getPosition().Height * getScreenScale()));
                        hcp.resize((int)(oldSize.Width * 0.95), (int)(oldSize.Height * 0.95));
                        Thread.Sleep(100);
                        hcp.resize(oldSize.Width, oldSize.Height);
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    found = false;
                    for(int i = 0; i < vmList.Count; i++)
                    {
                        HappyChang h = (HappyChang) vmList[i];
                        if (h != null && h.Proc.Id == hcp.Proc.Id)
                        {
                            listBoxVMs.SetSelected(i, true);
                            found = true;
                            break;
                        }

                    }

                    // combobox
                    //for (int i = 0; i < procs.Length; i++)
                    //{
                    //    HappyChang h = procs[i];
                    //    if (h != null && h.Proc.Id == hcp.Proc.Id)
                    //    {
                    //        comboBoxVMInstances.SelectedIndex = i+1;
                    //        found = true;
                    //        break;
                    //    }
                    //}
                }
            }
        }

        private void buttonRefreshStatus_Click(object sender, EventArgs e)
        {
            refreshStatus();
        }

        private void refreshStatus()
        {
            HappyChang p = getSelectedProcess();
            if (p != null)
            {
                if (p.isQueueingSongOn())
                {
                    buttonQueueSongs.Text = "Stop Queueing Songs";
                    labelQueueStatus.Text = String.Format("Queueing {0} of {1} songs.", p.FinishedSongs, p.NumSongsToQueue);
                }
                else
                {
                    buttonQueueSongs.Text = "Start Queueing Songs";
                    labelQueueStatus.Text = "OFF";
                }
                if (p.GiveRedPocketsOn)
                {
                    buttonTestGiveRedPockets.Text = "Stop Giving Red Pockets";
                    labelGiveRedPocketsStatus.Text = String.Format("Giving {0} of {1} times.", p.GiveRedPocketCompletedTimes, p.GiveRedPocketTimes);
                }
                else
                {
                    buttonTestGiveRedPockets.Text = "Start Giving Red Pockets";
                    labelGiveRedPocketsStatus.Text = "OFF";
                }
                if (p.InactivityAutoExitOn)
                {
                    labelAutoCloseRedPockets.Text = String.Format("ON. Last red pockets seen {0:0#}:{1:0#} before.", p.lastRedPocketTime().Minutes, p.lastRedPocketTime().Seconds);
                    buttonAutoExit.Text = "Stop Auto Exit";
                }
                else
                {
                    labelAutoCloseRedPockets.Text = String.Format("OFF. Last red pockets seen {0:0#}:{1:0#} before.", p.lastRedPocketTime().Minutes, p.lastRedPocketTime().Seconds);
                    buttonAutoExit.Text = "Start Auto Exit";
                }
                if (p.isMonitoringOn())
                {
                    labelMonitoringStatus.Text = String.Format("ON");
                }
                else
                {
                    labelMonitoringStatus.Text = String.Format("OFF");
                }
            }
        }

        private void buttonAllMonitoringOn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < vmList.Count; i++)
            {
                HappyChang h = vmList[i];
                if (h != null)
                {
                    if (h.isMonitoringOn() == false)
                    {
                        h.startMonitoring();
                    }
                }
            }

            refreshStatus();
        }

        private void buttonAllMonitoringOff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < vmList.Count; i++)
            {
                HappyChang h = vmList[i];
                if (h != null)
                {
                    if (h.isMonitoringOn())
                    {
                        h.stopMonitoring();
                    }
                }
            }

            refreshStatus();
        }

        private void buttonAllAutoExitOn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < vmList.Count; i++)
            {
                HappyChang h = vmList[i];
                if (h != null && !h.InactivityAutoExitOn)
                {
                    h.RedPocketInactivity = TimeSpan.FromMinutes(60);
                    Thread autoExit = new Thread(new ThreadStart(h.inactivityAutoExitWork));
                    autoExit.Start();
                    h.InactivityAutoExitOn = true;
                }
            }

            refreshStatus();
        }

        private void buttonAllAutoExitOff_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < vmList.Count; i++)
            {
                HappyChang h = vmList[i];
                if (h != null && h.InactivityAutoExitOn)
                {
                    h.InactivityAutoExitOn = false;
                }
            }

            refreshStatus();
        }

        private void buttonClickNTimes_Click(object sender, EventArgs e)
        {
            try
            {
                //int x = int.Parse(textBoxLocationX.Text);
                //int y = int.Parse(textBoxLocationY.Text);
                

                //Point oldPos = Cursor.Position;
                //int x = int.Parse(textBoxSX.Text);
                //int y = int.Parse(textBoxSY.Text);
                int t = (int)numericUpDownQueueTimes.Value;

                //if (checkBoxScreenRatioApply.Checked)
                //{
                //    x = (int)(x / getScreenScale());
                //    y = (int)(y / getScreenScale());
                //}

                Point currPos = Cursor.Position;

                for (int i = 0; i < t; i++)
                {
                    getSelectedProcess().click(new Point(95, 1095));
                    Thread.Sleep(80);
                }

                Cursor.Position = currPos;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid X, Y and times.");
            }

        }

        private void buttonAutoExit_Click(object sender, EventArgs e)
        {
            HappyChang p = getSelectedProcess();
            if (p != null)
            {
                if (p.InactivityAutoExitOn)
                {
                    p.InactivityAutoExitOn = false;
                }
                else
                {
                    p.RedPocketInactivity = TimeSpan.FromMinutes(30);
                    Thread autoExit = new Thread(new ThreadStart(p.inactivityAutoExitWork));
                    autoExit.Start();
                    p.InactivityAutoExitOn = true;
                }
            }

            refreshStatus();
        }

        private void buttonClickAppLoc_Click(object sender, EventArgs e)
        {
            Stopwatch w = new Stopwatch();
            w.Start();

            Point currPos = Cursor.Position;
            try
            {
                int x = int.Parse(textBoxLocationX.Text);
                int y = int.Parse(textBoxLocationY.Text);
                int c = (int) numericUpDownQueueTimes.Value;
                
                for(int i = 0; i < c; i++)
                {
                    getSelectedProcess().click(new Point(x, y));
                    Thread.Sleep(30);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid X and Y");
            }
            Cursor.Position = currPos;
            w.Stop();
            Console.WriteLine("Time Elapsed: {0:0#}:{1:0#}", w.Elapsed.Minutes, w.Elapsed.Seconds);
        }

        private void buttonArrangeWindows_Click(object sender, EventArgs e)
        {

        }

        private void buttonAutoAssign_Click(object sender, EventArgs e)
        {
            List<Process> processes = WindowManager.getRunningProcess("NoxVMHandle"); // LeapdroidVM");
            for (int i = vmList.Count - 1; i >= 0; i--)
            {
                HappyChang h = vmList[i];
                if (h.Proc == null || h.Proc.HasExited)
                {
                    vmList.Remove(h);
                }
            }
            foreach (Process p in processes)
            {
                bool found = false;
                foreach(HappyChang h in vmList)
                {
                    if (h.ProcessId == p.Id)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    HappyChang hc = new HappyChang(p, getScreenScale());
                    vmList.Add(hc);
                }
            }
            listBoxVMs.DataSource = null;
            listBoxVMs.DataSource = vmList;
            listBoxVMs.DisplayMember = "listBoxDisplay";
            listBoxVMs.ValueMember = "ProcessId";
            listBoxVMs.Refresh();
        }

        private void listBoxVMs_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateImage();
            HappyChang h = null;
            if (listBoxVMs.SelectedIndex >= 0)
            {
                h = (HappyChang)listBoxVMs.SelectedItem;

            }
            if (h != null)
            {
                setButtonsEnable(true);
                if (h.isMonitoringOn())
                {
                    buttonMonitor.Text = "Stop Monitoring";
                }
                else
                {
                    buttonMonitor.Text = "Start Monitoring";
                }
                if (h.isQueueingSongOn())
                {
                    buttonQueueSongs.Text = "Stop Queueing Songs";
                    labelQueueStatus.Text = String.Format("Queueing {0} of {1} songs.", h.FinishedSongs, h.NumSongsToQueue);
                }
                else
                {
                    buttonQueueSongs.Text = "Start Queueing Songs";
                    labelQueueStatus.Text = "OFF";
                }
                if (h.GiveRedPocketsOn)
                {
                    buttonTestGiveRedPockets.Text = "Stop Giving Red Pockets";
                    labelGiveRedPocketsStatus.Text = String.Format("Giving {0} of {1} times.", h.GiveRedPocketCompletedTimes, h.GiveRedPocketTimes);
                }
                else
                {
                    buttonTestGiveRedPockets.Text = "Start Giving Red Pockets";
                    labelGiveRedPocketsStatus.Text = "OFF";
                }
                if (h.InactivityAutoExitOn)
                {
                    buttonAutoExit.Text = "Stop Auto Exit";
                }
                else
                {
                    buttonAutoExit.Text = "Start Auto Exit";
                }
            }
            refreshStatus();
        }

        private void HappyChangHelperForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            for(int i = 0; i < vmList.Count; i++)
            {
                HappyChang h = (HappyChang)vmList[i];
                if (h.isMonitoringOn())
                {
                    h.stopMonitoring();
                }
                if (h.isQueueingSongOn())
                {
                    h.stopQueueingSongs();
                }
                if (h.InactivityAutoExitOn)
                {
                    
                }
            }
        }

    }
}

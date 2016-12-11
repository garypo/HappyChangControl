using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Happy_Chang_Player
{
    class HappyChang : NoxAppProcess
    {
        private int processId;

        private Point favRooms = new Point(107, 77);
        private Point[] favRoomList = new Point[4] {
            new Point(400, 188), new Point(400, 318), new Point(400, 448), new Point(400, 578) };
        private Point[,] roomList = new Point[3, 2] {
            { new Point(200, 420), new Point(600, 420) },
            { new Point(200, 700), new Point(600, 700) },
            { new Point(200, 980), new Point(600, 980) }
            };

        private Point roomPassword = new Point(400, 610);
        private Point roomMenuChat = new Point(100, 1175);
        private Point roomMenuGift = new Point(300, 1175);
        private Point roomMenuSing = new Point(500, 1175);
        private Point roomMenuHistory = new Point(700, 1175);

        private Point[] songList = new Point[10]
        {
            new Point(740, 345), new Point(740, 432),
            new Point(740, 519), new Point(740, 607),
            new Point(740, 694), new Point(740, 781),
            new Point(740, 868), new Point(740, 956),
            new Point(740, 1043), new Point(740, 1130)
        };
        private Point replyYes = new Point(313, 675);
        private Point replyNo = new Point(505, 675);
        private Point replyOK = new Point(400, 700);
        private Point closeAwardMsg = new Point(726, 411);
        private Point DownMic = new Point(55, 80);

        private int redPocketWidth = 60;
        private int redPocketHeight = 80;

        private Queue<Point> redPockets;
        private Object redPocketsQueueToken;

        private Thread killRedPocketThread;
        private Thread monitoringThread;
        private bool monitoringOn = false;
        private bool killingOn = false;
        private bool killingRedPocketInProgress = false;

        // Inactivity (No red pockets)
        private Stopwatch stopwatchRedPocket;
        private TimeSpan redPocketInactivity;
        private Thread inactivityAutoExitThread;
        private bool inactivityAutoExitOn = false;

        // Give Red Pockets
        private bool giveRedPocketsOn = false;
        private Thread giveRedPocketsThread;
        private int giveRedPocketAmount = 500;
        private int giveRedPocketTimes = 1;
        private int giveRedPocketCompletedTimes = 0;
        private TimeSpan giveRedPocketTimeSpan = TimeSpan.FromMinutes(5);

        // Queue Songs
        private bool queueSongOn = false;
        private Thread queueSongThread;
        private int numSongsToQueue = 0;
        private int finishedSongs = 0;
        private int suspendQueueLength = 2;

        Stopwatch calcTime = new Stopwatch();

        public HappyChang(ProcessStartInfo startInfo, float screenScale) :base(startInfo, screenScale)
        {
            redPockets = new Queue<Point>();
            redPocketsQueueToken = new object();
            stopwatchRedPocket = new Stopwatch();
            stopwatchRedPocket.Start();
            ProcessId = Proc.Id;
        }

        public HappyChang(Process openedProc, float screenScale) : base(openedProc, screenScale)
        {
            redPockets = new Queue<Point>();
            redPocketsQueueToken = new object();
            stopwatchRedPocket = new Stopwatch();
            stopwatchRedPocket.Start();
            ProcessId = Proc.Id;
        }

        public bool isMonitoringOn()
        {
            return monitoringOn;
        }

        public bool isQueueingSongOn()
        {
            return queueSongOn;
        }

        public bool queueASong()
        {
            bool success = false;
            Point closePt = new Point();
            Point curPos = Cursor.Position;
            //Thread playsoundThread;

            //Assembly assembly = Assembly.GetExecutingAssembly();
            //SoundPlayer simpleSound = new SoundPlayer(assembly.GetManifestResourceStream("AutoMouseClicks.HappyChang.wav"));
            //SoundPlayer simpleSound = new SoundPlayer(@"C:\Users\Gary\Music\HappyChang.wav");

            if (!waitForRoomMenu(TimeSpan.FromSeconds(1)))
            {
                click(backKey);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            while (waitForDimmed(TimeSpan.FromMilliseconds(50))) ;
            click(roomMenuSing);
            //Thread.Sleep(TimeSpan.FromSeconds(2));
            while (waitForDimmed(TimeSpan.FromSeconds(2))) ;
            Random rnd = new Random();
            int songIdx = rnd.Next(0, 5);
            click(songList[songIdx]);
            Console.WriteLine("Queue Song: Waiting for my turn.");
            Cursor.Position = curPos;
            if (waitForMicUp(TimeSpan.FromMinutes(15)))
            {
                click(replyYes);
                Cursor.Position = curPos;
                Console.WriteLine("Queue Song: Yes to MIC UP.");
                if (waitForMusic(TimeSpan.FromSeconds(10)) || waitForMicDownButton(TimeSpan.FromSeconds(10)))
                {
                    Console.WriteLine("Queue Song: Start playing sound and wait for 2 mins.");
                    //playsoundThread = new Thread(new ThreadStart(simpleSound.Play));
                    //playsoundThread.Start();
                    //for(int i = 0; i < 12; i++)
                    //{
                    //    simpleSound.Play();
                    //    Thread.Sleep(TimeSpan.FromSeconds(10));
                    //}
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                    while (waitForDimmed(TimeSpan.FromSeconds(1)) && waitForAwardMessage(TimeSpan.FromSeconds(1), ref closePt))
                    {
                        click(closePt);
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    if (waitForMusic(TimeSpan.FromSeconds(1)) || waitForMicDownButton(TimeSpan.FromSeconds(1)))
                    {
                        click(DownMic);
                        Console.WriteLine("Queue Song: Still Singing and clicked DOWN MIC.");
                        if (waitForQuestion(TimeSpan.FromSeconds(3)))
                        {
                            click(replyYes);
                            Console.WriteLine("Queue Song: Yes to MIC DOWN.");
                        }
                        //simpleSound.Stop();
                        //playsoundThread.Abort();
                    }
                    Cursor.Position = curPos;
                    while (waitForDimmed(TimeSpan.FromSeconds(4)))
                    {
                        if (waitForMessage(TimeSpan.FromSeconds(5)))
                        {
                            click(replyOK);
                            Console.WriteLine("Queue Song: Clicked MESSAGEBOX.");
                        }
                        
                        if (waitForAwardMessage(TimeSpan.FromSeconds(5), ref closePt)){
                            click(closePt);
                            Console.WriteLine("Queue Song: Clicked AWARD BOX.");
                        }
                        if (!waitForDimmed(TimeSpan.FromSeconds(1)))
                        {
                            success = true;
                            break;
                        }
                    }
                    if (!waitForDimmed(TimeSpan.FromSeconds(1)))
                    {
                        success = true;
                    }
                }
            }
            //if (success) { 
            //    Console.WriteLine("Successful.");
            //}
            //else
            //{
            //    Console.WriteLine("Aborted.");
            //}
            Cursor.Position = curPos;

            return success;
        }

        public bool testIfFinishedLoading(Bitmap bmp)
        {
            // check the top red bar. H: 50 and 100 are red
            Color red = Color.FromArgb(225, 67, 68);

            bool result = false;
            int redCount = 0;
            int otherCount = 0;

            Rectangle stdDim = new Rectangle(1, 50, 800, 1);
            Rectangle actualDim = appToBitmap(stdDim);
            int xs = actualDim.X;
            int xe = actualDim.X + actualDim.Width - 1;
            int ys = actualDim.Y;
            int ye = actualDim.Y + actualDim.Height - 1;

            for (int y = ys; y < ye + 1; y++)
            {
                for (int x = xs; x < xe + 1; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == red.ToArgb())
                    {
                        redCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            // test on line#100
            stdDim = new Rectangle(1, 100, 800, 1);
            //actualDim = toActualScaledRegion(stdDim);
            actualDim = appToBitmap(stdDim);
            xs = actualDim.X;
            xe = actualDim.X + actualDim.Width - 1;
            ys = actualDim.Y;
            ye = actualDim.Y + actualDim.Height - 1;

            for (int y = ys; y < ye + 1; y++)
            {
                for (int x = xs; x < xe + 1; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == red.ToArgb())
                    {
                        redCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            if (otherCount == 0)
            {
                result = true;
            }

            return result;
        }

        public bool testIfQuestionPrompted(Bitmap bmp)
        {
            // Line 570, 240-560 all white
            bool result = true;
            int blackCount = 0;
            int whiteCount = 0;
            int redCount = 0;
            float percent = 0.0f;

            Point upperLeft = new Point(240, 570);
            Point lowerRight = new Point(560, 570);
            int row = appToBitmap(upperLeft).Y;
            int xs = appToBitmap(upperLeft).X;
            int xe = appToBitmap(lowerRight).X;
            //int row = 570 * getPhysicalPosition().Height / stdAppCaptureSize.Y - 1 + borderTopCapture;
            //int xs = 240 * getPhysicalPosition().Width / stdAppCaptureSize.X - 1 + borderLeftCapture;
            //int xe = 560 * getPhysicalPosition().Width / stdAppCaptureSize.X - 1 + borderLeftCapture;

            for (int i = xs; i < xe; i++)
            {
                Color c = bmp.GetPixel(i, row);
                if (c.R != 255 || c.G != 255 || c.B != 255)
                {
                    result = false;
                    //Console.WriteLine("DEBUG: YesNoBox Test: (1) has non-white colo: ({0},{1},{2})", c.R,c.G,c.B);
                    break;
                }
            }
            if (result)
            {
                // Line 600, 240-560 white and black
                upperLeft = new Point(240, 600);
                lowerRight = new Point(560, 600);
                row = appToBitmap(upperLeft).Y;
                xs = appToBitmap(upperLeft).X;
                xe = appToBitmap(lowerRight).X;
                //row = 600 * getPhysicalPosition().Height / stdAppCaptureSize.Y - 1 + borderTopCapture;
                for (int i = xs; i < xe; i++)
                {
                    Color c = bmp.GetPixel(i, row);
                    if (c.R == 255 && c.G == 255 && c.B == 255)
                    {
                        whiteCount++;
                        break;
                    }
                    else if (c.R + c.G + c.B == 0)
                    {
                        blackCount++;
                    }
                    percent = (whiteCount + redCount) / (xe - xs + 1);
                    if (percent < 0.75)
                    {
                        result = false;
                    }
                }
            }
            if (result)
            {
                upperLeft = new Point(240, 630);
                lowerRight = new Point(560, 630);
                row = appToBitmap(upperLeft).Y;
                xs = appToBitmap(upperLeft).X;
                xe = appToBitmap(lowerRight).X;
                //row = 630 * getPhysicalPosition().Height / stdAppCaptureSize.Y - 1 + borderTopCapture;
                for (int i = xs; i < xe; i++)
                {
                    Color c = bmp.GetPixel(i, row);
                    if (c.R != 255 || c.G != 255 || c.B != 255)
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        public bool testIfMessagePrompted(Bitmap bmp)
        {
            // Line 609-720, 370-430 red and white
            Color red = Color.FromArgb(224, 51, 52);
            
            HashSet<Color> otherColors = new HashSet<Color>();
            bool result = false;
            int blackCount = 0;
            int whiteCount = 0;
            int redCount = 0;
            int otherCount = 0;
            
            Point upperLeft = appToBitmap(new Point(370, 690));
            Point lowerRight = appToBitmap(new Point(430, 720));
            int xs = upperLeft.X;
            int xe = lowerRight.X;
            int ys = upperLeft.Y;
            int ye = lowerRight.Y;
            for (int y = ys; y < ye + 1; y++)
            {
                for (int x = xs; x < xe + 1; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == Color.White.ToArgb())
                    {
                        whiteCount++;
                    }
                    else if (c.R > c.G + c.B)
                    {
                        redCount++;
                    }
                    else if (c.ToArgb() == Color.Black.ToArgb())
                    {
                        blackCount++;
                        break;
                    }
                    else
                    {
                        otherColors.Add(c);
                        otherCount++;
                    }
                }
            }

            int totalCount = whiteCount + blackCount + redCount + otherCount;
            float whitePerc = (float)whiteCount / totalCount;
            float redPerc = (float)redCount / totalCount;
            if (blackCount == 0)
            {
                if (whitePerc > 0.4 && redPerc > 0.1)
                {
                    result = true;
                }
            }
            //Console.WriteLine("DEBUG: White, Black, Red, Other, W%, R%: {0}, {1}, {2}, {3}, {4:0.00}, {5:0.00}", whiteCount, blackCount, redCount, otherCount, whitePerc, redPerc);
            //Console.WriteLine("DEBUG: Other Color Count: {0}", otherColors.Count);
            //foreach(Color c in otherColors)
            //{
            //    Console.WriteLine("DEBUG: Color (RGB): {0}, {1}, {2}: ", c.R, c.G, c.B);
            //}
            return result;
        }

        public bool testIfMicUpQuestionPrompted(Bitmap b1, Bitmap b2)
        {
            // Dimmed is assumed happened
            bool micUpPrompted = false;

            if (testIfQuestionPrompted(b1) && testIfQuestionPrompted(b2) &&
                b1.Width == b2.Width && b1.Height == b2.Height)
            {
                Point upperLeft = appToBitmap(new Point(270, 660));
                Point lowerRight = appToBitmap(new Point(385, 690));
                int xs = upperLeft.X;
                int xe = lowerRight.X;
                int ys = upperLeft.Y;
                int ye = lowerRight.Y;
                //int xs = toBitmapX(270);
                //int xe = toBitmapY(385);
                //int ys = toBitmapY(660);
                //int ye = toBitmapY(690);

                for (int y = ys - 1; y < ye; y++)
                {
                    for (int x = xs - 1; x < xe; x++)
                    {
                        if (b1.GetPixel(x, y).ToArgb() != b2.GetPixel(x, y).ToArgb())
                        {
                            micUpPrompted = true;
                            break;
                        }
                    }
                    if (micUpPrompted)
                    {
                        break;
                    }
                }
            }

            //if (micUpPrompted) {
            //    Console.WriteLine("DEBUG: Checking MicUp: Mic is up.");
            //}
            //else
            //{
            //    Console.WriteLine("DEBUG: Checking MicUp: Mic is not up.");
            //}
            
            return micUpPrompted;
        }

        public bool testIfAwardMessagePrompted(Bitmap bmp)
        {
            Boolean result = false;
            Color white = Color.FromArgb(246, 246, 247);
            Color brown = Color.FromArgb(252, 189, 61);
            Color green = Color.FromArgb(159, 209, 84);
            Point upperLeft = appToBitmap(new Point(310, 550));// y2 = 530
            Point lowerRight = appToBitmap(new Point(370, 680)); // y2 = 560
            int xs = upperLeft.X;
            int xe = lowerRight.X;
            int ys = upperLeft.Y;
            int ye = lowerRight.Y;

            int whiteCount = 0;
            int brownCount = 0;
            int greenCount = 0;
            int otherCount = 0;

            for (int y = ys - 1; y < ye; y++)
            {
                for(int x = xs - 1; x < xe; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == white.ToArgb())
                    {
                        whiteCount++;
                    }
                    else if (c.ToArgb() == brown.ToArgb())
                    {
                        brownCount++;
                    }
                    else if (c.ToArgb() == green.ToArgb())
                    {
                        greenCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            int totalCount = brownCount + greenCount + whiteCount + otherCount;
            float brownPerc = (float)brownCount / totalCount;
            float greenPerc = (float)greenCount / totalCount;
            float whitePerc = (float)whiteCount / totalCount;
            float otherPerc = (float)otherCount / totalCount;

            if (whitePerc > 0.2 && brownPerc > 0.1 && greenPerc > 0.1)
            {
                result = true;
            }
            Console.WriteLine("DEBUG: Mission Award test - White, Brown, Green, Other: {0}({1:0.##%}), {2}({3:0.##%}), {4}({5:0.##%}), {6}({7:0.##%})", whiteCount, whitePerc, brownCount, brownPerc, greenCount, greenPerc, otherCount, otherPerc);

            return result;
            //DEBUG: Message Mission test 1 - White, Black, Brown, Green, Orange, Blue, Other: 384, 0, 40, 0, 0, 0, 177
            //DEBUG: Message Mission test 2 - White, Black, Brown, Green, Orange, Blue, Other: 601, 0, 0, 0, 0, 0, 0
            //DEBUG: Message Mission test 3 - White, Black, Brown, Green, Orange, Blue, Other: 601, 0, 0, 0, 0, 0, 0
            //DEBUG: Message Mission test 4 - White, Black, Brown, Green, Orange, Blue, Other: 518, 0, 0, 0, 0, 51, 32

            //Boolean result = true;
            //Color white = Color.FromArgb(246, 246, 247);
            //Color brown = Color.FromArgb(164, 88, 20);
            //Color green = Color.FromArgb(159, 209, 84);
            //Color orange = Color.FromArgb(252, 189, 61);
            //Color blue = Color.FromArgb(0, 122, 255);


            //int[] testLine = new int[4] { 512, 540, 700, 787 };

            //int whiteCount = 0;
            //int blackCount = 0;
            //int brownCount = 0;
            //int greenCount = 0;
            //int orangeCount = 0;
            //int blueCount = 0;
            //int otherCount = 0;

            //int xs = toBitmapX(100);
            //int xe = toBitmapX(700);
            //for (int i = 0; i < testLine.Count(); i++)
            //{
            //    HashSet<Color> otherColors = new HashSet<Color>();
            //    int line = toBitmapY(testLine[i]);
            //    for (int x = xs - 1; x < xe; x++)
            //    {
            //        Color c = bmp.GetPixel(x, line - 1);
            //        if (c.ToArgb() == white.ToArgb())
            //        {
            //            whiteCount++;
            //        }
            //        else if (c.ToArgb() == brown.ToArgb())
            //        {
            //            brownCount++;
            //        }
            //        else if (c.ToArgb() == green.ToArgb())
            //        {
            //            greenCount++;
            //        }
            //        else if (c.ToArgb() == orange.ToArgb())
            //        {
            //            orangeCount++;
            //        }
            //        else if (c.ToArgb() == blue.ToArgb())
            //        {
            //            blueCount++;
            //        }
            //        else if (c.ToArgb() == Color.Black.ToArgb())
            //        {
            //            blackCount++;
            //        }
            //        else
            //        {
            //            otherColors.Add(c);
            //            otherCount++;
            //        }
            //    }
            //    int totalCount = testLine.Count() * (xe - xs + 1);
            //    float whitePerc = (float)whiteCount / totalCount;
            //    float brownPerc = (float)brownCount / totalCount;
            //    float bluePerc = (float)blueCount / totalCount;
            //    Console.WriteLine("DEBUG: Message Mission test {0} - White, Black, Brown, Green, Orange, Blue, Other, W%, Br%, Bl%: {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8:0.##}, {9:0.##}, {10:0.##}", i+1, whiteCount,blackCount,brownCount,greenCount,orangeCount,blueCount,otherCount, whitePerc, brownPerc, bluePerc);
            //    foreach (Color c in otherColors)
            //    {
            //        Console.WriteLine("DEBUG: Color (RGB): {0}, {1}, {2}: ", c.R, c.G, c.B);
            //    }
            //    if (testLine[i]==700 || testLine[i] == 540)
            //    {
            //        if (blackCount+brownCount+greenCount+orangeCount+blueCount+otherCount > 0)
            //        {
            //            result = false;
            //        }
            //    }
            //    else if (testLine[i] == 512)
            //    {
            //        if (blackCount + greenCount + orangeCount + blueCount > 0 || whitePerc < 0.5 || brownPerc < 0.04)
            //        {
            //            result = false;
            //        }
            //    }
            //    else if (testLine[i] == 787)
            //    {
            //        if (blackCount + brownCount + greenCount + orangeCount > 0 || whitePerc < 0.65 || bluePerc < 0.05)
            //        {
            //            result = false;
            //        }
            //    }
            //    whiteCount = 0;
            //    blackCount = 0;
            //    brownCount = 0;
            //    greenCount = 0;
            //    orangeCount = 0;
            //    blueCount = 0;
            //    otherCount = 0;
            //}

            //return result;
        }

        const int NO_AWARD_MESSAGE = 0;
        const int LEVEL_UP_MESSAGE = 1;
        const int STAMP_AWARD_MESSAGE = 2;
        const int COIN_AWARD_MESSAGE = 3;
        const int EXP_AWARD_MESSAGE = 4;
        const int EXPCOIN_AWARD_MESSAGE = 5;

        public int testIfAwardMessagePrompted(Bitmap bmp, ref Point closePos)
        {
            int result = NO_AWARD_MESSAGE;
            
            Color bgWhite = Color.FromArgb(246, 246, 247); // for exp and coin award
            Color bgRed1 = Color.FromArgb(229, 101, 72); // for up level
            Color bgRed2 = Color.FromArgb(226, 92, 65); // for up level
            Color bgYellow1 = Color.FromArgb(255, 251, 185); // for stamp award
            Color bgYellow2 = Color.FromArgb(255, 251, 177); // for stamp award

            Color coins = Color.FromArgb(252, 189, 61);
            Color exp = Color.FromArgb(159, 209, 84);

            Point upperLeft = appToBitmap(new Point(1, 580));
            Point lowerRight = appToBitmap(new Point(800, 650));

            int xs = upperLeft.X;
            int xe = lowerRight.X;
            int ys = upperLeft.Y;
            int ye = lowerRight.Y;

            List<Color> pixels = new List<Color>();
            //Console.WriteLine("Bitmap size: {0}x{1}", bmp.Width, bmp.Height);

            for (int x = xs; x < xe - 1 ; x++)
            {
                for (int y = ys; y < ye - 1; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    pixels.Add(c);
                }

            }

            int totalPixels = pixels.Count;

            var colorGroups = pixels.GroupBy(i => i).OrderByDescending(x => x.Count()).Take(4);
            bool generalAwardMsg = false;
            bool experience = false;
            bool coin = false;

            foreach (var group in colorGroups)
            {
                float perc = (float)group.Count() / totalPixels;
                Color c = group.Key;
                if (c.ToArgb() == bgWhite.ToArgb() && perc > 0.65)
                {
                    generalAwardMsg = true;
                }
                else if (c.ToArgb() == bgRed1.ToArgb() && perc > 0.55)
                {
                    result = LEVEL_UP_MESSAGE;
                    break;
                }
                else if (c.ToArgb() == bgYellow1.ToArgb() && perc > 0.35)
                {
                    result = STAMP_AWARD_MESSAGE;
                    break;
                }
                else if (c.ToArgb() == exp.ToArgb() && perc > 0.008)
                {
                    experience = true;
                }
                else if (c.ToArgb() == coins.ToArgb() && perc > 0.008)
                {
                    coin = true;
                }
            }

            if (generalAwardMsg)
            {
                if (experience && coin)
                {
                    result = EXPCOIN_AWARD_MESSAGE;
                }
                else if (experience)
                {
                    result = EXP_AWARD_MESSAGE;
                }
                else if (coin)
                {
                    result = COIN_AWARD_MESSAGE;
                }
            }

            if (result != NO_AWARD_MESSAGE)
            {
                Color bg = new Color();
                switch (result)
                {
                    case LEVEL_UP_MESSAGE:
                        bg = bgRed1;
                        break;
                    case STAMP_AWARD_MESSAGE:
                        bg = bgYellow1;
                        break;
                    case COIN_AWARD_MESSAGE:
                    case EXP_AWARD_MESSAGE:
                    case EXPCOIN_AWARD_MESSAGE:
                        bg = bgWhite;
                        break;
                }

                Point bmpPos = new Point();
                int mY = (ye - ys + 1) / 2 + ys;
                xs = borderLeftCapture;
                xe = bmp.Width - borderRightCapture;
                for(int x = xe - 1; x >= xs; x--)
                {
                    Color pixel = bmp.GetPixel(x, mY);
                    if (pixel.ToArgb() == bg.ToArgb() &&
                        bmp.GetPixel(x - 1, mY).ToArgb() == bg.ToArgb() &&
                        bmp.GetPixel(x - 2, mY).ToArgb() == bg.ToArgb())
                    {
                        bmpPos.X = x;
                        break;
                    }
                }

                int mX = (xe - xs + 1) / 2 + xs;
                ys = borderTopCapture;
                ye = bmp.Height - borderBottomCapture;
                for (int y = ys; y < ye; y++)
                {
                    Color pixel = bmp.GetPixel(mX, y);
                    if (pixel.ToArgb() == bg.ToArgb() &&
                        bmp.GetPixel(mX, y + 1).ToArgb() == bg.ToArgb() &&
                        bmp.GetPixel(mX, y + 2).ToArgb() == bg.ToArgb())
                    {
                        bmpPos.Y = y;
                        break;
                    }
                }
                closePos = bitmapToApp(bmpPos);
            }
            // Locate Close Position
            //Console.WriteLine("DEBUG: Award test - Result: {0}, Close Position: ({1},{2})", result, closePos.X, closePos.Y);
            
            return result;
        }

        public bool testIfMicDownButton(Bitmap bmp)
        {
            bool result = false;

            //15,50 to 95,105
            Point upperLeft = appToBitmap(new Point(15, 50));
            Point lowerRight = appToBitmap(new Point(95, 105));

            Color dark1 = Color.FromArgb(153, 155, 153);
            Color dark2 = Color.FromArgb(156, 156, 156);
            int darkCount = 0;
            int otherCount = 0;
            for (int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == dark1.ToArgb() || c.ToArgb() == dark2.ToArgb())
                    {
                        darkCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            // when music on: Green 130, other 310
            float validPerc = (float)darkCount / (darkCount + otherCount);
            if (validPerc > 0.65)
            {
                result = true;
            }

            //Console.WriteLine("DEBUG: Button - Dark, Other: {0}({1:0.##%)}, {2}", darkCount, validPerc, otherCount);

            return result;
        }

        public bool testIfMusicIsPlaying(Bitmap bmp)
        {
            bool result = false;

            //450,140 to 470,162
            Point upperLeft = appToBitmap(new Point(450, 140));
            Point lowerRight = appToBitmap(new Point(470, 162));

            Color green = Color.FromArgb(37, 194, 84);
            Color red = Color.FromArgb(255, 150, 13);
            int greenCount = 0;
            int redCount = 0;
            int otherCount = 0;
            for(int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for(int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == green.ToArgb())
                    {
                        greenCount++;
                    }
                    else if (c.ToArgb() == red.ToArgb())
                    {
                        redCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }
            //Console.WriteLine("DEBUG: Music Playing - Green, Other: {0}, {1}", greenCount, otherCount);

            // when music on: Green 130, other 310
            float validPerc = (float) (greenCount + redCount) / (greenCount + redCount + otherCount);
            if (validPerc > 0.05)
            {
                result = true;
            }
            return result;
        }

        public bool testIfSingingDetected(Bitmap bmp)
        {
            bool result = false;

            //292,142 to 325,160
            Point upperLeft = appToBitmap(new Point(292, 142));
            Point lowerRight = appToBitmap(new Point(325, 160));

            Color blue = Color.FromArgb(59, 200, 255);
            int blueCount = 0;
            int otherCount = 0;
            for (int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == blue.ToArgb())
                    {
                        blueCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }
            //Console.WriteLine("DEBUG: Music Playing - Blue, Other: {0}, {1}", blueCount, otherCount);

            float bluePerc = (float) blueCount / (blueCount + otherCount);
            if (bluePerc > 0.1)
            {
                result = true;
            }

            return result;
        }

        public bool testIfDimmed(Bitmap bmp)
        {
            // line 1180 to 1200 (check brightness) 330-450
            bool result = true;
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;

            Point upperLeft = appToBitmap(new Point(330, 1180));
            Point lowerRight = appToBitmap(new Point(450, 1200));
            int ys = upperLeft.Y;
            int ye = lowerRight.Y;
            int xs = upperLeft.X;
            int xe = lowerRight.X;

            for (int y = ys; y < ye; y++)
            { 
                for (int x = xs; x < xe; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    sumR += c.R;
                    sumG += c.G;
                    sumB += c.B;
                }
            }
            int totalPixels = (ye - ys + 1) * (xe - xs + 1);
            int total = sumR + sumG + sumB;
            int average = total / totalPixels;
            //Console.WriteLine("DEBUG: Color Sum: {0}, {1:0.##}", sum, (float) sum / (450-330)/(1200-1180));

            // average: normal: 598 to 739, dimmed: 295 to 519 (light dimmed when awarded)

            if (average < 580)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool testIfRoomMenuVisible(Bitmap bmp)
        {
            bool result = false;

            //292,142 to 325,160
            Point upperLeft = appToBitmap(new Point(0, 1150));
            Point lowerRight = appToBitmap(new Point(50, 1200));

            Color bgColor = Color.FromArgb(246, 246, 247);
            int bgCount = 0;
            int otherCount = 0;
            for (int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == bgColor.ToArgb())
                    {
                        bgCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            float bgPerc = (float)bgCount / (bgCount + otherCount);
            if (bgPerc > 0.98)
            {
                result = true;
            }

            return result;
        }

        public bool testIfGiveRedPocketsPage(Bitmap bmp)
        {
            bool result = false;

            //292,142 to 325,160
            Point upperLeft = appToBitmap(new Point(250, 290));
            Point lowerRight = appToBitmap(new Point(550, 335));

            Color bgColor = Color.FromArgb(242, 241, 237);
            Color redColor = Color.FromArgb(223, 48, 49);
            int bgCount = 0;
            int redCount = 0;
            int otherCount = 0;
            for (int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == bgColor.ToArgb())
                    {
                        bgCount++;
                    }
                    else if (c.ToArgb() == redColor.ToArgb())
                    {
                        redCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            int totalCount = bgCount + redCount + otherCount;
            float bgPerc = (float)bgCount / totalCount;
            float redPerc = (float)redCount / totalCount;
            if (bgPerc > 0.65 && redPerc > 0.05)
            {
                result = true;
            }

            return result;
        }

        public bool testIfRoomOptionVisible(Bitmap bmp)
        {
            bool result = false;

            //292,142 to 325,160
            Point upperLeft = appToBitmap(new Point(735, 40));
            Point lowerRight = appToBitmap(new Point(800, 120));

            Color greyColor = Color.FromArgb(48, 48, 48);
            int greyCount = 0;
            int otherCount = 0;
            for (int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == greyColor.ToArgb())
                    {
                        greyCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            int totalCount = greyCount + otherCount;
            float greyPerc = (float)greyCount / totalCount;
            
            if (greyPerc > 0.015)
            {
                result = true;
            }

            return result;
        }

        public bool testIfMenuQueueSelected(Bitmap bmp)
        {
            bool result = false;

            //292,142 to 325,160
            Point upperLeft = appToBitmap(new Point(650, 530));
            Point lowerRight = appToBitmap(new Point(750, 570));

            int whiteCount = 0;
            int otherCount = 0;
            for (int x = upperLeft.X; x < lowerRight.X; x++)
            {
                for (int y = upperLeft.Y; y < lowerRight.Y; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == Color.White.ToArgb())
                    {
                        whiteCount++;
                    }
                    else
                    {
                        otherCount++;
                    }
                }
            }

            int totalCount = whiteCount + otherCount;
            float whitePerc = (float)whiteCount / totalCount;

            if (whitePerc > 0.65 && whitePerc < 0.85)
            {
                result = true;
            }

            return result;
        }

        public bool testGiveRedPockets(int amount)
        {
            bool success = false;

            String keys = String.Format("{0}", amount);
            click(new Point(760, 200));
            Thread.Sleep(TimeSpan.FromSeconds(1));
            click(new Point(760, 80));
            Thread.Sleep(TimeSpan.FromSeconds(1));
            click(new Point(300, 930));
            if (waitForGiveRedPocketsPage(TimeSpan.FromSeconds(2)))
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                click(new Point(200, 235));
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                SendKeys.SendWait(keys);
                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                click(new Point(405, 390));
                Thread.Sleep(TimeSpan.FromSeconds(1));
                while (waitForGiveRedPocketsPage(TimeSpan.FromSeconds(1)))
                {
                    click(backKey);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                success = true;
            }

            return success;
        }

        public int getQueueNumber()
        {
            int queueNo = 0;
            int xs = 1;
            int xWidth = 110;
            int[] y = new int[6] { 625, 715, 805, 895, 985, 1075 };
            
            Color blue = Color.FromArgb(27, 136, 255);
            
            if (waitForQueuePage(TimeSpan.FromSeconds(2)))
            {
                Bitmap bmp = captureScreen();
                for(int i = 0; i < y.Count(); i++)
                {
                    Rectangle dimOrig = new Rectangle(xs, y[i], xWidth, 1);
                    Rectangle dim = appToBitmap(dimOrig);
                    
                    int otherCount = 0;
                    for (int x = dim.X; x <= dim.X + dim.Width; x++)
                    {
                        if (bmp.GetPixel(x, dim.Y).ToArgb() != Color.White.ToArgb() && bmp.GetPixel(x, dim.Y).ToArgb() != blue.ToArgb())
                        {
                            //Console.WriteLine("Loc:({0},{1}), {2}", x, dim.Y, bmp.GetPixel(x, dim.Y));
                            otherCount++;
                        }
                    }
                    if (otherCount > 0)
                    {
                        //Console.WriteLine("Other Count: {0}", otherCount);
                        queueNo++;
                    }
                    else { 
                        break;
                    }
                }
            }

            return queueNo;
        }

        public bool waitForDimmed(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for dimmed");
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed)>0)
            {
                if (testIfDimmed(bmp))
                {
                    //Console.WriteLine("waitForDimmed: dimmed");
                    aborted = false;
                    break;
                }
                else
                {
                    //Console.WriteLine("waitForDimmed: NOT dimmed");
                    bmp.Dispose();
                    bmp = captureScreen();
                    Thread.Sleep(1000);
                }
            }

            bmp.Dispose();
            return !aborted;
        }

        public bool waitForMusic(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for music");
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                if (testIfMusicIsPlaying(bmp))
                {
                    //Console.WriteLine("Music Started");
                    aborted = false;
                    break;
                }
                else
                {
                    //Console.WriteLine("No music");
                    bmp.Dispose();
                    bmp = captureScreen();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForQueuePage(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                if (testIfMenuQueueSelected(bmp))
                {
                    aborted = false;
                    break;
                }
                else
                {
                    bmp.Dispose();
                    bmp = captureScreen();
                    click(new Point(795, 550));
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(500));
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForRoomMenu(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                if (testIfRoomMenuVisible(bmp))
                {
                    aborted = false;
                    break;
                }
                else
                {
                    bmp.Dispose();
                    bmp = captureScreen();
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(500));
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForQuestion(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Waiting for question");
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                if (testIfQuestionPrompted(bmp))
                {
                    Console.WriteLine("Question prompted");
                    aborted = false;
                    break;
                }
                else
                {
                    Console.WriteLine("No question");
                    bmp.Dispose();
                    bmp = captureScreen();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForMicUp(TimeSpan timeout)
        {
            bool aborted = true;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for mic up");
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                //Console.WriteLine("Time elapsed for MicUp: {0}s", stopwatch.Elapsed.TotalSeconds);
                TimeSpan remainingTime = timeout.Subtract(stopwatch.Elapsed);
                Bitmap b1 = captureScreen();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                Bitmap b2 = captureScreen();
                //bmpToFile(b1, "MicUp", "-b1");
                //bmpToFile(b2, "MicUp", "-b2");
                if (testIfMicUpQuestionPrompted(b1, b2))
                {
                    //Console.WriteLine("MicUp prompted");
                    aborted = false;
                    break;
                }
                else
                {
                    Point closePt = new Point();
                    int awardResult = testIfAwardMessagePrompted(b2, ref closePt);
                    if (awardResult != NO_AWARD_MESSAGE)
                    {
                        click(closePt);
                        Console.WriteLine("Got an award({0}).", awardResult);
                    }
                }
                b1.Dispose();
                b2.Dispose();
                //if (aborted)
                //{
                //    if (getQueueNumber() == 0)
                //    {
                //        break;
                //    }
                //}
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1)); // tried 500ms not working
            }

            return !aborted;
        }

        public void bmpToFile(Bitmap bmp, String pre, String suf)
        {
            
            int sn = 0;

            while (File.Exists(pre + String.Format("{0:D3}", sn) + suf + ".bmp"))
            {
                sn++;
            }
            bmp.Save(pre + String.Format("{0:D3}", sn) + suf + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

        }

        public bool waitForMessage(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for message");
            Bitmap bmp = captureScreen();
            try
            {
                while (timeout.CompareTo(stopwatch.Elapsed) > 0)
                {
                    if (testIfMessagePrompted(bmp))
                    {
                        //Console.WriteLine("Message Shown");
                        aborted = false;
                        break;
                    }
                    else
                    {
                        //Console.WriteLine("NO message");
                        bmp.Dispose();
                        bmp = captureScreen();
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }
            } catch (Exception e)
            {

            }
            
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForMicDownButton(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for Mic Down Button");
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                if (testIfMicDownButton(bmp))
                {
                    //Console.WriteLine("Button Shown");
                    aborted = false;
                    break;
                }
                else
                {
                    //Console.WriteLine("Button NOT found");
                    bmp.Dispose();
                    bmp = captureScreen();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForAwardMessage(TimeSpan timeout, ref Point closePt)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for award message");
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                int awardResult = testIfAwardMessagePrompted(bmp, ref closePt);
                if (awardResult != NO_AWARD_MESSAGE)
                {
                    //Console.WriteLine("Award Message Shown");
                    aborted = false;
                    break;
                }
                else
                {
                    //Console.WriteLine("NO award message");
                    bmp.Dispose();
                    bmp = captureScreen();
                    Thread.Sleep(1000);
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForAwardMessage(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for award message");
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                if (testIfAwardMessagePrompted(bmp))
                {
                    //Console.WriteLine("Award Message Shown");
                    aborted = false;
                    break;
                }
                else
                {
                    //Console.WriteLine("NO award message");
                    bmp.Dispose();
                    bmp = captureScreen();
                    Thread.Sleep(1000);
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public bool waitForGiveRedPocketsPage(TimeSpan timeout)
        {
            bool aborted = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Console.WriteLine("Waiting for music");
            Bitmap bmp = captureScreen();
            while (timeout.CompareTo(stopwatch.Elapsed) > 0)
            {
                if (testIfGiveRedPocketsPage(bmp))
                {
                    aborted = false;
                    break;
                }
                else
                {
                    bmp.Dispose();
                    bmp = captureScreen();
                    Thread.Sleep(500);
                }
            }
            bmp.Dispose();

            return !aborted;
        }

        public int scanForRedPocketsSlow(Bitmap bmp)
        {
            Color topColor = Color.FromArgb(252, 217, 28);
            Color LowerLeftColor = Color.FromArgb(255, 86, 58);
            Color LowerRighttColor = Color.FromArgb(240, 67, 45);

            int targetCount = 0;
            int xs = borderLeftCapture;
            int xe = (int) (bmp.Width - borderLeftCapture - borderRightCapture - redPocketWidth * getAppScale() - 1);
            int ys = borderTopCapture;
            int ye = (int)(bmp.Height - borderTopCapture - borderBottomCapture - redPocketHeight * getAppScale() - 1);
            int newRedPocketWidth = (int) (redPocketWidth * getAppScale());
            int newRedPocketHeight = (int)(redPocketHeight * getAppScale());
            Point currPos = Cursor.Position;

            for (int y = ys; y < ye; y++)
            {
                for (int x = xs; x < xe; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    if (c.ToArgb() == topColor.ToArgb())
                    {
                        Color ll = bmp.GetPixel(x, (int)(y + newRedPocketHeight));
                        Color lr = bmp.GetPixel((int)(x + newRedPocketWidth), (int)(y + newRedPocketHeight));
                        if (ll.ToArgb() == LowerLeftColor.ToArgb() &&
                            lr.ToArgb() == LowerRighttColor.ToArgb())
                        {
                            Point target = new Point();
                            //target.X = (int)(x + newRedPocketWidth / 2 - borderLeftCapture);
                            //target.Y = (int)(y + newRedPocketHeight / 2 - borderTopCapture);
                            target.X = x + newRedPocketWidth / 2;
                            target.Y = y + newRedPocketHeight / 2;
                            click(bitmapToApp(target));
                            //if (targetCount == 0)
                            //{
                            //    // With new image we can clear the outdated targets
                            //    clearRedPockets();
                            //}
                            //addRedPocket(target);
                            //lock (redPocketsQueueToken)
                            //{
                            //    redPockets.Enqueue(target);
                            //}
                            targetCount++;

                            y += newRedPocketHeight;    // skip several lines
                            stopwatchRedPocket.Restart();
                            break; // skip the rest of X
                        }
                    }
                }
            }

            Cursor.Position = currPos;

            return targetCount;
        }

        public int scanForRedPockets(Bitmap bmp)
        {
            Color topColor = Color.FromArgb(252, 217, 28);
            Color LowerLeftColor = Color.FromArgb(255, 86, 58);
            Color LowerRighttColor = Color.FromArgb(240, 67, 45);

            int targetCount = 0;
            int xs = borderLeftCapture;
            int xe = (int)(bmp.Width - borderLeftCapture - borderRightCapture - 1);
            int ys = borderTopCapture;
            int ye = (int)(bmp.Height - borderTopCapture - borderBottomCapture - 1);
            int newRedPocketWidth = (int)(redPocketWidth * getAppScale());
            int newRedPocketHeight = (int)(redPocketHeight * getAppScale());
            unsafe
            {
                try
                {
                    BitmapData bitmapData = bmp.LockBits(new Rectangle(xs, ys, xe-xs+1, ye-ys+1), ImageLockMode.ReadOnly, bmp.PixelFormat);
                    int bytesPerPixel = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
                    int heightInPixels = bitmapData.Height;
                    int widthInBytes = bitmapData.Width * bytesPerPixel;
                    byte* ptrFirstPixel = (byte*)bitmapData.Scan0;

                    for (int y = heightInPixels - 1; y >= newRedPocketHeight; y--)
                    {
                        byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
                        for (int x = 0; x < widthInBytes - newRedPocketWidth * bytesPerPixel; x = x + bytesPerPixel)
                        {
                            int blue = currentLine[x];
                            int green = currentLine[x + 1];
                            int red = currentLine[x + 2];

                            if (LowerLeftColor.R == red && LowerLeftColor.G == green && LowerLeftColor.B == blue)
                            {
                                int rX = x + newRedPocketWidth * bytesPerPixel;
                                int uY = y - newRedPocketHeight;

                                blue = currentLine[rX];
                                green = currentLine[rX + 1];
                                red = currentLine[rX + 2];
                                if (LowerRighttColor.R == red && LowerRighttColor.G == green && LowerRighttColor.B == blue)
                                {
                                    byte* upperLine = ptrFirstPixel + (uY * bitmapData.Stride);
                                    blue = upperLine[rX];
                                    green = upperLine[rX + 1];
                                    red = upperLine[rX + 2];
                                    if (topColor.R == red && topColor.G == green && topColor.B == blue)
                                    {
                                        Point p = new Point();
                                        Point target = new Point();
                                        p.X = (int)(x / bytesPerPixel + newRedPocketWidth / 2) + borderLeftCapture;
                                        p.Y = (int)(y - newRedPocketHeight / 2) + borderTopCapture;
                                        target = bitmapToApp(p);
                                        click(target);
                                        targetCount++;
                                        //Console.WriteLine("Target #{0}: {1}, Time: {2}", targetCount, target, calcTime.Elapsed);
                                        killingRedPocketInProgress = true;
                                        //Console.WriteLine("Target (x, y): ({0}, {1}): ", target.X, target.Y);
                                        y -= newRedPocketHeight;    // skip several lines
                                        
                                        break; // skip the rest of X
                                    }
                                }
                            }
                        }
                    }
                    if (targetCount > 0)
                    {
                        stopwatchRedPocket.Restart();
                    }
                    bmp.UnlockBits(bitmapData);
                }
                catch (Exception e)
                {
                    // ignore
                }
            }

            killingRedPocketInProgress = false;

            return targetCount;
        }

        private void monitorRedPockets()
        {
            int count = 0;

            Console.WriteLine("Monitoring Thread STARTED.");
            calcTime.Start();

            while (this.isMonitoringOn())
            {
                Bitmap bmp = captureScreen();
                count = scanForRedPockets(bmp);
                if (count > 0)
                {
                    //Console.WriteLine("Find {0} Red Pocket(s)", count);
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                bmp.Dispose();
            }
            Console.WriteLine("Monitoring Thread ENDED.");
            calcTime.Stop();
        }

        public void startMonitoring()
        {
            if (isMonitoringOn())
            {
                monitoringThread.Abort();
            }

            killRedPocketThread = new Thread(new ThreadStart(killRedPockets));
            monitoringThread = new Thread(new ThreadStart(monitorRedPockets));
            killRedPocketThread.Start();
            monitoringThread.Start();
            killingOn = true;
            monitoringOn = true;
        }

        public void stopMonitoring()
        {
            if (isMonitoringOn())
            {
                monitoringThread.Abort();
                monitoringOn = false;
                killingOn = false;
            }
            Console.WriteLine("Monitoring Thread ENDED.");
        }

        public void killRedPockets()
        {
            Console.WriteLine("Kill Red Pockets Thread STARTED.");
            Point target = new Point();
            while (killingOn)
            {
                int targetCount = 0;

                lock (redPocketsQueueToken)
                {
                    targetCount = redPockets.Count();
                    if (targetCount > 0)
                    {
                        target = redPockets.Dequeue();
                    }
                }

                if (targetCount > 0)
                {
                    click(target);
                    Console.WriteLine("Kill 1 Red Pocket.");
                    Thread.Sleep(TimeSpan.FromMilliseconds(30));
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            Console.WriteLine("Kill Red Pockets Thread ENDED.");
        }

        public bool GiveRedPocketsOn
        {
            get
            {
                return giveRedPocketsOn;
            }

            set
            {
                giveRedPocketsOn = value;
            }
        }

        public int GiveRedPocketAmount
        {
            get
            {
                return giveRedPocketAmount;
            }

            set
            {
                giveRedPocketAmount = value;
            }
        }

        public int GiveRedPocketTimes
        {
            get
            {
                return giveRedPocketTimes;
            }

            set
            {
                giveRedPocketTimes = value;
            }
        }

        public TimeSpan GiveRedPocketTimeSpan
        {
            get
            {
                return giveRedPocketTimeSpan;
            }

            set
            {
                giveRedPocketTimeSpan = value;
            }
        }

        public int FinishedSongs
        {
            get
            {
                return finishedSongs;
            }

            set
            {
                finishedSongs = value;
            }
        }

        public bool QueueSongOn
        {
            get
            {
                return queueSongOn;
            }

            set
            {
                queueSongOn = value;
            }
        }

        public int NumSongsToQueue
        {
            get
            {
                return numSongsToQueue;
            }

            set
            {
                numSongsToQueue = value;
            }
        }

        public int SuspendQueueLength
        {
            get
            {
                return suspendQueueLength;
            }

            set
            {
                suspendQueueLength = value;
            }
        }

        public int GiveRedPocketCompletedTimes
        {
            get
            {
                return giveRedPocketCompletedTimes;
            }

            set
            {
                giveRedPocketCompletedTimes = value;
            }
        }

        public TimeSpan RedPocketInactivity
        {
            get
            {
                return redPocketInactivity;
            }

            set
            {
                redPocketInactivity = value;
            }
        }

        public bool InactivityAutoExitOn
        {
            get
            {
                return inactivityAutoExitOn;
            }

            set
            {
                inactivityAutoExitOn = value;
            }
        }

        public int ProcessId
        {
            get
            {
                return processId;
            }

            set
            {
                processId = value;
            }
        }

        public void giveRedPockets()
        {
            int numSuccess = 0;
            giveRedPocketCompletedTimes = 0;
            for (int i = 0; i < GiveRedPocketTimes; i++)
            {
                if (giveRedPocketsOn)
                {
                    if (testGiveRedPockets(giveRedPocketAmount))
                    {
                        numSuccess++;
                    }
                    giveRedPocketCompletedTimes++;
                    Thread.Sleep(GiveRedPocketTimeSpan);
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Given {0} times Red Pockets and Exits", numSuccess);

            giveRedPocketsOn = false;
        }

        public void startGiveRedPockets()
        {
            giveRedPocketsThread = new Thread(new ThreadStart(giveRedPockets));
            giveRedPocketsThread.Start();
            giveRedPocketsOn = true;
        }

        public void stopGiveRedPockets()
        {
            if (GiveRedPocketsOn)
            {
                giveRedPocketsOn = false;
                giveRedPocketsThread.Abort();
            }
            Console.WriteLine("Give Red Pockets Thread ENDED.");
        }

        public void startAutoExit()
        {
            if (inactivityAutoExitThread != null && inactivityAutoExitThread.IsAlive)
            {
                inactivityAutoExitThread.Abort();
            }
            inactivityAutoExitThread = new Thread(new ThreadStart(inactivityAutoExitWork));
            inactivityAutoExitOn = true;
        }

        public void stopAutoExit()
        {
            if (inactivityAutoExitThread != null && inactivityAutoExitThread.IsAlive)
            {
                inactivityAutoExitThread.Abort();
            }
            inactivityAutoExitOn = false;

        }

        public void queueSongs()
        {
            finishedSongs = 0;
            for (int i = 0; i < numSongsToQueue; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int queueLength = getQueueNumber();
                while (queueLength >= suspendQueueLength)
                {
                    TimeSpan waitTime = TimeSpan.FromSeconds(30 * queueLength);
                    Console.WriteLine("DEBUG: Queue has more than {0} person, Wait for ({1} more).", suspendQueueLength - 1, waitTime);
                    Thread.Sleep(waitTime);
                    Point closePt = new Point();
                    while (waitForDimmed(TimeSpan.FromSeconds(1)) && waitForAwardMessage(TimeSpan.FromSeconds(1), ref closePt))
                    {
                        click(closePt);
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    queueLength = getQueueNumber();
                }
                Console.WriteLine("DEBUG: Queueing {0} of {1} song(s).", i + 1, numSongsToQueue);
                if (queueASong())
                {
                    finishedSongs++;
                }
                else
                {
                    QueueSongOn = false;
                    Console.WriteLine("Queue Song ABORTED.");
                    break;
                }
            }
        }

        public void startQueueingSongs(int numSongs)
        {
            if (isQueueingSongOn())
            {
                queueSongThread.Abort();
            }
            numSongsToQueue = numSongs;
            queueSongThread = new Thread(new ThreadStart(queueSongs));
            queueSongThread.Start();
            queueSongOn = true;
        }

        public void stopQueueingSongs()
        {
            queueSongOn = false;
            if (queueSongThread != null && queueSongThread.IsAlive)
            {
                queueSongThread.Abort();
            }
        }

        public void addRedPocket(Point p)
        {
            lock (redPocketsQueueToken)
            {
                this.redPockets.Enqueue(p);
            }
        }

        public Point consumeRedPocket()
        {
            Point p;
            lock (redPocketsQueueToken)
            {
                p = this.redPockets.Dequeue();
            }

            return p;
        }

        public void clearRedPockets()
        {
            lock (redPocketsQueueToken)
            {
                this.redPockets.Clear();
            }
        }

        public TimeSpan lastRedPocketTime()
        {
            return stopwatchRedPocket.Elapsed;
        }

        public void inactivityAutoExitWork()
        {
            stopwatchRedPocket.Start();
            while (inactivityAutoExitOn)
            {
                TimeSpan elapsed = stopwatchRedPocket.Elapsed;
                if (elapsed.CompareTo(redPocketInactivity) >= 0)
                {
                    Random rnd = new Random();
                    int randomDelay = rnd.Next(0, 120);
                    Thread.Sleep(TimeSpan.FromSeconds(randomDelay));
                    for (int i = 0; i < 10; i++)
                    {
                        click(backKey);
                        Thread.Sleep(50);
                        click(backKey);
                        Thread.Sleep(100);
                    }
                    stopwatchRedPocket.Reset();
                    Console.WriteLine("Inactivity exit");
                    break;
                }
                else
                {
                    TimeSpan timeBeforeExit = redPocketInactivity.Subtract(elapsed);
                    Console.WriteLine("Time since last red pocket: {0}", elapsed);
                    Console.WriteLine("{0} before exit", timeBeforeExit);
                    Thread.Sleep(timeBeforeExit);
                }
            }
            stopMonitoring();
            stopQueueingSongs();
            inactivityAutoExitOn = false;
            //close();
        }

        public String listBoxDsiplay()
        {
            String idLoc = String.Format("{0} - ({1}, {2})", this.Proc.Id, this.getPosition().X, this.getPosition().Y);

            return idLoc;
        }

    }
}

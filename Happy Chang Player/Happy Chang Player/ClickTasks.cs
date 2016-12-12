using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Happy_Chang_Player
{
    class ClickTasks
    {
        private Queue<Point> targets;
        private List<Point> potentialTargets;
        private Object tokenQueue;
        private Object tokenList;
        private Object tokenKillThreads;
        private int _pocketSize;
        private float _screenScale;
        private List<Thread> killThreads;

        public ClickTasks(int pocketSize, float screenScale)
        {
            targets = new Queue<Point>();
            potentialTargets = new List<Point>();
            tokenQueue = new Object();
            tokenList = new Object();
            tokenKillThreads = new Object();
            _pocketSize = pocketSize;
            _screenScale = screenScale;
            killThreads = new List<Thread>();
        }

        public void addTarget(Point p)
        {
            Boolean isNew = true;
            lock (tokenList)
            {
                foreach(Point pt in potentialTargets)
                {
                    if (Math.Abs(pt.X - p.X) < _pocketSize && Math.Abs(pt.Y - p.Y) < _pocketSize)
                    {
                        isNew = false;
                    }
                }
                if (isNew)
                {
                    potentialTargets.Add(p);
                }
            }
            if (isNew)
            {
                lock (tokenQueue)
                {
                    targets.Enqueue(p);
                }
            }
        }

        public int getNumOfTargets()
        {
            return targets.Count;
        }

        public void killTargets()
        {
            int tasks = 0;
            lock (tokenQueue)
            {
                tasks = targets.Count;
                Console.WriteLine("killTargets - " + tasks);
            }
            lock (tokenKillThreads)
            {
                if (killThreads.Count < Math.Ceiling((decimal)targets.Count / 6))
                {
                    Thread t = new Thread(new ThreadStart(killTargetsThread));
                    t.Start();
                    killThreads.Add(t);
                }
            }
        }

        private void killTargetsThread()
        {
            Console.WriteLine("Kill Thread Started.");

            Point p;
            TimeSpan start = DateTime.Now.TimeOfDay;

            while (true)
            {
                lock (tokenQueue)
                {
                    p = targets.Dequeue();
                }
                if (p != null)
                {
                    Cursor.Position = new Point((int)(p.X / _screenScale), (int)((p.Y + _pocketSize / 2) / _screenScale));

                    MouseSimulator.ClickLeftMouseButton();
                    Console.WriteLine("Kill");
                }
                else
                {
                    TimeSpan current = DateTime.Now.TimeOfDay;
                    if (current.Subtract(start).TotalSeconds >= 2)
                    {
                        break;
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Happy_Chang_Player
{
    class BitmapAnalysis
    {
        static public int specificColorAnalysis(Bitmap bmp, Rectangle region, float threshold)
        {
            List<Color> pixels = new List<Color>();
            Console.WriteLine("Bitmap size: {0}x{1}", bmp.Width, bmp.Height);
            Console.WriteLine("(X,Y,W,H): {0},{1} {2}x{3}", region.X, region.Y, region.Width, region.Height);

            for (int x = region.X; x < region.X + region.Width; x++)
            {
                for (int y = region.Y; y < region.Y + region.Height; y++)
                {
                    Color c = bmp.GetPixel(x-1, y-1);
                    pixels.Add(c);
                }

            }

            int totalPixels = pixels.Count;

            int sn = 0;
            var colorGroups = pixels.GroupBy(i => i).OrderByDescending(x => x.Count());

            //var groups = pixels.GroupBy(info => info.ToArgb())
            //    .Select(group => new { group.Key, Count = group.Count() })
            //    .OrderByDescending(x => x.Count);
            //foreach (var element in groups)
            foreach(var group in colorGroups)
            {
                sn++;
                float perc = (float)group.Count() / totalPixels;
                if (perc >= threshold)
                {
                    Console.WriteLine("#{00} - RGB({1},{2},{3},{4}): {5}({6:0.0#%})", sn, group.Key.R, group.Key.G, group.Key.B, group.Key.A, group.Count(), perc);
                }
                else
                {
                    break;
                }
            }

            return colorGroups.Count();
        }

        static public int averageColorAnalysis(Bitmap bmp, Rectangle region)
        {
            List<Color> pixels = new List<Color>();
            Console.WriteLine("Bitmap size: {0}x{1}", bmp.Width, bmp.Height);
            Console.WriteLine("(X,Y,W,H): {0},{1} {2}x{3}", region.X, region.Y, region.Width, region.Height);
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;

            for (int x = region.X; x < region.X + region.Width; x++)
            {
                for (int y = region.Y; y < region.Y + region.Height; y++)
                {
                    Color c = bmp.GetPixel(x - 1, y - 1);
                    sumR += c.R;
                    sumG += c.G;
                    sumB += c.B;
                }
            }

            int totalPixels = region.Width * region.Height;
            int total = sumR + sumG + sumB;
            int average = total / totalPixels;

            Console.WriteLine("(R,G,B), sum, average: ({0},{1},{2}), {3}, {4}", sumR, sumG, sumB, total, average);

            return average;
        }
    }

}

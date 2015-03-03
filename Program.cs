using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SharpDXTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            TimeSpan timeTaken = TimeTaken.For(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Task t = Task.Run(() =>
                        {
                            ImageCreator iw = new ImageCreator(1000, 500, 200);

                            using (MemoryStream ms = new MemoryStream())
                           //using (FileStream fs = new FileStream("C:\\D2DOut.jpg", FileMode.OpenOrCreate))
                            {
                                iw.SaveRenderedImage(ms, Direct2DImageFormatType.Jpeg);
                            }

                            iw.Dispose();
                        });
                    tasks.Add(t);
                }

                Task.WaitAll(tasks.ToArray());
                foreach (Task t in tasks)
                {
                    if(t.Status != TaskStatus.RanToCompletion)
                        Console.WriteLine("ERROR In Task {0} Status: {1}", t.Id, t.Status);
                }
            });

            Console.WriteLine("Generated {0} sub images in {1} ms.", tasks.Count, timeTaken.TotalMilliseconds);
            Console.ReadLine();
        }


    }

    public static class TimeTaken
    {
        public static TimeSpan For(Action operation)
        {
            Stopwatch watch = new Stopwatch();
            try
            {
                watch.Start();
                operation();
            }
            finally
            {
                watch.Stop();
            }
            return watch.Elapsed;
        }
    }
}

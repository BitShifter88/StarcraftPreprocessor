using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CrashHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            CpuMonitor monitor = new CpuMonitor();

            Queue<float> measures = new Queue<float>();

            while (true)
            {
                Thread.Sleep(500);
                measures.Enqueue(monitor.getCurrentCpuUsage());

                if (measures.Count == 20)
                {
                    //if (
                    measures.Dequeue();
                }
                Console.WriteLine(monitor.getCurrentCpuUsage());
            }
        }


    }
}

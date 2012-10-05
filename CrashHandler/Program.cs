using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace CrashHandler
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        static void Main(string[] args)
        {
            CpuMonitor monitor = new CpuMonitor();

            Queue<float> measures = new Queue<float>();

            Console.Write("Trying to detect StarCraft crash...");

            while (true)
            {
                Thread.Sleep(500);
                measures.Enqueue(monitor.getCurrentCpuUsage());

                if (measures.Count == 20)
                {
                    List<float> l = measures.OrderByDescending(i => i).ToList();
                    if (l[0] < 25)
                    {
                        Console.WriteLine("Crash detected!");
                        Console.WriteLine("Taking screenshot of crash..");
                        TakeScreenShot();

                        string[] data = File.ReadAllLines(@"C:\Users\admin\Desktop\StarCraft\StarCraft\Starcraft\output.txt");

                        Console.WriteLine("Reading output file..");
                        string replay = data[data.Length - 1].Split(',')[1];

                        Console.WriteLine("Replay " + replay + " caused the crash!");
                        Process.GetProcesses().First(i => i.ProcessName == "StarCraft").Kill();

                        Console.WriteLine("Starcraft process killed!");
                        Thread.Sleep(500);

                        File.Delete(@"C:\Users\admin\Desktop\StarCraft\StarCraft\Starcraft\maps\replays\" + replay);
                        Console.WriteLine("Bad Replay deleted");

                        File.Delete(@"C:\Users\admin\Desktop\StarCraft\StarCraft\Starcraft\output.txt");
                        Console.WriteLine("output.txt deleted");

                        Process.Start(@"C:\Users\admin\Desktop\StarCraft\StarCraft\Chaoslauncher\Chaoslauncher.exe");
                        Console.WriteLine("Chaoslauncher started!");

                        Thread.Sleep(5000);
                        SimulateEnter();
                        Console.WriteLine("Simulated enter click!");
                        Thread.Sleep(60000 * 2);
                    }
                    measures.Clear();
                }
            }

            Console.ReadLine();
        }

        private static void SimulateEnter()
        {
            Process[] processes = Process.GetProcessesByName("Chaoslauncher");

            foreach (Process proc in processes)
            {
                SetForegroundWindow(proc.MainWindowHandle);
                SendKeys.SendWait("{ENTER}");
            }

        }

        private static void TakeScreenShot()
        {
            Bitmap bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            // Create a graphics object from the bitmap
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            // Take the screenshot from the upper left corner to the right bottom corner
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            // Save the screenshot to the specified path that the user has chosen
            bmpScreenshot.Save(new Random().Next(0,100000000) + ".png", ImageFormat.Png);
        }
    }
}

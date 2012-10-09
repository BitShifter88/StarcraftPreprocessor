using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OutputMover
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading output file...");
            string[] data = File.ReadAllLines(@"C:\Users\admin\Desktop\StarCraft\StarCraft\Starcraft\output.txt");

            MoveGoodReplays(GetAllGoodReplays(data));

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        private static void MoveGoodReplays(List<string> replays)
        {
            foreach (string replay in replays)
            {
                Console.WriteLine("moving: " + replay);
                File.Move(@"C:\Users\admin\Desktop\StarCraft\StarCraft\Starcraft\maps\replays\" + replay, @"goodReplays\" + replay);
            }
            Console.WriteLine("Good replays moved to safe place!");
            File.Copy(@"C:\Users\admin\Desktop\StarCraft\StarCraft\Starcraft\output.txt", @"goodReplays\output.txt");
        }

        private static List<string> GetAllGoodReplays(string[] data)
        {
            List<string> replays = new List<string>();
            foreach (string line in data)
            {
                string replay = line.Split(',')[1];
                if (replays.Contains(replay) == false)
                    replays.Add(replay);
            }
            replays.Remove(replays[replays.Count - 1]);
            return replays;
        }
    }
}

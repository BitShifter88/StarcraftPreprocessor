using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace StarcraftParser
{
    enum CsvType : int
    {
        Weka = 2,
        Excel = 1,
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("How would you like to handle dublicates?\n(1) Do nothing\n(2) Remove game dublicates\n(3) Remove replay dublicates\n(4) Remove all dublicates");
            int removeDub = int.Parse(Console.ReadLine());

            Console.WriteLine("Choose processor:\nTime Slice Preprocessor (1)\nFirst Time Build Preprocessor (2)");
            int processor = int.Parse(Console.ReadLine());
            Console.WriteLine("Choose CSV output format:\nExcel csv (1)\nWeka csv (2)");
            int csv = int.Parse(Console.ReadLine());

            Parser p = new Parser();
            // Parses the raw log file from Mikkels replay parser. It returns a list of ScGames, which is simply a C# representation of a game event log.
            List<ScGame> games = p.Parse("input.csv");

            // Because ScGame is just a C# representation of a game event log, we need to convert it to a more appropriate format in order to do data analysis.
            // In this instance, the VectorProcessor class is used. It converts the game event log of a ScGame game, into a list of game state vectors.
            // A game state vector will contain a list of all units produced within a timeperiod. If timeGranularity is set to 30, and timeSlice is set to 4
            // This will produce 4 game state vectors, each with a length of 30 seconds
            TimeSliceProcessor vp = new TimeSliceProcessor();
            vp.BuildUnitList(games);
            FirstTimeBuildProcessor ftbp = new FirstTimeBuildProcessor();
            ftbp.BuildUnitList(games);

            Console.WriteLine("\n");

            Console.WriteLine(games.Count + " games in the input file");


            int prevCount = games.Count;
            if (removeDub == 2)
                p.RemoveGameDublicates(games);
            else if (removeDub == 3)
                p.RemoveReplayDublicates(games);
            else if (removeDub == 4)
            {
                p.RemoveGameDublicates(games);
                p.RemoveReplayDublicates(games);
            }
            Console.WriteLine(prevCount - games.Count + " game dublicates removed");
            Console.WriteLine(games.Count + " total left");

            if (processor == 1)
            {
                List<TimeSliceGame> pGames = new List<TimeSliceGame>();

                foreach (ScGame game in games)
                {
                    pGames.Add(vp.ProcessGame(game, 90, 6));
                }

                //pGames =  vp.Normalize(pGames);

                vp.WriteGamesToCsv(pGames.Where(i => i.Race == Race.Terran).ToList(), "output/terranGames.csv", false, (CsvType)csv);
                vp.WriteGamesToCsv(pGames.Where(i => i.Race == Race.Protoss).ToList(), "output/protossGames.csv", false, (CsvType)csv);
                vp.WriteGamesToCsv(pGames.Where(i => i.Race == Race.Zerg).ToList(), "output/zergGames.csv", false, (CsvType)csv);
            }
            else if (processor == 2)
            {
                List<UnitTimeVector> pGames = new List<UnitTimeVector>();

                foreach (ScGame game in games)
                {
                    pGames.Add(ftbp.ProcessGame(game));
                }

                ftbp.WriteGamesToCsv(pGames.Where(i => i.Race == Race.Terran).ToList(), "output/terranGames.csv", (CsvType)csv);
                ftbp.WriteGamesToCsv(pGames.Where(i => i.Race == Race.Protoss).ToList(), "output/protossGames.csv", (CsvType)csv);
                ftbp.WriteGamesToCsv(pGames.Where(i => i.Race == Race.Zerg).ToList(), "output/zergGames.csv", (CsvType)csv);
            }

            Console.WriteLine("Done!");

            Console.ReadLine();
        }
    }
}

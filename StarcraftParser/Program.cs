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
            //Console.WriteLine("Would you like to use the Time Slice Preprocessor (1), or the First Time Build Preprocessor (2)?");
            //int processor = int.Parse(Console.ReadLine());
            //Console.WriteLine("Should the result be saven in an Excel csv (1) or Weka csv (2)?");
            //int csv = int.Parse(Console.ReadLine());
            int processor = 1;
            int csv = 1;

            Parser p = new Parser();
            // Parses the raw log file from Mikkels replay parser. It returns a list of ScGames, which is simply a C# representation of a game event log.
            List<ScGame> games = p.Parse("input.csv");
            
            List<ScEvent> possibleRoots = new List<ScEvent>();
            foreach (ScGame game in games)
            {
                ScEvent r = game.Events[0];
                if (r == null) continue;
                IEnumerable<ScEvent> q = possibleRoots.Where(e => e.Unit == r.Unit);
                if (q.Count() == 0) possibleRoots.Add(r);
            }

            NodeList<ScEvent> roots = new NodeList<ScEvent>(possibleRoots);

            


            // Because ScGame is just a C# representation of a game event log, we need to convert it to a more appropriate format in order to do data analysis.
            // In this instance, the VectorProcessor class is used. It converts the game event log of a ScGame game, into a list of game state vectors.
            // A game state vector will contain a list of all units produced within a timeperiod. If timeGranularity is set to 30, and timeSlice is set to 4
            // This will produce 4 game state vectors, each with a length of 30 seconds
            TimeSliceProcessor vp = new TimeSliceProcessor();
            vp.BuildUnitList(games);
            FirstTimeBuildProcessor ftbp = new FirstTimeBuildProcessor();
            ftbp.BuildUnitList(games);
            //GraphProcessor gp = new GraphProcessor();
            //gp.BuildUnitList(games);
            //List<ScGraph<ScGraphNode>> graph = gp.ProcessGames(games);


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

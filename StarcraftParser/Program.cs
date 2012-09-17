using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace StarcraftParser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Parser p = new Parser();
            // Parses the raw log file from Mikkels replay parser. It returns a list of ScGames, which is simply a C# representation of a game event log.
            List<ScGame> games = p.Parse("output.txt");

            // Because ScGame is just a C# representation of a game event log, we need to convert it to a more appropriate format in order to do data analysis.
            // In this instance, the VectorProcessor class is used. It converts the game event log of a ScGame game, into a list of game state vectors.
            // A game state vector will contain all events that has happend within a timeperiod. If timeGranularity is set to 30, and timeSlice is set to 4
            // This will produce 4 game state vectors, each with a length of 30 seconds
            VectorProcessor vp = new VectorProcessor();
            foreach (ScGame game in games)
            {
                vp.GenerateGameStateVectors(game, 30, 4);
            }

            Console.ReadLine();
        }
    }
}

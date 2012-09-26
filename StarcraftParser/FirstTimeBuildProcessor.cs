using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace StarcraftParser
{
    class UnitTimeVector
    {
        public Dictionary<string, int> Vector { get; set; }
        public Race Race { get; set; }

        public UnitTimeVector()
        {
            Vector = new Dictionary<string, int>();
        }
    }

    class FirstTimeBuildProcessor : Processor
    {
        private string SEPERATION_SYMBOL = ";";
        private string CULTURE = "en-US";

        public UnitTimeVector ProcessGame(ScGame game)
        {
            UnitTimeVector v = new UnitTimeVector();
            v.Race = game.Race;

            IniVector(v.Vector, game.Race);

            foreach (ScEvent scEvent in game.Events)
            {
                if (v.Vector[scEvent.Unit] > scEvent.Time)
                    v.Vector[scEvent.Unit] = scEvent.Time;
            }

            return v;
        }

        private void IniVector(Dictionary<string, int> vector, Race race)
        {
            if (race == Race.Terran)
            {
                foreach (string unit in TerrainUnits)
                {
                    vector.Add(unit, int.MaxValue);
                }
            }
            else if (race == Race.Protoss)
            {
                foreach (string unit in ProtossUnits)
                {
                    vector.Add(unit, int.MaxValue);
                }
            }
            else if (race == Race.Zerg)
            {
                foreach (string unit in ZergUnits)
                {
                    vector.Add(unit, int.MaxValue);
                }
            }
        }

        public void WriteGamesToCsv(List<UnitTimeVector> games, string file, CsvType csvType)
        {
            if (csvType == CsvType.Excel)
            {
                CULTURE = "da-DK";
                SEPERATION_SYMBOL = ";";
            }
            else if (csvType == CsvType.Weka)
            {
                CULTURE = "en-US";
                SEPERATION_SYMBOL = ",";
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(file, FileMode.Create)))
            {
                // Writes the CSV Header
                sw.Write("id" + SEPERATION_SYMBOL);

                int counter = 0;
                foreach (KeyValuePair<string, int> unit in games[0].Vector)
                {
                    sw.Write(unit.Key);
                    if (counter != games[0].Vector.Count - 1)
                        sw.Write(SEPERATION_SYMBOL);
                    counter++;
                }
                sw.WriteLine("");

                // Writes the CSV body
                for (int i = 0; i < games.Count; i++)
                {
                    WriteGameToCsv(games[i], i, sw);
                }
            }
        }

        private void WriteGameToCsv(UnitTimeVector game, int gameId, StreamWriter sw)
        {
            sw.Write(gameId + SEPERATION_SYMBOL);

            int counter = 0;
            foreach (KeyValuePair<string, int> unit in game.Vector)
            {
                if (unit.Value == int.MaxValue)
                    sw.Write(-1);
                else
                    sw.Write(unit.Value);
                if (counter != game.Vector.Count - 1)
                    sw.Write(SEPERATION_SYMBOL);
                counter++;
            }

            sw.WriteLine("");
        }
    }
}

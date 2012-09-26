using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace StarcraftParser
{
    class TimeSliceProcessor : Processor
    {
        private string SEPERATION_SYMBOL = ";";
        private string CULTURE = "en-US";

        public TimeSliceGame ProcessGame(ScGame game, int timeGranularity, int timeSlices)
        {
            List<GameStateVector> gameStateVectors = new List<GameStateVector>();

            for (int i = 0; i < timeSlices; i++)
            {
                List<ScEvent> events = game.Events.Where(e => e.Time < timeGranularity * (i + 1)).ToList();

                Dictionary<string, float> unitCounter = new Dictionary<string, float>();
                IniUnitCounter(unitCounter, game.Race);

                foreach (ScEvent scEvent in events)
                {
                    if (unitCounter.ContainsKey(scEvent.Unit))
                    {
                        unitCounter[scEvent.Unit]++;
                    }
                    else
                    {
                        unitCounter.Add(scEvent.Unit, 1);
                    }
                }

                GameStateVector gsv = new GameStateVector() { UnitCounter = unitCounter };
                gameStateVectors.Add(gsv);
            }

            return new TimeSliceGame() { GameStateVectors = gameStateVectors, Race = game.Race };
        }

        /// <summary>
        /// Fills the unitCounter with unit entries
        /// </summary>
        /// <param name="unitCounter"></param>
        /// <param name="race"></param>
        private void IniUnitCounter(Dictionary<string, float> unitCounter, Race race)
        {
            if (race == Race.Terran)
            {
                foreach (string unit in TerrainUnits)
                {
                    unitCounter.Add(unit, 0);
                }
            }
            else if (race == Race.Protoss)
            {
                foreach (string unit in ProtossUnits)
                {
                    unitCounter.Add(unit, 0);
                }
            }
            else if (race == Race.Zerg)
            {
                foreach (string unit in ZergUnits)
                {
                    unitCounter.Add(unit, 0);
                }
            }
        }
       
        /// <summary>
        /// Assumes that all games are of the same sc race
        /// </summary>
        /// <param name="games"></param>
        /// <param name="file"></param>
        public void WriteGamesToCsv(List<TimeSliceGame> games, string file, bool onlyWriteLastVector, CsvType csvType)
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
                sw.Write("id" + SEPERATION_SYMBOL + "time_slice" + SEPERATION_SYMBOL);

                int counter = 0;
                foreach (KeyValuePair<string, float> unit in games[0].GameStateVectors[0].UnitCounter)
                {
                    sw.Write(unit.Key);
                    if (counter != games[0].GameStateVectors[0].UnitCounter.Count - 1)
                        sw.Write(SEPERATION_SYMBOL);
                    counter++;
                }
                sw.WriteLine("");

                // Writes the CSV body
                for (int i = 0; i < games.Count; i++)
                {
                    WriteGameToCsv(games[i], i, sw, onlyWriteLastVector);
                }
            }
        }

        private void WriteGameToCsv(TimeSliceGame game, int gameId, StreamWriter sw, bool onlyWriteLastVector)
        {
            for (int i = 0; i < game.GameStateVectors.Count; i++)
            {
                if (onlyWriteLastVector)
                    if (i != game.GameStateVectors.Count - 1)
                        continue;
                sw.Write(gameId + SEPERATION_SYMBOL + i + SEPERATION_SYMBOL);

                int counter = 0;
                foreach (KeyValuePair<string, float> unit in game.GameStateVectors[i].UnitCounter)
                {
                    sw.Write(unit.Value.ToString("F8", CultureInfo.CreateSpecificCulture(CULTURE)));
                    if (counter != game.GameStateVectors[i].UnitCounter.Count - 1)
                        sw.Write(SEPERATION_SYMBOL);
                    counter++;
                }

                sw.WriteLine("");
            }
        }

        public List<TimeSliceGame> Normalize(List<TimeSliceGame> games)
        {
            Dictionary<string, float> maxUnitCounts = new Dictionary<string, float>();

            foreach (TimeSliceGame game in games)
            {
                foreach (GameStateVector vector in game.GameStateVectors)
                {
                    foreach (KeyValuePair<string, float> unit in vector.UnitCounter)
                    {
                        if (maxUnitCounts.ContainsKey(unit.Key) == false)
                        {
                            maxUnitCounts.Add(unit.Key, unit.Value);
                        }
                        else if (maxUnitCounts[unit.Key] < unit.Value)
                        {
                            maxUnitCounts.Remove(unit.Key);
                            maxUnitCounts.Add(unit.Key, unit.Value);
                        }
                    }
                }
            }

            List<TimeSliceGame> newGames = new List<TimeSliceGame>();
            foreach (TimeSliceGame game in games)
            {
                TimeSliceGame newGame = new TimeSliceGame();
                newGame.Race = game.Race;
                foreach (GameStateVector vector in game.GameStateVectors)
                {
                    Dictionary<string, float> newUnitCounter = new Dictionary<string, float>();
                    foreach (KeyValuePair<string, float> unit in vector.UnitCounter)
                    {
                        float newValue = unit.Value / maxUnitCounts[unit.Key];
                        newUnitCounter.Add(unit.Key, newValue);
                    }
                    GameStateVector gsv = new GameStateVector();
                    gsv.UnitCounter = newUnitCounter;

                    newGame.GameStateVectors.Add(gsv);
                }
                newGames.Add(newGame);
            }

            return newGames;
        }

    }
}

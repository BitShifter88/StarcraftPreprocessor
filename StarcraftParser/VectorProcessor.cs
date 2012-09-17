using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StarcraftParser
{
    class VectorProcessor
    {
        private const string SEPERATION_SYMBOL = ",";
        private List<string> _terrainUnits;
        private List<string> _zergUnits;
        private List<string> _protossUnits;

        public ProcessedGame GenerateGameStateVectors(ScGame game, int timeGranularity, int timeSlices)
        {
            List<GameStateVector> gameStateVectors = new List<GameStateVector>();

            for (int i = 0; i < timeSlices; i++)
            {
                List<ScEvent> events = game.Events.Where(e => e.Time < timeGranularity * (i + 1)).ToList();

                Dictionary<string, int> unitCounter = new Dictionary<string, int>();
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

            return new ProcessedGame() { GameStateVectors = gameStateVectors, Race = game.Race };
        }

        /// <summary>
        /// Fills the unitCounter with unit entries
        /// </summary>
        /// <param name="unitCounter"></param>
        /// <param name="race"></param>
        private void IniUnitCounter(Dictionary<string, int> unitCounter, Race race)
        {
            if (race == Race.Terran)
            {
                foreach (string unit in _terrainUnits)
                {
                    unitCounter.Add(unit, 0);
                }
            }
            else if (race == Race.Protoss)
            {
                foreach (string unit in _protossUnits)
                {
                    unitCounter.Add(unit, 0);
                }
            }
            else if (race == Race.Zerg)
            {
                foreach (string unit in _zergUnits)
                {
                    unitCounter.Add(unit, 0);
                }
            }
        }

        /// <summary>
        /// Builds a list of all known units, based on the units that appears in all game logs
        /// </summary>
        /// <param name="games"></param>
        public void BuildUnitList(List<ScGame> games)
        {
            _terrainUnits = new List<string>();
            _zergUnits = new List<string>();
            _protossUnits = new List<string>();

            foreach (ScGame game in games)
            {
                foreach (ScEvent scEvent in game.Events)
                {
                    if (game.Race == Race.Terran)
                    {
                        if (_terrainUnits.Contains(scEvent.Unit) == false)
                        {
                            _terrainUnits.Add(scEvent.Unit);
                        }
                    }
                    else if (game.Race == Race.Protoss)
                    {
                        if (_protossUnits.Contains(scEvent.Unit) == false)
                        {
                            _protossUnits.Add(scEvent.Unit);
                        }
                    }
                    else if (game.Race == Race.Zerg)
                    {
                        if (_zergUnits.Contains(scEvent.Unit) == false)
                        {
                            _zergUnits.Add(scEvent.Unit);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Assumes that all games are of the same sc race
        /// </summary>
        /// <param name="games"></param>
        /// <param name="file"></param>
        public void WriteGamesToCsv(List<ProcessedGame> games, string file)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(file, FileMode.Create)))
            {
                // Writes the CSV Header
                sw.Write("id" + SEPERATION_SYMBOL + "vector" + SEPERATION_SYMBOL);

                int counter = 0;
                foreach (KeyValuePair<string, int> unit in games[0].GameStateVectors[0].UnitCounter)
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
                    WriteGameToCsv(games[i], i, sw);
                }
            }
        }

        private void WriteGameToCsv(ProcessedGame game, int gameId, StreamWriter sw)
        {
            for (int i = 0; i < game.GameStateVectors.Count; i++)
            {
                sw.Write(gameId + SEPERATION_SYMBOL + i + SEPERATION_SYMBOL);

                int counter = 0;
                foreach (KeyValuePair<string, int> unit in game.GameStateVectors[i].UnitCounter)
                {
                    sw.Write(unit.Value);
                    if (counter != game.GameStateVectors[i].UnitCounter.Count - 1)
                        sw.Write(SEPERATION_SYMBOL);
                    counter++;
                }

                sw.WriteLine("");
            }
        }

    }
}

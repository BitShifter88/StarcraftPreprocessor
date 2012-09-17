using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    class GameStateVector
    {
        public Dictionary<string, int> UnitCounter { get; set; }

        public GameStateVector()
        {
            UnitCounter = new Dictionary<string, int>();
        }
    }

    class VectorProcessor
    {
        private List<string> _terrainUnits;
        private List<string> _zergUnits;
        private List<string> _protossUnits;


        public List<GameStateVector> GenerateGameStateVectors(ScGame game, int timeGranularity, int timeSlices)
        {
            List<GameStateVector> gameStateVectors = new List<GameStateVector>();

            for (int i = 0; i < timeSlices; i++)
            {
                List<ScEvent> events = game.Events.Where(e => e.Time < timeGranularity * (i + 1)).ToList();

                Dictionary<string, int> unitCounter = new Dictionary<string,int>();
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

            return gameStateVectors;
        }

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
    }
}

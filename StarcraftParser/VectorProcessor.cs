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
        public List<GameStateVector> GenerateGameStateVectors(ScGame game, int timeGranularity, int timeSlices)
        {
            List<GameStateVector> gameStateVectors = new List<GameStateVector>();

            for (int i = 0; i < timeSlices; i++)
            {
                List<ScEvent> events = game.Events.Where(e => e.Time < timeGranularity * (i + 1)).ToList();

                Dictionary<string, int> unitCounter = new Dictionary<string,int>();
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
    }
}

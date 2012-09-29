using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    abstract class Processor
    {
        public List<string> TerranUnits { get; set; }
        public List<string> ZergUnits { get; set; }
        public List<string> ProtossUnits { get; set; }

        /// <summary>
        /// Builds a list of all known units, based on the units that appears in all game logs
        /// </summary>
        /// <param name="games"></param>
        public void BuildUnitList(List<ScGame> games)
        {
            TerranUnits = new List<string>();
            ZergUnits = new List<string>();
            ProtossUnits = new List<string>();

            foreach (ScGame game in games)
            {
                foreach (ScEvent scEvent in game.Events)
                {
                    if (game.Race == Race.Terran)
                    {
                        if (TerranUnits.Contains(scEvent.Unit) == false)
                        {
                            TerranUnits.Add(scEvent.Unit);
                        }
                    }
                    else if (game.Race == Race.Protoss)
                    {
                        if (ProtossUnits.Contains(scEvent.Unit) == false)
                        {
                            ProtossUnits.Add(scEvent.Unit);
                        }
                    }
                    else if (game.Race == Race.Zerg)
                    {
                        if (ZergUnits.Contains(scEvent.Unit) == false)
                        {
                            ZergUnits.Add(scEvent.Unit);
                        }
                    }
                }
            }
        }
    }
}

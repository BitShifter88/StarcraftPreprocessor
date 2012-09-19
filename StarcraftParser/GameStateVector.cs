using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    class GameStateVector
    {
        public Dictionary<string, float> UnitCounter { get; set; }

        public GameStateVector()
        {
            UnitCounter = new Dictionary<string, float>();
        }
    }
}

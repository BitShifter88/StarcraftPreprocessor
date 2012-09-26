using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    class TimeSliceGame
    {
        public List<GameStateVector> GameStateVectors { get; set; }
        public Race Race { get; set; }

        public TimeSliceGame()
        {
            GameStateVectors = new List<GameStateVector>();
        }
    }
}

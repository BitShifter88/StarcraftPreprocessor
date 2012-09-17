using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    class ProcessedGame
    {
        public List<GameStateVector> GameStateVectors { get; set; }
        public Race Race { get; set; }

        public ProcessedGame()
        {
            GameStateVectors = new List<GameStateVector>();
        }
    }
}

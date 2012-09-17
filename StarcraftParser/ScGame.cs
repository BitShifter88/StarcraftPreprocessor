using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    class ScGame
    {
        public List<ScEvent> Events { get; set; }
        public Race Race { get; set; }

        public ScGame()
        {
            Events = new List<ScEvent>();
        }
    }
}

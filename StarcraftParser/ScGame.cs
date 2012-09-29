using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

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

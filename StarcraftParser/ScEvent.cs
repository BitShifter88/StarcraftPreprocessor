﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    class ScEvent
    {
        public int Time { get; set; }
        public Race Race { get; set; }
        public int Score { get; set; }
        public string Unit { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Minerals { get; set; }
        public int Gas { get; set; }
        public int WorkerCount { get; set; }
    }
}
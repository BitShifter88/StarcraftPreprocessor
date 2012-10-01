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

        public override bool Equals(object obj)
        {
            ScEvent e = (ScEvent)obj;

            if (Race == e.Race &&
                Score == e.Score &&
                Unit == e.Unit &&
                X == e.X &&
                Y == e.Y &&
                Minerals == e.Minerals &&
                Gas == e.Gas &&
                WorkerCount == e.WorkerCount &&
                Time == e.Time)
                return true;
            else
                return false;
        }
    }
}

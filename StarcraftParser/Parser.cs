using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StarcraftParser
{
    enum Race
    {
        Protoss,
        Terran,
        Zerg,
    }

    enum Unit
    {
        TerranSCV,
        TerranSupplyDepot,
        TerranBarracks,
        TerranFactory,
        TerranMachineShop,
        TerranMarine,
        TerranGateway,
        TerranSiegeTank,
        TerranVulture,
        TerranVultureSpiderMine, // Måske skal denne ikke med
        TerranAcademy,
        TerranBunker,
        TerranEngineeringBay,
        TerranMissileTurret,
        TerranMedic,
        TerranControlTower,
        TerranScienceFacility,
        TerranRefinery,
        TerranSiegeTankTankMode, // Måske skal denne ikke med
        TerranCommandCenter,
        TerranComsatStation,
        SpellScannerSweep,
    }

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

    class ScGame
    {
        public List<ScEvent> Events { get; set; }
        public Race Race { get; set; }

        public ScGame()
        {
            Events = new List<ScEvent>();
        }
    }

    class Parser
    {
        public List<ScGame> Parse(string file)
        {
            string[] raw = File.ReadAllLines(file);
            Dictionary<long, ScGame> games = new Dictionary<long, ScGame>();

            Dictionary<string, int> terranUnits = new Dictionary<string, int>();

            foreach (string str in raw)
            {
                string line = str.Replace('\"', ' ');
                string[] split = line.Split(',');

                // Rå strings fra dette event, splittet op i de forskellige elementer
                string idStr = split[0];
                string timeStr = split[1];
                string playerStr = split[2];
                string raceStr = split[3];
                string scoreStr = split[4];
                string unitStr = split[5];
                string xStr = split[6];
                string yStr = split[7];
                string mineralsStr = split[8];
                string gasStr = split[9];
                string workerCountStr = split[10];

                // De forskellige ting et event består af.
                long id;
                int time;
                Race race = Race.Protoss;
                int score;
                string unit;
                int x;
                int y;
                int minerals;
                int gas;
                int workerCount;

                id = long.Parse(idStr) * 2 + long.Parse(playerStr);

                switch (raceStr)
                {
                    case " Protoss ":
                        race = Race.Protoss;
                        break;
                    case " Terran ":
                        race = Race.Terran;
                        break;
                    case " Zerg ":
                        race = Race.Zerg;
                        break;
                    default:
                        throw new Exception("Yo, den race findes ikke");
                }

                time = int.Parse(timeStr);
                score = int.Parse(scoreStr);
                unit = unitStr.Trim();
                x = int.Parse(xStr);
                y = int.Parse(yStr);
                minerals = int.Parse(mineralsStr);
                gas = int.Parse(gasStr);
                workerCount = int.Parse(workerCountStr);

                ScEvent ev = new ScEvent() { Gas = gas, Minerals = minerals, Race = race, Score = score, Time = time, Unit = unit, WorkerCount = workerCount, X = x, Y = y };

                if (games.ContainsKey(id) == true)
                {
                    games[id].Events.Add(ev);
                }
                else
                {
                    games.Add(id, new ScGame() { Race = race });
                    games[id].Events.Add(ev);
                }

                if (race == Race.Terran)
                {
                    if (terranUnits.ContainsKey(unit) == false)
                        terranUnits.Add(unit, 0);
                }
            }

            //            1347392075,0,1,"Protoss",200,"Protoss Probe",3808,3856,50,0,5
            //1347392075,0,0,"Terran",200,"Terran SCV",288,3856,50,0,5
            //1347392075,15,1,"Protoss",250,"Protoss Probe",3808,3856,106,0,6
            //1347392075,18,0,"Terran",250,"Terran SCV",288,3856,106,0,6

            List<ScGame> gamesList = new List<ScGame>();

            foreach (KeyValuePair<long, ScGame> game in games)
            {
                gamesList.Add(game.Value);
            }

            return gamesList;
        }

        private Unit GetUnit(string unit)
        {
                switch (unit)
                {
                    case " Terran SCV ":
                        return Unit.TerranSCV;
                        break;
                    case " Terran Supply Depot ":
                        return Unit.TerranSupplyDepot;
                        break;
                    case " Terran Barracks ":
                        return Unit.TerranBarracks;
                        break;
                    case " Terran Factory ":
                        return Unit.TerranFactory;
                        break;
                    case " Terran Machine Shop ":
                        return Unit.TerranMachineShop;
                        break;
                    case " Terran Marine ":
                        return Unit.TerranMarine;
                        break;
                    case " Terran Gateway ":
                        return Unit.TerranGateway;
                        break;
                    case " Terran Siege Tank ":
                        return Unit.TerranSiegeTank;
                        break;
                    case " Terran Vulture ":
                        return Unit.TerranVulture;
                        break;
                    case " Terran Vulture Spider Mine ":
                        return Unit.TerranVultureSpiderMine;
                        break;
                    case " Terran Academy ":
                        return Unit.TerranAcademy;
                        break;
                    case " Terran Bunker ":
                        return Unit.TerranBunker;
                        break;
                    case " Terran Engineering Bay ":
                        return Unit.TerranEngineeringBay;
                        break;
                    case " Terran Missile Turret ":
                        return Unit.TerranMissileTurret;
                        break;
                    case " Terran Medic ":
                        return Unit.TerranMedic;
                        break;
                    case " Terran Control Tower ":
                        return Unit.TerranControlTower;
                        break;
                    case " Terran Science Facility ":
                        return Unit.TerranScienceFacility;
                        break;
                    case " Terran Refinery ":
                        return Unit.TerranRefinery;
                        break;
                    case " Terran Siege Tank Tank Mode ":
                        return Unit.TerranSiegeTankTankMode;
                        break;
                    case " Terran Command Center ":
                        return Unit.TerranCommandCenter;
                        break;
                    case " Terran Comsat Station ":
                        return Unit.TerranComsatStation;
                        break;
                    case " Spell Scanner Sweep ":
                        return Unit.SpellScannerSweep;
                        break;
                    default:
                        throw new Exception(unit);
                }
        }
    }
}

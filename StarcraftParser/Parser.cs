using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StarcraftParser
{
    enum Race : int
    {
        Protoss = 0,
        Terran = 1,
        Zerg = 2,
    }

    class Parser
    {
        public List<ScGame> Parse(string file)
        {
            string[] raw = File.ReadAllLines(file);
            Dictionary<string, ScGame> games = new Dictionary<string, ScGame>();

            Dictionary<string, int> terranUnits = new Dictionary<string, int>();

            foreach (string str in raw)
            {
                string line = str.Replace('\"', ' ');
                string[] split = line.Split(',');

                // Rå strings fra dette event, splittet op i de forskellige elementer
                string filenameStr = split[0];
                //string mapnameStr = split[1];
                string timeStr = split[1];
                string playeridStr = split[2];
                //string playerStr = split[3];
                string raceStr = split[3];
                string scoreStr = split[4];
                string unitStr = split[5];
                string xStr = split[6];
                string yStr = split[7];
                string gasStr = split[8];
                string mineralsStr = split[9];
                string workerCountStr = split[10];
			
                // De forskellige ting et event består af.
                string filename = filenameStr;
                //string mapname = mapnameStr;
                int time = int.Parse(timeStr);
                int playerid = int.Parse(playeridStr);
                //string player = playerStr;
                Race race;
                int score = int.Parse(scoreStr);
                string unit = unitStr;
                int x = int.Parse(xStr);
                int y = int.Parse(yStr);
                int gas = int.Parse(gasStr);
                int minerals = int.Parse(mineralsStr);
                int workerCount = int.Parse(workerCountStr);

                //id = long.Parse(idStr) * 2 + long.Parse(playerStr);

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
                        throw new Exception(raceStr + "is not a race in Starcraft");
                }

                ScEvent ev = new ScEvent() {
                    Filename = filename, 
                    //Mapname = mapname,
                    Time = time, 
                    Playerid = playerid, 
                    //Player = player, 
                    Race = race, 
                    Score = score, 
                    Unit = unit,
                    X = x, 
                    Y = y,
                    Gas = gas,
                    Minerals = minerals,
                    WorkerCount = workerCount
                };

                string id = filename + playerid;

                if (games.ContainsKey(id) == true)
                {
                    games[id].Events.Add(ev);
                }
                else
                {
                    games.Add(id, new ScGame() { Race = race });
                    games[id].Events.Add(ev);
                }
            }

            List<ScGame> gamesList = new List<ScGame>();

            foreach (KeyValuePair<string, ScGame> game in games)
            {
                gamesList.Add(game.Value);
            }

            return gamesList;
        }
    }
}

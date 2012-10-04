using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WekaClusterConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args[0];

            string[] lines = File.ReadAllLines(file);

            Dictionary<int, List<string>> sorted = new Dictionary<int, List<string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] split = lines[i].Split(',');
                string clusterStr = split[split.Length - 1].Remove(0, 7);
                int cluster = int.Parse(clusterStr);

                if (sorted.ContainsKey(cluster) == false)
                {
                    sorted.Add(cluster, new List<string>());
                }
                sorted[cluster].Add(lines[i]);

            }

            using (StreamWriter sw = new StreamWriter(new FileStream("output.csv", FileMode.Create)))
            {
                sw.WriteLine(lines[0]);

                foreach (KeyValuePair<int, List<string>> line in sorted)
                {
                    foreach (string str in line.Value)
                    {
                        sw.WriteLine(str);
                    }
                }
            }

        }
    }
}

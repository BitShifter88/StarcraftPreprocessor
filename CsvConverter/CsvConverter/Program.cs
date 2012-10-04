using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CsvConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = args[0];

            string[] lines = File.ReadAllLines(file);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace(',', ';');
            }

            File.WriteAllLines("output.csv", lines);
        }
    }
}

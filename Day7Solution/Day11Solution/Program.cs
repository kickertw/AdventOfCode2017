using System;
using System.IO;

namespace Day11Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = new StreamReader((@".\Input\Day11Input.txt"));

            string line;
            if ((line = file.ReadLine()) == null)
            {
                Console.WriteLine("Bad reading!");
                Environment.Exit(0);
            }

            file.Close();

            string[] steps = line.Split(',');
            int x = 0, y = 0, max = 0;
            for (int i = 0; i < steps.Length; i++)
            {
                UpdateXYCount(steps[i], ref x, ref y);
                int temp = HexDistance(x, y);
                if (temp > max) max = temp;
            }

            Console.WriteLine("pt1: " + HexDistance(x, y));
            Console.WriteLine("pt2: " + max);
            Console.ReadLine();
        }

        static void UpdateXYCount(string step, ref int x, ref int y)
        {

            if (step == "n") { y += 1; }
            else if (step == "s") { y += -1; }
            else if (step == "ne") { x += 1; }
            else if (step == "nw") { x += -1; y += 1; }
            else if (step == "se") { x += 1; y += -1; }
            else if (step == "sw") { x += -1; }
        }

        // https://www.redblobgames.com/grids/hexagons/#distances
        static int HexDistance(int x, int y)
        {
            return (Math.Abs(x) + Math.Abs(y) + Math.Abs(x + y)) / 2;
        }
    }
}
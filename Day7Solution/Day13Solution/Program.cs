using System;
using System.IO;
using System.Linq;

namespace Day13Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var totalPenaltyCost = 1;
            var delay = 0;
            var input = File.ReadAllLines(@".\Input\Input.txt");

            while (totalPenaltyCost > 0)
            {
                var cursorIndex = 0;
                totalPenaltyCost = 0;

                foreach (var line in input)
                {
                    var depthRange = line.Split(": ").Select(i => int.Parse(i)).ToArray();
                    while (cursorIndex <= depthRange[0])
                    {
                        if (IsCaught(cursorIndex, depthRange[0], depthRange[1], delay))
                        {
                            Console.WriteLine($"We got caught! Position {cursorIndex}");
                            totalPenaltyCost += cursorIndex * depthRange[1];
                        }

                        cursorIndex++;
                    }
                }

                // Part 1
                if (delay == 0) Console.WriteLine($"Total Penalty = {totalPenaltyCost}");

                // Part 2
                if (totalPenaltyCost == 0) Console.WriteLine($"Delay so no penalty = {delay}");
                delay++;
            }
            
            Console.ReadLine();
        }

        private static bool IsCaught(int cursorIndex, int depth, int range, int delay = 0)
        {
            if (cursorIndex < depth)
            {
                return false;
            }
            else
            {
                // determine scanner location.  If the scanner ends at position 1, it was at position 0 which is the only way
                // we get caught
                var reverse = false;
                var currentScannerIndex = 0;
                for (var ii = 0; ii <= cursorIndex + delay; ii++)
                {
                    //Console.WriteLine($"{ii}) Scanner start = {currentScannerIndex} going {(!reverse ? "forward" : "backward")}");

                    // change direction
                    if (currentScannerIndex == range - 1)
                    {
                        reverse = true;
                    }
                    else if (currentScannerIndex == 0)
                    {
                        reverse = false;
                    }

                    //increment
                    if (reverse) { currentScannerIndex--; }
                    else { currentScannerIndex++; }

                    //Console.WriteLine($"{ii}) Scanner end = {currentScannerIndex} going {(!reverse ? "forward" : "backward")}");
                }

                //Console.WriteLine($"Scanner Index ended up at {currentScannerIndex} going {(!reverse ? "forward" : "backward")}");
                //if (currentScannerIndex == 1 && !reverse) Console.WriteLine($"Scanner {cursorIndex} caught you!");
                //else Console.WriteLine($"Scanner {cursorIndex} ended on {currentScannerIndex}!");
                return currentScannerIndex == 1 && !reverse;
            }
        }
    }
}

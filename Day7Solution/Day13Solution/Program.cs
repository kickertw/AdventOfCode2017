using System;
using System.IO;
using System.Linq;

namespace Day13Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var cursorIndex = 0;
            var totalPenaltyCost = 0;
            var input = File.ReadAllLines(@".\Input\Input.txt");

            foreach (var line in input)
            {
                var depthRange = line.Split(": ").Select(i => int.Parse(i)).ToArray();
                while (cursorIndex <= depthRange[0])
                {
                    if (IsCaught(cursorIndex, depthRange[0], depthRange[1]))
                    {
                        Console.WriteLine($"We got caught! Position {cursorIndex}");
                        totalPenaltyCost += cursorIndex * depthRange[1];
                    }
                    
                    cursorIndex++;
                }
            }

            Console.WriteLine($"Total Penalty = {totalPenaltyCost}");
            Console.ReadLine();
        }

        private static bool IsCaught(int cursorIndex, int depth, int range)
        {
            if (cursorIndex < depth)
            {
                Console.WriteLine($"CursorPosition = {cursorIndex} has no scanner");
                return false;
            }
            else
            {
                // determine scanner location.  If the scanner ends at position 1, it was at position 0 which is the only way
                // we get caught
                var reverse = false;
                var currentScannerIndex = 0;
                for (var ii = 0; ii <= cursorIndex; ii++)
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

                return currentScannerIndex == 1 && !reverse;
            }
        }
    }
}

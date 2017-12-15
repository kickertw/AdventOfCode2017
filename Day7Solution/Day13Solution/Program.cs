using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13Solution
{
    class Program
    {
        static int delay = 0;
        // key = ending scanner location, value = direction (true = reverse, false = forward)
        static Dictionary<int, bool>[] firewall = new Dictionary<int, bool>[96];

        static void Main(string[] args)
        {
            var totalPenaltyCost = 1;
            var input = File.ReadAllLines(@".\Input\Input.txt");
            var isCaught = true;

            while (isCaught)
            {
                isCaught = false;
                var cursorIndex = 0;
                totalPenaltyCost = 0;

                //Console.WriteLine($"delay = {delay}");
                foreach (var line in input)
                {
                    var depthRange = line.Split(": ").Select(i => int.Parse(i)).ToArray();
                    while (cursorIndex <= depthRange[0])
                    {
                        var caughtTemp = IsCaught(cursorIndex, depthRange[0], depthRange[1], delay);
                        if (caughtTemp && !isCaught) isCaught = true;
                        if (caughtTemp)
                        {
                            //Console.WriteLine($"We got caught! Position {cursorIndex}");
                            totalPenaltyCost += cursorIndex * depthRange[1];
                        }

                        cursorIndex++;
                    }
                }
                //Console.WriteLine($"Delay {delay} / StillGotCaught = {isCaught}");

                // Part 1
                if (delay == 0) Console.WriteLine($"Total Penalty = {totalPenaltyCost}");

                // Part 2
                if (totalPenaltyCost == 0 && isCaught) Console.WriteLine($"Delay so no penalty = {delay}");
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

                if (delay == 0)
                {
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

                    firewall[cursorIndex].Add(cursorIndex, reverse);
                }
                else
                {
                    for (var jj = 0; jj < firewall.Length; jj++){
                        if (firewall[jj].Any())
                        {
                            var lastPosition = firewall[jj].First();
                            firewall[jj].Clear();

                            // change direction
                            if (lastPosition.Key == range - 1)
                            {
                                reverse = true;
                            }
                            else if (lastPosition.Key == 0)
                            {
                                reverse = false;
                            }

                            //increment
                            if (reverse) { firewall[jj].Add(lastPosition.Key - 1, reverse); }
                            else { firewall[jj].Add(lastPosition.Key - 1, reverse); }
                        }
                    }
                }

                //Console.WriteLine($"Scanner Index ended up at {currentScannerIndex} going {(!reverse ? "forward" : "backward")}");
                //if (currentScannerIndex == 1 && !reverse) Console.WriteLine($"Scanner {cursorIndex} caught you!");
                //else Console.WriteLine($"Scanner {cursorIndex} ended on {currentScannerIndex}!");
                return currentScannerIndex == 1 && !reverse;
            }
        }
    }
}

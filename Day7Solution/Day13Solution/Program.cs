using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13Solution
{
    /// <summary>
    /// Solves: http://adventofcode.com/2017/day/13
    /// </summary>
    public class Day13
    {
        public static void Main(string[] args)
        {
            var test = new Day13();
            test.Run();
        }

        public void Run()
        {
            Dictionary<int, Scanner> scanners = LoadScannersFromInput();

            int severity = RunFirewall(scanners);
            Console.WriteLine("Part 1 solution: " + severity);

            int delay = FindMinimumCleanFirewallRunDelay(scanners);
            Console.WriteLine("Part 2 solution: " + delay);
            Console.ReadLine();
        }

        private Dictionary<int, Scanner> LoadScannersFromInput()
        {
            Dictionary<int, Scanner> scanners = new Dictionary<int, Scanner>();

            string[] lines = File.ReadAllLines(".\\Input\\Input.txt");
            foreach (string line in lines)
            {
                string[] tokens = line.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int depth = Int32.Parse(tokens[0]);
                int range = Int32.Parse(tokens[1]);
                Scanner scanner = new Scanner(range);
                scanners[depth] = scanner;
            }
            return scanners;
        }

        private int RunFirewall(Dictionary<int, Scanner> scanners)
        {
            int severity = 0;

            int maxDepth = scanners.Keys.Max();
            for (int depth = 0; depth <= maxDepth; depth++)
            {
                // Are we caught at this depth?
                if (scanners.ContainsKey(depth) && scanners[depth].Position == 0)
                {
                    severity += (depth * scanners[depth].Range);
                }

                AdvanceScannerPositions(scanners);
            }

            return severity;
        }

        private void AdvanceScannerPositions(Dictionary<int, Scanner> scanners)
        {
            foreach (Scanner scanner in scanners.Values)
            {
                scanner.AdvancePosition();
            }
        }

        /// <summary>
        /// Instead of actually running a simulation as we did in Part 1, this 
        /// Part 2 solution just directly "does the math" to determine whether,
        /// at a given time index, a given scanner's position is 0 or not.
        /// </summary>
        private int FindMinimumCleanFirewallRunDelay(Dictionary<int, Scanner> scanners)
        {
            int delay = 0;
            while (true)
            {
                int maxDepth = scanners.Keys.Max();
                bool detected = false;
                for (int depth = 0; depth <= maxDepth; depth++)
                {
                    int elapsedTime = delay + depth;

                    if (scanners.ContainsKey(depth))
                    {
                        // The "frequency" is how often a scanner will be at 
                        // position 0, based on its range, as it moves back and
                        // forth along the range. For example:
                        // Range 2: Every 2 turns
                        // Range 3: Every 4 turns
                        // Range 4: Every 6 turns
                        // Range 5: Every 8 turns
                        // ... and so on.
                        int frequency = (scanners[depth].Range - 1) * 2;

                        if (elapsedTime % frequency == 0)
                        {
                            detected = true;
                            break;
                        }
                    }
                }
                if (!detected)
                {
                    return delay;
                }
                delay++;
            }
        }
    }

    class Scanner
    {
        private readonly int _range;

        public int Range { get { return _range; } }
        public int Position { get; set; }
        public bool Direction { get; set; } // Down == true, Up == false

        public Scanner(int range)
        {
            this._range = range;
            Position = 0;
            Direction = true;
        }

        public void AdvancePosition()
        {
            if (Direction == true) // Down
            {
                Position++;
                if (Position == Range)
                {
                    Direction = false;
                    Position -= 2;
                }
            }
            else // Up
            {
                Position--;
                if (Position < 0)
                {
                    Direction = true;
                    Position += 2;
                }
            }
        }
    }
}

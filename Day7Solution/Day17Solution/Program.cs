using System;
using System.Collections.Generic;

namespace Day17Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var steps = 312;
            var spinLock = new List<int>() { 0 };
            var currentIndex = 0;
            var iterations = 2017;

            StartSpinlock(steps, iterations, 1, ref currentIndex, ref spinLock);
            Console.WriteLine($"(Part 1) Value after current position = {spinLock[currentIndex+1]}");
            Console.WriteLine($"(Part 1) Value after 0 = {spinLock[1]}");
            var part2Ans = Part2(steps);
            Console.WriteLine($"(Part 2) Value after 0 = {part2Ans}");
            Console.ReadLine();
        }

        static int Part2(int steps, int total = 50000000)
        {
            var pos = 0;
            var retVal = 0;

            for (var ii = 0; ii < total; ii++)
            {
                pos = MoveCursorV2(steps, pos, ii);
                if (pos == 0)
                {
                    retVal = ii;
                }

                // After the cursor moves the right amount of steps, the new value would be added
                // and we should start the next round at the next position
                pos++;
            }

            return retVal;
        }

        static int MoveCursorV2(int steps, int pos, int iteration)
        {
            if (iteration == 0) return 0;
            return (pos + steps) % iteration;
        }

        static void StartSpinlock(int steps, int iterations, int startingVal, ref int currentIndex, ref List<int> spinLock)
        {
            for (var ii = startingVal; ii <= iterations; ii++)
            {
                for (var jj = 0; jj < steps; jj++)
                {
                    currentIndex = MoveCursor(spinLock, currentIndex);
                }                

                spinLock = InsertValue(spinLock, currentIndex, ii);
                currentIndex = MoveCursor(spinLock, currentIndex);

                // Console.WriteLine(string.Join(",", spinLock));
            }
        }

        static int MoveCursor(List<int> spinLock, int currentIndex)
        {
            currentIndex++;

            if (currentIndex == spinLock.Count)
            {
                currentIndex = 0;
            }

            return currentIndex;
        }

        static List<int> InsertValue(List<int> oldList, int currentIndex, int value)
        {
            if (currentIndex == oldList.Count - 1)
            {
                oldList.Add(value);
                return oldList;
            }

            var retVal = oldList.GetRange(0, currentIndex + 1);
            retVal.Add(value);
            retVal.AddRange(oldList.GetRange(currentIndex + 1, oldList.Count - 1 - currentIndex));

            return retVal;
        }
    }
}

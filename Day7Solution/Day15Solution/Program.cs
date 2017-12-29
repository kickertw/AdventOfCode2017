using System;

namespace Day15Solution
{
    public class Day15
    {
        static void Main(string[] args)
        {
            long genA = 277;
            long genB = 349;

            long genAFactor = 16807;
            long genBFactor = 48271;

            var totalScore = 0;

            for (var ii = 0; ii < 5000000; ii++)
            {
                genA = GetNextValue(genA, genAFactor, 4);
                var valA = Convert.ToString(genA, 2);

                genB = GetNextValue(genB, genBFactor, 8);
                var valB = Convert.ToString(genB, 2);

                if (valA.Length >= 16 && valB.Length >= 16 && valA.Substring(valA.Length - 16) == valB.Substring(valB.Length - 16))
                {
                    //Console.WriteLine($"{ii} - Match On [{valA.Substring(valA.Length - 16)}]");
                    totalScore++;
                }
            }

            Console.WriteLine(totalScore);
            Console.ReadLine();
        }

        static long GetNextValue(long startingVal, long factor, long checkVal)
        {
            var retVal = (startingVal * factor % 2147483647);

            if (retVal % checkVal == 0) { return retVal; }

            return GetNextValue(retVal, factor, checkVal);
        }
    }
}

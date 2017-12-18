using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            int finalSum = 0;
            var input = File.ReadAllLines(@".\input.txt");

            foreach(var line in input)
            {
                var numbers = line.Split(' ').Select(i => int.Parse(i)).ToList();

                finalSum += TheQuotient(numbers);
            }

            Console.WriteLine(finalSum);
            Console.ReadLine();
        }

        private static int TheQuotient(List<int> numbers)
        {
            for (int ii = 0; ii < numbers.Count; ii++)
            {
                var jj = ii + 1;
                while (jj < numbers.Count)
                {
                    if (numbers[ii] % numbers[jj] == 0 || numbers[jj] % numbers[ii] == 0)
                    {
                        if (numbers[ii] > numbers[jj])
                        {
                            return numbers[ii] / numbers[jj];
                        }
                        else
                        {
                            return numbers[jj] / numbers[ii];
                        }

                    }

                    jj++;
                }
            }

            // We should never get here
            return int.MinValue;
        }
    }
}
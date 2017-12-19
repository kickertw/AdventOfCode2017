using System;
using System.IO;
using System.Linq;

namespace Day5Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var ii = 0;
            var answer = 0;
            var input = File.ReadAllLines(@".\input.txt").Select(i => int.Parse(i)).ToArray();

            while (ii >= 0 && ii < input.Length)
            {
                var lastLocation = ii;

                // jump
                ii += input[ii];

                // increment the last location
                if (input[lastLocation] >= 3)
                {
                    input[lastLocation]--;
                }
                else
                {
                    input[lastLocation]++;
                }                

                answer++;
            }


            Console.WriteLine("The answer is " + answer);
            Console.ReadLine();
        }
    }
}

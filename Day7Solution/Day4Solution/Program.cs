using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace Day4Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(@".\input.txt");
            var validCounter = 0;
            foreach(var line in input)
            {
                if (IsValid(line))
                {
                    validCounter++;
                }
            }

            Console.WriteLine("Valid Lines = " + validCounter);
            Console.ReadLine();
        }

        static bool IsValid(string line)
        {
            var orderedPhrases = new HashSet<string>();
            var passphrases = line.Split(' ');

            foreach (var pwd in passphrases)
            {
                if (!orderedPhrases.Add(string.Concat(pwd.OrderBy(i => i))))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

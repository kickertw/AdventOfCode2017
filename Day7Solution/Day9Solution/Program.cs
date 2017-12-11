using System;
using System.IO;

namespace Day9Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            // Should only be one line of text to read in
            var input = File.ReadAllLines(@".\Input\Day9Input.txt");
            var inp = input[0].ToCharArray();
            var garbage = false;
            int score = 0, depth = 1, garbageCount = 0;

            for (var i = 0; i < inp.Count(); i++)
            {
                var c = inp[i];
                if (c != '<' && !garbage) { Console.Write(c); }

                if (c == '!') i++;
                else if (garbage && c != '>') garbageCount++;
                else if (c == '<') garbage = true;
                else if (c == '>') garbage = false;
                else if (c == '{') { Console.Write(c); score += depth++; }
                else if (c == '}') { Console.Write(c); depth--; }
            }

            //Console.WriteLine(realString + " has score of " + score);
            Console.WriteLine("Final Score = " + score);
            Console.WriteLine("Garbage Char Count = " + garbageCount);
            Console.ReadLine();
        }
    }
}
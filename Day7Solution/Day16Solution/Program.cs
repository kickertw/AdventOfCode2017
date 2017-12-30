using System;
using System.IO;
using System.Linq;

namespace Day16Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(".\\input.txt");
            char[] programs = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };
            var start = "";

            var orderAfterDance = new string[60];

            var lastLoopIndex = 0;
            for (int ii = 0; ii < 1000000000; ii++)
            {
                lastLoopIndex = ii;
                foreach (var move in lines[0].Split(','))
                {
                    if (move.StartsWith('s'))
                    {
                        programs = Spin(programs, int.Parse(move.Substring(1)));
                    }
                    else if (move.StartsWith('x'))
                    {
                        var positions = move.Substring(1).Split('/');
                        programs = Exchange(programs, int.Parse(positions[0]), int.Parse(positions[1]));
                    }
                    else
                    {
                        var progs = move.Substring(1).Split('/');
                        programs = Partner(programs, char.Parse(progs[0]), char.Parse(progs[1]));
                    }
                }

                var newOrder = string.Join("", programs);
                if (orderAfterDance.Any(i => i == newOrder))
                {
                    break;
                }

                orderAfterDance[ii] = string.Join("", programs);
                Console.WriteLine($"{ii}    - {orderAfterDance[ii]}");
            }

            Console.WriteLine("Number of iterations = " + lastLoopIndex);

            var position = 0;
            for(int jj = 0; jj < 1000000000; jj++)
            {
                if (position == orderAfterDance.Length) { position = 0; }
                position++;
            }

            Console.WriteLine(orderAfterDance[position-1]);
            Console.ReadLine();
        }

        static char[] Spin(char[] programs, int lengthToGrab)
        {
            var copy = (char[])programs.Clone();
            var jj = 0;

            // put the end at the beginning
            for (var ii = programs.Length - lengthToGrab; ii < programs.Length; ii++)
            {
                copy[jj++] = programs[ii];
            }

            // Copy the rest
            var kk = 0;
            while (jj < programs.Length)
            {
                copy[jj++] = programs[kk++];
            }

            return copy;
        }

        static char[] Exchange(char[] programs, int slotA, int slotB)
        {
            var temp = programs[slotA];
            programs[slotA] = programs[slotB];
            programs[slotB] = temp;

            return programs;
        }

        static char[] Partner(char[] programs, char first, char second)
        {
            var index1 = -1;
            var index2 = -1;
            for (var ii = 0; ii < programs.Length; ii++)
            {
                if (programs[ii] == first || programs[ii] == second)
                {
                    if (index1 > -1) { index2 = ii; }
                    else index1 = ii;
                }
            }

            return Exchange(programs, index1, index2);
        }
    }
}

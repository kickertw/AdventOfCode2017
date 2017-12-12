using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            int groupCount = 1;
            var input = File.ReadAllLines(@".\Input\Day12Input.txt");
            var connectedList = new HashSet<string>() { "0" };
            connectedTo(input, ref connectedList);

            Console.WriteLine($"There are {connectedList.Count} items grouped with ZERO");

            // Find other groups
            for (var ii = 1; ii < input.Length; ii++)
            {
                if (!connectedList.Contains(ii.ToString()))
                {
                    connectedTo(input, ref connectedList, ii);
                    groupCount++;
                }
            }

            Console.Write($"There are {groupCount} individual groups");            
            Console.ReadLine();
        }

        static void connectedTo(string[] origList, ref HashSet<string> connectedList, int startingPoint = 0)
        {
            var connectedNodes = origList[startingPoint].Substring(origList[startingPoint].IndexOf("<->") + 4).Replace(" ", "").Split(',');

            foreach (var node in connectedNodes)
            {
                if (!connectedList.Contains(node))
                {
                    connectedList.Add(node);
                    connectedTo(origList, ref connectedList, int.Parse(node));
                }
            }
        }
    }
}

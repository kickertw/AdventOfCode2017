using System;
using System.Collections.Generic;
using System.IO;

namespace Day7Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = String.Empty;
            var parentNodes = new HashSet<string>();
            var childNodes = new HashSet<string>();
            var file = new StreamReader(@".\Input\Day7Input.txt");

            while((line = file.ReadLine()) != null)
            {
                if (line.Contains("->"))
                {
                    parentNodes.Add(line.Split(' ')[0]);
                    childNodes.UnionWith(line.Substring(line.IndexOf("->") + 3).Split(", "));
                }
            }

            foreach (var node in parentNodes)
            {
                if (!childNodes.Contains(node))
                {
                    Console.WriteLine($"The root node is [{node}]");
                    break;
                }
            }

            Console.ReadLine();
        }
    }
}

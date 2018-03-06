using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Day24Solution
{
    public class Node : ICloneable
    {
        public int PortCountA { get; set; }
        public int PortCountB { get; set; }

        public bool MatchesPortCount(int number)
        {
            return PortCountA == number || PortCountB == number;
        }

        public int Total()
        {
            return PortCountA + PortCountB;
        }

        public int OpenPortCount(int number)
        {
            return PortCountA == number ? PortCountB : PortCountA;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            var otherNode = (Node) obj;
            return this.PortCountA == otherNode.PortCountA && this.PortCountB == otherNode.PortCountB;
        }
    }

    class Program
    {
        private static int maxDepth = 0;

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(".\\input.txt");

            List<int[]> tempArr = lines.Select(line => line.Split('/').Select(int.Parse).ToArray()).ToList();
            List<Node> nodes = new List<Node>();
            foreach (var temp in tempArr)
            {
                nodes.Add(new Node()
                {
                    PortCountA = temp[0],
                    PortCountB = temp[1]
                });
            }


            int maxweight = -1;
            
            FindMaxDepth(0, 0, nodes);

            var tempBool = false;
            maxweight = FindMaxWeight(0, 0, ref tempBool, nodes);

            //Console.WriteLine();
            Console.WriteLine($"Max weight = {maxweight}");
            Console.ReadLine();
        }


        static void FindMaxDepth(int currentDepth, int portCountToMatch, List<Node> nodes = null)
        {
            if (nodes == null || nodes.Count == 0)
            {
                return;
            }

            if (currentDepth > maxDepth) maxDepth = currentDepth;

            foreach (var node in nodes)
            {
                if (node.MatchesPortCount(portCountToMatch))
                {
                    var subList = nodes.Clone();
                    subList.Remove(node);

                    FindMaxDepth(currentDepth + 1, node.OpenPortCount(portCountToMatch), subList);
                }
            }
        }

        static int FindMaxWeight(int startingWeight, int portCountToMatch, ref bool hitMaxDepth, List<Node> nodes = null, int depth = 0)
        {
            var retVal = 0;

            if (nodes == null || nodes.Count == 0)
            {
                return 0;
            }

            if (depth == maxDepth) hitMaxDepth = true;

            foreach (var node in nodes)
            {
                if (depth == 0) hitMaxDepth = false;

                if (node.MatchesPortCount(portCountToMatch))
                {
                    var currentWeightSum = node.Total();
                    var subList = nodes.Clone();
                    subList.Remove(node);
                    
                    currentWeightSum += FindMaxWeight(node.Total(), node.OpenPortCount(portCountToMatch), ref hitMaxDepth, subList, depth + 1);

                    if (hitMaxDepth && currentWeightSum > retVal)
                    {
                        retVal = currentWeightSum;
                    }
                }
            }

            return retVal;
        }
    }

    static class Extensions
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}

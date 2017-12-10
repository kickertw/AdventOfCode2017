using Day7Solution.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7Solution
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = String.Empty;
            var allNodeWeights = new Dictionary<string, int>();   //key = node name, value = weight;
            var allNodeParents = new Dictionary<string, List<string>>();
            var childNodes = new HashSet<string>();
            //var file = new StreamReader(@".\Input\TestInput.txt");
            var file = new StreamReader(@".\Input\Day7Input.txt");

            while ((line = file.ReadLine()) != null) {
                var splitLine = line.Split(' ');

                allNodeWeights.Add(splitLine[0], int.Parse(splitLine[1].TrimStart('(').TrimEnd(')')));
                if (line.Contains("->"))
                {
                    allNodeParents.Add(splitLine[0], line.Substring(line.IndexOf("->") + 3).Split(", ").ToList());
                    childNodes.UnionWith(line.Substring(line.IndexOf("->") + 3).Split(", "));
                }
            }

            //Part 1
            var rootNodeId = FindRootNode(allNodeParents, childNodes);
            Console.WriteLine($"The root node is [{rootNodeId}]");

            //Part 2
            var flatNodeList = new List<Node>();
            var rootNode = CreateNode(rootNodeId, allNodeWeights, allNodeParents, ref flatNodeList);
            var correctWeight = CorrectTreeWeight(rootNode, flatNodeList);

            Console.WriteLine($"The correct weight should be [{correctWeight}]");
            Console.ReadLine();
        }


        private static Node CreateNode(string rootNodeId, Dictionary<string, int> nodeWeights, Dictionary<string, List<string>> nodeParents, ref List<Node> flatNodeList, int depth = 1, string parentId = "")
        {
            Node retVal = new Node(rootNodeId, nodeWeights[rootNodeId], depth, parentId);
            flatNodeList.Add(retVal);

            if (nodeParents.ContainsKey(rootNodeId))
            {
                retVal.Children = new List<Node>();
                foreach (var childNode in nodeParents[rootNodeId])
                {
                    retVal.Children.Add(CreateNode(childNode, nodeWeights, nodeParents, ref flatNodeList, depth + 1, rootNodeId));
                }
            }

            return retVal;
        }

        // Find the Root Node (Part 1)
        private static string FindRootNode(Dictionary<string, List<string>> parentNodes, HashSet<string> childNodes)
        {
            foreach (var node in parentNodes.Keys.ToArray())
            {
                if (!childNodes.Contains(node))
                {
                    return node;
                }
            }

            throw new KeyNotFoundException();
        }

        // Find where there is an issue and what the weight correction would be
        private static int CorrectTreeWeight(Node rootNode, List<Node> flatNodeList)
        {
            int? correctWeight = null;
            var curDepth = Node.GetMaxDepth(rootNode);

            // Should load all the weights for each node in the tree
            rootNode.GetTotalWeight();

            while (curDepth > 1)
            {
                var parentIds = flatNodeList.FindAll(i => i.Depth == curDepth).Select(i => i.ParentId).Distinct();

                foreach (var parentId in parentIds)
                {
                    var node = rootNode.FindById(parentId);
                    var badNode = WhichNodeIsBad(node.Children);

                    if (badNode != null)
                    {
                        foreach(var child in node.Children)
                        {
                            if (child.Id != badNode.Id)
                            {
                                var delta = child.GetTotalWeight() - badNode.GetTotalWeight();
                                correctWeight = badNode.Weight + delta;
                                break;
                            }
                        }
                    }

                    if (correctWeight.HasValue) return correctWeight.Value;
                }

                curDepth--;
            }

            return int.MinValue;
        }

        private static Dictionary<string, int> FindNodeWeights(List<string> nodes, Dictionary<string, int> allNodes)
        {
            var retVal = new Dictionary<string, int>();

            foreach (var nodeName in nodes)
            {
                retVal.Add(nodeName, allNodes[nodeName]);
            }

            return retVal;
        }

        private static Node WhichNodeIsBad(List<Node> nodeList)
        {
            Node badNode = null;

            for(var ii = 1; ii < nodeList.Count && badNode == null; ii++)
            {
                if (nodeList[ii-1].GetTotalWeight() != nodeList[ii].GetTotalWeight())
                {
                    if (ii == 1)
                    {
                        if (nodeList[2].GetTotalWeight() == nodeList[0].GetTotalWeight())
                        {
                            badNode = nodeList[1];
                        }
                        else
                        {
                            badNode = nodeList[0];
                        }                        
                    }
                    else
                    {
                        badNode = nodeList[ii];
                    }
                }
            }

            if (badNode != null)
            {
                Console.WriteLine($"Bad node found! [{badNode.Id}]");
            }

            return badNode;
        }
    }
}

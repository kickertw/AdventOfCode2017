using System;
using System.Collections.Generic;
using System.Linq;

namespace Day7Solution.Model
{
    public class Node
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int Weight { get; set; }
        public int Depth { get; set; }

        public List<Node> Children { get; set; }

        public Node()
        {
        }

        public Node(string id, int weight, int depth, string parentId)
        {
            Id = id;
            Weight = weight;
            Depth = depth;
            ParentId = parentId;
        }

        public Node FindById(string id)
        {
            Node retVal = null;

            if (Id == id) return this;

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    retVal = child.FindById(id);
                    if (retVal != null) break;
                }
            }

            return retVal;
        }

        /// <summary>
        /// Returns the weight of the node and the sum of it's child nodes
        /// </summary>
        /// <returns></returns>
        public int GetTotalWeight()
        {
            var retVal = 0;

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    retVal += child.GetTotalWeight();
                }
            }

            return retVal + Weight;
        }

        public static int GetMaxDepth(Node rootNode)
        {
            var depth = rootNode.Depth;
            var baseNode = rootNode;
            while (baseNode.Children != null)
            {
                depth = baseNode.Depth;
                baseNode = baseNode.Children.FirstOrDefault();
            }

            return depth;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStar
{
    public static class AStarEngine
    {
        private class NodeDescription<TNode>
        {
            public float G { get; set; }

            public float F { get; set; }

            public TNode Predecessor { get; set; }
        }

        public static bool FindPathTo<TNode>(this TNode startNode, TNode endNode, out Stack<TNode> path) where TNode : class, IPathNode<TNode>
        {
            path = null;

            var descriptions = new Dictionary<TNode, NodeDescription<TNode>>(); 
            descriptions.Add(startNode, new NodeDescription<TNode>());

            if (!startNode.IsPassable || !endNode.IsPassable)
                return false;

            var openList = new List<TNode>();
            openList.Add(startNode);
            var closedList = new List<TNode>();
            
            do
            {
                TNode currentNode = openList[0];
                openList.RemoveAt(0);

                if (currentNode == endNode)
                {
                    path = new Stack<TNode>();
                    TNode node = currentNode;
                    while (node != null)
                    {
                        path.Push(node);
                        node = descriptions[node].Predecessor;
                    }

                    return true;
                }

                closedList.Add(currentNode);

                for (int i = 0; i < currentNode.Neighbors.Length; i++)
                {
                    TNode neighbor = currentNode.Neighbors[i];

                    if (!neighbor.IsPassable || closedList.Contains(neighbor))
                        continue;

                    if (!descriptions.ContainsKey(neighbor))
                        descriptions.Add(neighbor, new NodeDescription<TNode>());

                    float g = descriptions[currentNode].G + currentNode.DistanceTo(neighbor);

                    bool isInOpenList = openList.Contains(neighbor);
                    if (isInOpenList && g >= descriptions[neighbor].G)
                        continue;

                    descriptions[neighbor].Predecessor = currentNode;
                    descriptions[neighbor].G = g;
                    descriptions[neighbor].F = g + neighbor.DistanceTo(endNode);
                    if (!isInOpenList) openList.Add(neighbor);
                }

                openList.Sort((x, y) => descriptions[x].F.CompareTo(descriptions[y].F));
            } while (openList.Count > 0);

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AStar;

namespace AStarTester
{
    public class Tile : IPathNode<Tile>
    {
        public Tile[] Neighbors { get; set; }

        public bool IsPassable { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public float DistanceTo(Tile node)
        {
            int dX = X - node.X;
            int dY = Y - node.Y;
            return (float)Math.Sqrt(dX * dX + dY * dY);
        }
    }
}

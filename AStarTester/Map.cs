using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AStar;

namespace AStarTester
{
    public class Map
    {
        readonly Tile[,] tiles;

        public Tile this[int x, int y]
        {
            get { return tiles[x, y]; }
            set
            {
                tiles[x, y] = value;
                value.X = x;
                value.Y = y;
            }
        }

        public Map(int width, int height)
        {
            tiles = new Tile[width, height];
        }
    }
}

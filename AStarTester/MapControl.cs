using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AStar;

namespace AStarTester
{
    public class MapControl : Control
    {
        readonly Map map;
        const int TileSize = 15;
        Tile startTile;
        Tile endTile;
        Stack<Tile> pathStack;
        readonly List<Tile> pathList;
        readonly Timer timer;
        Tile tile_MouseOver;

        public SelectionMode SelectionMode { get; set; }

        public MapControl(int width, int height)
        {
            map = new Map(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = new Tile() { IsPassable = true };
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var neighbors = new List<Tile>();
                    for (int i = Math.Max(x - 1, 0); i < Math.Min(x + 2, width); i++)
                    {
                        for (int j = Math.Max(y - 1, 0); j < Math.Min(y + 2, height); j++)
                        {
                            if (i == x && j == y)
                                continue;

                            neighbors.Add(map[i, j]);
                        }
                    }
                    map[x, y].Neighbors = neighbors.ToArray();
                }
            }
            startTile = map[0, 0];
            endTile = map[width - 1, height - 1];

            Size = new Size(width * TileSize + 1, height * TileSize + 1);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
            this.UpdateStyles();

            pathList = new List<Tile>();
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += OnTimerTick;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                int x = Math.Min(e.X, Width - 2) / TileSize;
                int y = Math.Min(e.Y, Height - 2) / TileSize;
                Tile tile = map[x, y];

                tile_MouseOver = tile;

                switch (SelectionMode)
                {
                    case SelectionMode.Start:
                        startTile = tile;
                        break;
                    case SelectionMode.End:
                        endTile = tile;
                        break;
                    default:
                        tile.IsPassable = !tile.IsPassable;
                        break;
                }

                pathList.Clear();
                if (timer.Enabled) timer.Stop();
                this.Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && SelectionMode == SelectionMode.Wall)
            {
                if (!(e.X < 0 || e.X > Width || e.Y < 0 || e.Y > Height))
                {
                    int x = Math.Min(e.X, Width - 2) / TileSize;
                    int y = Math.Min(e.Y, Height - 2) / TileSize;
                    Tile tile = map[x, y];

                    if (tile != tile_MouseOver)
                    {
                        tile.IsPassable = !tile.IsPassable;
                        tile_MouseOver = tile;

                        this.Invalidate();
                    }
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            tile_MouseOver = null;
            
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int x = 0, i = 0; x < Width - 1; x += TileSize, i++)
            {
                for (int y = 0, j = 0; y < Height - 1; y += TileSize, j++)
                {
                    Tile tile = map[i, j];

                    var rect = new Rectangle(x, y, TileSize, TileSize);
                    if (!tile.IsPassable)
                        g.FillRectangle(Brushes.Black, rect);
                    else if (tile == startTile)
                        g.FillRectangle(Brushes.Blue, rect);
                    else if (tile == endTile)
                        g.FillRectangle(Brushes.LimeGreen, rect);
                    else if (pathList.Contains(tile))
                        g.FillRectangle(Brushes.Red, rect);
                }
            }

            for (int x = 0; x < Width; x += TileSize)
                g.DrawLine(Pens.Black, new Point(x, 0), new Point(x, Height));
            for (int y = 0; y < Height; y += TileSize)
                g.DrawLine(Pens.Black, new Point(0, y), new Point(Width, y));

            base.OnPaint(e);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (pathStack == null || pathStack.Count <= 0)
            {
                timer.Stop();
                return;
            }

            pathList.Add(pathStack.Pop());
            this.Invalidate();
        }

        public void FindPath()
        {
            pathList.Clear();
            if (startTile.FindPathTo(endTile, out pathStack))
            {
                timer.Start();
                this.Invalidate();
            }
        }
    }

    public enum SelectionMode
    {
        Wall,
        Start,
        End,
    }
}

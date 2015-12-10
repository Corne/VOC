using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Boards
{
    public class Vertex : IVertex
    {
        public Vertex(int x, int y, VertexTileSide side)
        {
            X = x;
            Y = y;
            Side = side;
        }

        public int X { get; }
        public int Y { get; }

        public VertexTileSide Side { get; }

        //(x,y,L) → (x,y,W) (x-1,y,E) (x-1,y,N)
        //(x,y,R) → (x+1,y-1,N) (x+1,y-1,W) (x,y,E)
        public bool IsAdjacentTo(IEdge edge)
        {
            if (edge == null)
                return false;

            switch (Side)
            {
                case VertexTileSide.Left:
                    return IsAdjacentToLeft(edge);
                case VertexTileSide.Right:
                    return IsAdjacentToRight(edge);
            }

            return false;
        }

        private bool IsAdjacentToLeft(IEdge edge)
        {
            switch (edge.Side)
            {
                case EdgeSide.West:
                    return X == edge.X && Y == edge.Y;
                case EdgeSide.East:
                case EdgeSide.North:
                    return X - 1 == edge.X && Y == edge.Y;
            }
            return false;
        }

        private bool IsAdjacentToRight(IEdge edge)
        {
            switch (edge.Side)
            {
                case EdgeSide.West:
                case EdgeSide.North:
                    return X + 1 == edge.X && Y - 1 == edge.Y;
                case EdgeSide.East:
                    return X == edge.X && Y == edge.Y;
            }
            return false;
        }

        public bool IsAdjacentTo(ITile tile)
        {
            if (tile == null)
                return false;

            if (X == tile.X && Y == tile.Y)
                return true;

            int adjustedX = X + (int)Side;
            if (adjustedX == tile.X && (Y == tile.Y || Y - 1 == tile.Y))
                return true;

            return false;
        }
    }
}

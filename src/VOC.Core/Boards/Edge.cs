using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Boards
{
    public class Edge : IEdge
    {
        public Edge(int x, int y, EdgeSide side)
        {
            X = x;
            Y = y;
            Side = side;
        }

        public EdgeSide Side { get; }

        public int X { get; }

        public int Y { get; }

        //W: (x-1, y, E) (x-1, y+1, E) (x-1, y, N)  (x, y, N)
        //N: (x,y,W) (x, y, E), (x-1, y+1, E), (x+1, y, W)
        //E: (x+1, y-1, W) (x+1, y-1, N) (x+1, y, W) (x, y, N)
        public bool IsAdjacentTo(IEdge edge)
        {
            if (edge == null)
                return false;

            switch (Side)
            {
                case EdgeSide.West:
                    return IsAdjacentToWest(edge);
                case EdgeSide.North:
                    return IsAdjacentToNorth(edge);
                case EdgeSide.East:
                    return IsAdjacentToEast(edge);
            }
            return false;
        }

        //W: (x-1, y, E) (x-1, y+1, E) (x-1, y, N)  (x, y, N)
        private bool IsAdjacentToWest(IEdge edge)
        {
            switch (edge.Side)
            {
                case EdgeSide.East:
                    return X - 1 == edge.X && (Y == edge.Y || Y + 1 == edge.Y);
                case EdgeSide.North:
                    return (X - 1 == edge.X || X == edge.X) && Y == edge.Y;
            }
            return false;
        }

        //N: (x,y,W) (x, y, E), (x-1, y+1, E), (x+1, y, W)
        private bool IsAdjacentToNorth(IEdge edge)
        {
            if (edge.Side == EdgeSide.North)
                return false;

            if (X == edge.X && Y == edge.Y)
                return true;

            switch (edge.Side)
            {
                case EdgeSide.West:
                    return X + 1 == edge.X && Y == edge.Y;
                case EdgeSide.East:
                    return X - 1 == edge.X && Y + 1 == edge.Y;
            }
            return false;
        }
        //E: (x+1, y-1, W) (x+1, y-1, N) (x+1, y, W) (x, y, N)
        private bool IsAdjacentToEast(IEdge edge)
        {
            switch (edge.Side)
            {
                case EdgeSide.West:
                    return X + 1 == edge.X && (Y == edge.Y || Y - 1 == edge.Y);
                case EdgeSide.North:
                    return (X + 1 == edge.X && Y - 1 == edge.Y) || (X == edge.X && Y == edge.Y);
            }
            return false;
        }

        public bool IsAdjacentTo(ITile tile)
        {
            if (tile == null)
                return false;

            if (X == tile.X && Y == tile.Y)
                return true;

            switch (Side)
            {
                case EdgeSide.East:
                    return X + 1 == tile.X && Y == tile.Y;
                case EdgeSide.North:
                    return X == tile.X && Y + 1 == tile.Y;
                case EdgeSide.West:
                    return X - 1 == tile.X && Y + 1 == tile.Y;
            }

            return false;
        }


        //(x,y,N) → (x+1,y,L) (x-1,y+1,R)
        //(x,y,E) → (x,y,R) (x+1,y,L)
        //(x,y,W) → (x-1,y+1,R) (x,y,L)
        public bool IsAdjacentTo(IVertex vertex)
        {
            if (vertex == null)
                return false;

            switch (vertex.Side)
            {
                case VertexTileSide.Left:
                    return IsAdjacentToLeftVertex(vertex);
                case VertexTileSide.Right:
                    return IsAdjacentToRightVertex(vertex);
            }

            return false;
        }

        private bool IsAdjacentToLeftVertex(IVertex vertex)
        {
            switch (Side)
            {
                case EdgeSide.West:
                    return X == vertex.X && Y == vertex.Y;
                case EdgeSide.East:
                case EdgeSide.North:
                    return X + 1 == vertex.X && Y == vertex.Y;
            }
            return false;
        }

        private bool IsAdjacentToRightVertex(IVertex vertex)
        {
            switch (Side)
            {
                case EdgeSide.East:
                    return X == vertex.X && Y == vertex.Y;
                case EdgeSide.West:
                case EdgeSide.North:
                    return X - 1 == vertex.X && Y + 1 == vertex.Y;
            }
            return false;
        }
    }
}

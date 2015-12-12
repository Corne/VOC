using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Boards
{
    public class Tile : ITile, IBoardComponent
    {

        public Tile(int x, int y, int number, MaterialType material)
        {
            X = x;
            Y = y;
            Number = number;
            Rawmaterial = material;
        }
        public int Number { get; }

        public MaterialType Rawmaterial { get; }

        public int X { get; }

        public int Y { get; }

        public IRawMaterial Farm()
        {
            return new RawMaterial(Rawmaterial);
        }

        public bool IsAdjacentTo(IEdge edge)
        {
            if (edge == null)
                return false;

            if (edge.X == X && edge.Y == Y)
                return true;

            switch (edge.Side)
            {
                case EdgeSide.North:
                    return X == edge.X && Y - 1 == edge.Y;
                case EdgeSide.East:
                    return X - 1 == edge.X && Y == edge.Y;
                case EdgeSide.West:
                    return X + 1 == edge.X && Y - 1 == edge.Y;
            }
            return false;
        }

        public bool IsAdjacentTo(ITile tile)
        {
            if(tile == null)
                return false;

            if (X == tile.X && Y == tile.Y)
                return false;

            int xDiff = tile.X - X;
            int yDiff = tile.Y - Y;

            int result = Math.Abs(xDiff + yDiff);

            return new int[] { Math.Abs(xDiff), Math.Abs(yDiff), result }.All(v => v <= 1);
        }

        //Top-Left:   (x-1,y+1,R) Top-Right:   (x+1,y,L) 
        //Bottom-Left:(x-1, y, R) Bottom-Right:(x+1, y-1, L)

        public bool IsAdjacentTo(IVertex vertex)
        {
            if (vertex == null)
                return false;

            if (X == vertex.X && Y == vertex.Y)
                return true;

            switch (vertex.Side)
            {
                //could change to single statement with adjusment value based on side
                case VertexTileSide.Left:
                    return X + 1 == vertex.X && (Y == vertex.Y || Y - 1 == vertex.Y);
                case VertexTileSide.Right:
                    return X - 1 == vertex.X && (Y == vertex.Y || Y + 1 == vertex.Y);
            }

            return false;
        }
    }
}

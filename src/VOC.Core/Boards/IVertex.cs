using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Boards
{
    public interface IVertex
    {
        int X { get; }
        int Y { get; }
        VertexTileSide Side { get; } 

        bool IsAdjacentTo(ITile tile);
        bool IsAdjacentTo(IEdge edge);
    }

    public enum VertexTileSide
    {
        Left,
        Right
    }
}

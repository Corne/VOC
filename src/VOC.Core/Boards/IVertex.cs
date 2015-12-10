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

        //CvB Todo, BoardComponent interface which contains all those 3 methods?
        bool IsAdjacentTo(ITile tile);
        bool IsAdjacentTo(IEdge edge);
    }

    public enum VertexTileSide
    {
        Left = -1,
        Right = 1
    }
}

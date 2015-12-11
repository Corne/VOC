using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Boards
{
    public interface IVertex : IBoardComponent
    {
        int X { get; }
        int Y { get; }
        VertexTileSide Side { get; }
    }

    public enum VertexTileSide
    {
        Left = -1,
        Right = 1
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Boards
{
    public interface IBoardComponent
    {
        bool IsAdjacentTo(IVertex vertex);
        bool IsAdjacentTo(ITile tile);
        bool IsAdjacentTo(IEdge edge);
    }
}

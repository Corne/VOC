using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Boards
{
    public interface IRoad
    {
        IEdge Edge { get; }
        IPlayer Owner { get; }

        bool IsAdjacentTo(IRoad road);
    }
}

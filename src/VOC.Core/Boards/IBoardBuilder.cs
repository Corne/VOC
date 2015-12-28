using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Boards
{
    public interface IBoardBuilder
    {
        void Build();

        IEnumerable<ITile> Tiles { get; }
        IEnumerable<IVertex> Vertices { get; }
        IEnumerable<IEdge> Edges { get; }
    }
}

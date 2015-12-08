using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Establishments;

namespace VOC.Core.Boards
{
    public interface IBoard
    {
        ITile[] Tiles { get; }

        IEnumerable<ITile> GetTiles(int number);
        IEnumerable<IEstablishment> GetEstablishments(ITile tile);
    }
}

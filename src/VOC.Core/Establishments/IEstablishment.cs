using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Players;

namespace VOC.Core.Establishments
{
    public interface IEstablishment
    {
        IPlayer Owner { get; }

        void Harvest(ITile tile);
    }
}

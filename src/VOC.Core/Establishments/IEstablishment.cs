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
        /// <summary>
        /// Owner of the establisment
        /// </summary>
        IPlayer Owner { get; }
        /// <summary>
        /// Vertex the establisment is placed on
        /// </summary>
        IVertex Vertex { get; }

        /// <summary>
        /// Harvest an adjacent tile
        /// </summary>
        /// <param name="tile"></param>
        void Harvest(ITile tile);
        /// <summary>
        /// Upgrade establisment to the next level
        /// </summary>
        void Upgrade();
    }
}

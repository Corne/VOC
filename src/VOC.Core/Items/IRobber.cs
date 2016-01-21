using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Players;

namespace VOC.Core.Items
{
    public interface IRobber
    {
        event EventHandler<ITile> Moved;

        ITile CurrentTile { get; }
        /// <summary>
        /// Move the robber to a new tile
        /// </summary>
        /// <param name="tile"></param>
        void Move(ITile tile);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Establishments;

namespace VOC.Core.Boards
{
    //http://www-cs-students.stanford.edu/~amitp/game-programming/grids/
    public interface IBoard
    {

        /// <summary>
        /// Get all tiles that have the specific number
        /// </summary>
        /// <param name="number">Number you want the tiles of</param>
        /// <returns>All Tiles on the board with given number</returns>
        IEnumerable<ITile> GetTiles(int number);

        /// <summary>
        /// Get all establiments that are adjacent to the tile
        /// </summary>
        /// <param name="tile">tile you want all establisments off</param>
        /// <returns>Enumberable of establiments that are adjacent to the tile</returns>
        IEnumerable<IEstablishment> GetEstablishments(ITile tile);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Establishments;
using VOC.Core.Players;

namespace VOC.Core.Boards
{
    //http://www-cs-students.stanford.edu/~amitp/game-programming/grids/
    public interface IBoard
    {
        IEnumerable<ITile> Tiles { get; }
        IEnumerable<IVertex> Vertices { get; }
        IEnumerable<IEdge> Edges { get; }
        IEnumerable<IEstablishment> Establishments { get; }
        IEnumerable<IRoad> Roads { get; }
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

        /// <summary>
        /// Build an establisment
        /// CvB ToDo: validate if this is the correct place for this method
        /// </summary>
        /// <param name="vertex">Vertex the establisment will be placed on</param>
        /// <param name="owner">Player who will own the establisment</param>
        /// <returns>The created establisment</returns>
        IEstablishment BuildEstablishment(IVertex vertex, IPlayer owner);

        /// <summary>
        /// Build a road
        /// CvB ToDo: validate if this is the correct place for this method
        /// </summary>
        /// <param name="edge">edge the road will be placed on</param>
        /// <param name="owner">owner of the road</param>
        /// <returns>The created road</returns>
        IRoad BuildRoad(IEdge edge, IPlayer owner);
    }
}

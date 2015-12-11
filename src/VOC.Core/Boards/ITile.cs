using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Boards
{
    //Todo: CvB interface is maybe not correct, Dessert should not have a farm method?
    public interface ITile
    {
        /// <summary>
        /// Tile Number
        /// Value between 1 and 12, that corresponds with a dice throw
        /// </summary>
        int Number { get; }

        /// <summary>
        /// RawmaterialType this Tile produces
        /// </summary>
        MaterialType Rawmaterial { get; }

        /// <summary>
        /// X Coordinate of the tile
        /// </summary>
        int X { get; }
        /// <summary>
        /// Y Cooridnate of the tile
        /// </summary>
        int Y { get; }

        /// <summary>
        /// Farm the tile, will create a new material with the tiles materialtype
        /// </summary>
        /// <returns>A Rawmaterial, where type is materialtype</returns>
        IRawMaterial Farm();
    }
}

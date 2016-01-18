using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Boards
{
    public interface IHarbor
    {
        /// <summary>
        /// Edge this harbor is aligned to
        /// </summary>
        IEdge Edge { get; }

        /// <summary>
        /// Tile this harbor is on, should be sea vertex
        /// </summary>
        ITile Tile { get; }

        /// <summary>
        /// Material this harbor gives a discount on
        /// Unsourced is generic 1 : 3, Sea is invalid
        /// Others are 2 : 1 discount
        /// </summary>
        MaterialType Discount { get; }
    }
}

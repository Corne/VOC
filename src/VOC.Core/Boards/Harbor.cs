using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Boards
{
    public class Harbor : IHarbor
    {

        public Harbor(MaterialType material, IEdge edge, ITile tile)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));
            if (!Enum.IsDefined(typeof(MaterialType), material) || material == MaterialType.Sea)
                throw new ArgumentException("Invalid discount material, should be defined enum value and not sea type");
            if (tile.Rawmaterial != MaterialType.Sea)
                throw new ArgumentException("Tile should be sea tile");
            if (!edge.IsAdjacentTo(tile))
                throw new ArgumentException("Edge and tile should be adjacent");

            Discount = material;
            Edge = edge;
            Tile = tile;
        }

        public MaterialType Discount { get; }
        public IEdge Edge { get; }
        public ITile Tile { get; }
    }
}

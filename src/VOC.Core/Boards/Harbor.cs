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
            Discount = material;
            Edge = edge;
            Tile = tile;
        }

        public MaterialType Discount { get; }
        public IEdge Edge { get; }
        public ITile Tile { get; }
    }
}

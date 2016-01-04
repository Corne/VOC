using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Boards
{
    public class Road : IRoad
    {
        public static readonly MaterialType[] BUILD_RESOURCES = { MaterialType.Lumber, MaterialType.Brick };

        public IEdge Edge { get; }

        public IPlayer Owner { get; }

        public Road(IEdge edge, IPlayer owner)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            Edge = edge;
            Owner = owner;
        }
    }
}

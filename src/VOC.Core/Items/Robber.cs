using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Items
{
    public class Robber : IRobber
    {
        private static ILog logger = LogManager.GetLogger(nameof(Robber));

        public event EventHandler<ITile> Moved;

        public ITile CurrentTile { get; private set; }

        public Robber(ITile initialTile)
        {
            if (initialTile == null)
                throw new ArgumentNullException(nameof(initialTile));
            if (initialTile.Rawmaterial != MaterialType.Unsourced)
                throw new ArgumentException("Robber should always start on the desert!");

            CurrentTile = initialTile;
        }

        public void Move(ITile tile)
        {
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));

            if (tile.Rawmaterial == MaterialType.Sea)
                throw new ArgumentException("Robber can't placed on a sea tile!");

            CurrentTile = tile;
            Moved?.Invoke(this, CurrentTile);
            logger.Info($"Robber moved to {CurrentTile}");
        }
    }
}

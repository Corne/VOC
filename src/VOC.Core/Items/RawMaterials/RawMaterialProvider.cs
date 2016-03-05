using System;
using System.Collections.Generic;
using VOC.Core.Boards;
using VOC.Core.Establishments;
using VOC.Core.Games;

namespace VOC.Core.Items.RawMaterials
{
    public class RawMaterialProvider : IRawmaterialProvider
    {
        private readonly IBoard board;

        public RawMaterialProvider(IBoard board)
        {
            this.board = board;
        }

        public void Distribute(int number)
        {
            if (number <= 0 || number > 12 || number == 7)
                throw new ArgumentException("Can only distrubte materials between 1 and 12, and excluding 7 because of it's the desserts number");

            IEnumerable<ITile> tiles = board.GetResourceTiles(number);
            foreach (ITile tile in tiles)
            {
                IEnumerable<IEstablishment> establisments = board.GetEstablishments(tile);
                foreach (var establisment in establisments)
                {
                    establisment.Harvest(tile);
                }
            }
        }
    }
}

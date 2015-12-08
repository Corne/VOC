using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Establishments;

namespace VOC.Core.Games
{
    public class RawMaterialProvider
    {
        private readonly IBoard board;

        public RawMaterialProvider(IBoard board)
        {
            this.board = board;
        }


        public void Distrubte(int value)
        {
            if (value <= 0 || value > 12 || value == 7)
                throw new ArgumentException("Can only distrubte materials between 1 and 12, and excluding 7 because of it's the robbers number");

            IEnumerable<ITile> tiles = board.GetTiles(value);

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

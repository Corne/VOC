using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;

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


        }
    }
}

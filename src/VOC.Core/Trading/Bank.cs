using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Trading
{
    public class Bank : IBank
    {
        private readonly IBoard board;

        public Bank(IBoard board)
        {
            if (board == null)
                throw new ArgumentNullException("Board can't be null");
            this.board = board;
        }

        public void BuyResource(MaterialType request, MaterialType offer, IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}

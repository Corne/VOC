using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Trading
{
    public interface IBank
    {
        /// <summary>
        /// Buy resources from the bank
        /// You can buy 1 resource a time, and the bank will check how much resource the player will need to pay based on docks
        /// </summary>
        /// <param name="request">MaterialType player wants</param>
        /// <param name="offer">MaterialType player offers, player requires 2-4 based on docks</param>
        /// <param name="player">The player who buys resources from the bank</param>
        void BuyResource(MaterialType request, MaterialType offer, IPlayer player);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Trading
{
    public interface IBank
    {
        /// <summary>
        /// Buy resources from the bank
        /// You can buy 1 resource a time, and the bank will check how much resource the player will need to pay based on docks
        /// http://boardgames.stackexchange.com/questions/5970/maritime-trading-both-ways
        /// </summary>
        /// <param name="request">MaterialType player wants</param>
        /// <param name="offer">MaterialType player offers, player requires 2-4 based on docks</param>
        /// <param name="player">The player who buys resources from the bank</param>
        void BuyResource(MaterialType request, MaterialType offer, IPlayer player);

        /// <summary>
        /// Get the Cost of resources you need to pay, when you want to buy something from the bank with specified resource
        /// </summary>
        /// <param name="offer">Material you want to use as offer</param>
        /// <param name="player">player </param>
        /// <returns>Array of MaterialTypes (of type offer) that will be needed to make an offer</returns>
        MaterialType[] GetInvestmentCost(MaterialType offer, IPlayer player);

        void BuyDevelopmentCard(IPlayer player, ITurn turn);
    }
}

using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Trading
{
    public interface ITrade
    {

        /// <summary>
        /// Materials that are requested
        /// </summary>
        MaterialType[] Request { get; }
        /// <summary>
        /// Materials that are offered
        /// </summary>
        MaterialType[] Offer { get; }
        /// <summary>
        /// Player who requested the trade
        /// </summary>
        IPlayer Owner { get; }
        /// <summary>
        /// State the trade is in
        /// </summary>
        TradeState State { get; }

        /// <summary>
        /// Accept the trade
        /// Trade can not be accepted if Request or Offer is empty, both parties should put items in a trade
        /// </summary>
        /// <param name="player">player who accepts the trade</param>
        void Accept(IPlayer player);
        /// <summary>
        /// Cancel the trade
        /// </summary>
        void Cancel();
    }


}
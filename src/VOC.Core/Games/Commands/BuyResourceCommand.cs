using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using VOC.Core.Trading;

namespace VOC.Core.Games.Commands
{
    public class BuyResourceCommand : IPlayerCommand
    {
        private readonly IBank bank;
        private readonly MaterialType buy;
        private readonly MaterialType offer;

        public BuyResourceCommand(IPlayer player, IBank bank, MaterialType buy, MaterialType offer)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (bank == null)
                throw new ArgumentNullException(nameof(bank));
            if (buy == offer)
                throw new ArgumentException("Buy and Offer material cant be same type");
            Player = player;
            this.bank = bank;
            this.buy = buy;
            this.offer = offer;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.Trade; } }

        public void Execute()
        {
            bank.BuyResource(buy, offer, Player);
        }
    }
}

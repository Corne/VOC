using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using VOC.Core.Trading;

namespace VOC.Core.Games.Commands
{
    public class OpenTradeCommand : IPlayerCommand
    {
        private readonly IMarket market;
        private readonly ITrade trade;

        public OpenTradeCommand(IPlayer player, IMarket market, ITrade trade)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (market == null)
                throw new ArgumentNullException(nameof(market));
            if (trade == null)
                throw new ArgumentNullException(nameof(trade));
            Player = player;
            this.market = market;
            this.trade = trade;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.Trade; } }

        public void Execute()
        {
            //CvB Todo: This seems kinda weird to have a whole class for this?
            market.OpenTrade(trade);
        }
    }
}

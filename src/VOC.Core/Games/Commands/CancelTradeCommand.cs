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
    public class CancelTradeCommand : IPlayerCommand
    {
        private readonly ITrade trade;

        public CancelTradeCommand(IPlayer player, ITrade trade)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (trade == null)
                throw new ArgumentNullException(nameof(trade));
            if (trade.Owner != player)
                throw new ArgumentException("Executing player should be the owner of the trade");
            Player = player;
            this.trade = trade;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.Trade; } }

        public void Execute()
        {
            trade.Cancel();
        }
    }
}

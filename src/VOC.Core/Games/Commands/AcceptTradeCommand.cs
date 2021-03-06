﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using VOC.Core.Trading;

namespace VOC.Core.Games.Commands
{
    public class AcceptTradeCommand : IPlayerCommand
    {
        private readonly ITrade trade;

        public AcceptTradeCommand(IPlayer player, ITrade trade)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (trade == null)
                throw new ArgumentNullException(nameof(trade));
            Player = player;
            this.trade = trade;
        }

        public IPlayer Player { get; }
        public GameCommand Type { get { return GameCommand.Trade; } }

        public void Execute()
        {
            trade.Accept(Player);
        }
    }
}

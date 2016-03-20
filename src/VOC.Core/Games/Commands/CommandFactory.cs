using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using VOC.Core.Boards;
using VOC.Core.Players;
using VOC.Core.Trading;

namespace VOC.Core.Games.Commands
{
    public class CommandFactory
    {
        private readonly ILifetimeScope scope;

        public CommandFactory(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public AcceptTradeCommand NewAcceptTrade(IPlayer player, Guid id)
        {
            var market = scope.Resolve<IMarket>();
            var trade = market.ActiveTrades.FirstOrDefault(t => t.Id == id);
            if (trade == null)
                throw new ArgumentException("No trade found for given id");
            return new AcceptTradeCommand(player, trade);
        }

        public BuildDevelopmentRoadCommand NewBuildDevelopmentRoad(IPlayer player, int x, int y, EdgeSide side)
        {
            var board = scope.Resolve<IBoard>();
            var edge = board.Edges.FirstOrDefault(e => e.X == x && e.Y == y && e.Side == side);
            if (edge == null)
                throw new ArgumentException($"Not edge found for given coordinates (x: {x}, y: {y}, side: {side})");
            return new BuildDevelopmentRoadCommand(player, board, edge);
        }
    }
}

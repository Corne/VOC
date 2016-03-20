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
            var trade = market.Find(id);
            if (trade == null)
                throw new ArgumentException("No trade found for given id");
            return new AcceptTradeCommand(player, trade);
        }

        public BuildDevelopmentRoadCommand NewBuildDevelopmentRoad(IPlayer player, int x, int y, EdgeSide side)
        {
            var board = scope.Resolve<IBoard>();
            var edge = board.FindEdge(x, y, side);
            if (edge == null)
                throw new ArgumentException($"No edge found for given coordinates (x: {x}, y: {y}, side: {side})");
            return new BuildDevelopmentRoadCommand(player, board, edge);
        }

        public BuildEstablishmentCommand NewBuildEstablishment(IPlayer player, int x, int y, VertexTileSide side)
        {
            var board = scope.Resolve<IBoard>();
            var vertex = board.FindVertex(x, y, side);
            if (vertex == null)
                throw new ArgumentException($"No vertex found for given coordinates (x: {x}, y: {y}, side: {side}");
            return new BuildEstablishmentCommand(player, board, vertex);
        }

        public BuildRoadCommand NewBuildRoad(IPlayer player, int x, int y, EdgeSide side)
        {
            var board = scope.Resolve<IBoard>();
            var edge = board.FindEdge(x, y, side);
            if (edge == null)
                throw new ArgumentException($"No edge found for given coordinates (x: {x}, y: {y}, side: {side})");
            return new BuildRoadCommand(player, board, edge);
        }

        public CancelTradeCommand NewCancelTrade(IPlayer player, Guid id)
        {
            var market = scope.Resolve<IMarket>();
            var trade = market.Find(id);
            if (trade == null)
                throw new ArgumentException("No trade found for given id");
            return new CancelTradeCommand(player, trade);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using VOC.Core.Boards;
using VOC.Core.Items;
using VOC.Core.Items.RawMaterials;
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

        public BuyDevelopmentCardCommand NewBuyDevelopmentCard(IPlayer player)
        {
            var game = scope.Resolve<IGame>();
            return new BuyDevelopmentCardCommand(player, game);
        }

        public BuyResourceCommand NewBuyResource(IPlayer player, MaterialType buy, MaterialType offer)
        {
            var bank = scope.Resolve<IBank>();
            return new BuyResourceCommand(player, bank, buy, offer);
        }

        public CancelTradeCommand NewCancelTrade(IPlayer player, Guid id)
        {
            var market = scope.Resolve<IMarket>();
            var trade = market.Find(id);
            if (trade == null)
                throw new ArgumentException("No trade found for given id");
            return new CancelTradeCommand(player, trade);
        }

        public DiscardResourcesCommand NewDiscardResources(IPlayer player, IEnumerable<MaterialType> materials)
        {
            return new DiscardResourcesCommand(player, materials);
        }

        public HighRollCommand NewHighRollCommand(IPlayer player)
        {
            var dice = scope.Resolve<IDice>();
            return new HighRollCommand(player, dice);
        }

        public MonopolyCommand NewMonopolyCommand(IPlayer player, MaterialType material)
        {
            var game = scope.Resolve<IGame>();
            var players = game.Players.Except(new[] { player });
            return new MonopolyCommand(player, players, material);
        }

        public MoveRobberCommand NewMoveRobberCommand(IPlayer player, int x, int y)
        {
            var robber = scope.Resolve<IRobber>();
            var board = scope.Resolve<IBoard>();
            var tile = board.FindTile(x, y);
            if (tile == null)
                throw new ArgumentException($"No tile found with coordinates (x {x}, y {y})");
            return new MoveRobberCommand(player, robber, tile);
        }

        public NextStateCommand NewNextState(IPlayer player)
        {
            return new NextStateCommand(player);
        }

        public OpenTradeCommand NewOpenTrade(IPlayer player, MaterialType[] offer, MaterialType[] request)
        {
            var trade = new Trade(offer, request, player);
            var market = scope.Resolve<IMarket>();
            return new OpenTradeCommand(player, market, trade);
        }

        public PlayDevelopmentCardCommand NewPlayDevelopmentCard(IPlayer player, Guid id)
        {
            var game = scope.Resolve<IGame>();
            var card = player.Cards.FirstOrDefault(c => c.Id == id);
            if (card == null)
                throw new ArgumentException("No card found with given id");
            return new PlayDevelopmentCardCommand(player, game, card);
        }

        public RollDiceCommand NewRollDice(IPlayer player)
        {
            var dice = scope.Resolve<IDice>();
            var provider = scope.Resolve<IRawmaterialProvider>();
            return new RollDiceCommand(player, dice, provider);
        }

        public StealResourceCommand NewStealResource(IPlayer player, Guid victimId)
        {
            var game = scope.Resolve<IGame>();
            var victim = game.FindPlayer(victimId);
            if (victim == null)
                throw new ArgumentException("No plaer found with given victim id");
            return new StealResourceCommand(player, victim);
        }

        public UpgradeEstablishmentCommand NewUpgradeEstablishment(IPlayer player, IVertex vertex)
        {
            var board = scope.Resolve<IBoard>();
            var establishment = board.Establishments.FirstOrDefault(e => e.Vertex == vertex);
            if (establishment == null)
                throw new ArgumentException("No establisment found on given vertex");
            if (establishment.Owner != player)
                throw new ArgumentException("Establishment that was found on vertex is not from the executing player");
            return new UpgradeEstablishmentCommand(player, establishment);
        }

        public YearOfPlentyCommand NewYearOfPlenty(IPlayer player, MaterialType material1, MaterialType material2)
        {
            return new YearOfPlentyCommand(player, material1, material2);
        }
    }
}

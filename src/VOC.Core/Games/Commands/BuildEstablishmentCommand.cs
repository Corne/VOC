using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Establishments;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class BuildEstablishmentCommand : IPlayerCommand
    {
        private readonly IBoard board;
        private readonly IVertex vertex;

        public BuildEstablishmentCommand(IPlayer player, IBoard board, IVertex vertex)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (board == null)
                throw new ArgumentNullException(nameof(board));
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            Player = player;
            this.board = board;
            this.vertex = vertex;
        }
        public IPlayer Player { get; }
        public GameCommand Type { get { return GameCommand.BuildEstablisment; } }

        public void Execute()
        {
            if (!Player.HasResources(Establishment.BUILD_RESOURCES))
                throw new InvalidOperationException("Can't build a house for this player, because there are not enough resources");

            board.BuildEstablishment(vertex, Player);
            Player.TakeResources(Establishment.BUILD_RESOURCES);
        }
    }
}

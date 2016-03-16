using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class BuildDevelopmentRoadCommand : IPlayerCommand
    {
        private readonly IBoard board;
        private readonly IEdge edge;

        public BuildDevelopmentRoadCommand(IPlayer player, IBoard board, IEdge edge)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (board == null)
                throw new ArgumentNullException(nameof(board));
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            Player = player;
            this.board = board;
            this.edge = edge;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.BuildRoad; } }

        public void Execute()
        {
            board.BuildRoad(edge, Player);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Games.Turns;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class MoveRobberCommand : IPlayerCommand
    {
        private readonly IRobber robber;
        private readonly ITile tile;
        public MoveRobberCommand(IPlayer player, IRobber robber, ITile tile)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (robber == null)
                throw new ArgumentNullException(nameof(robber));
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));
            Player = player;
            this.robber = robber;
            this.tile = tile;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.MoveRobber; } }

        public void Execute()
        {
            //CvB Todo: This seems kinda weird to have a whole class for this?
            robber.Move(tile);
        }
    }
}

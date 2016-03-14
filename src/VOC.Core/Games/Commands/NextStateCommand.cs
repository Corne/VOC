using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class NextStateCommand : IPlayerCommand
    {
        public NextStateCommand(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            Player = player;
        }

        public IPlayer Player { get; }
        public GameCommand Type { get { return GameCommand.NextState; } }

        public void Execute()
        {
            //Nothing??
        }
    }
}

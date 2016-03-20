using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class HighRollCommand : IPlayerCommand
    {
        private readonly IDice dice;
        public HighRollCommand(IPlayer player, IDice dice)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));

            Player = player;
            this.dice = dice;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.RollDice; } }

        public void Execute()
        {
            dice.Roll();
        }
    }
}

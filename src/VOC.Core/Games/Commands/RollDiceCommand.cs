using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Items;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class RollDiceCommand : IPlayerCommand
    {
        private readonly IRawmaterialProvider provider;
        public IDice Dice { get; }
        public IPlayer Player { get; }
        public GameCommand Type { get { return GameCommand.RollDice; } }

        public RollDiceCommand(IPlayer player, IDice dice, IRawmaterialProvider provider)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            Player = player;
            Dice = dice;
            this.provider = provider;
        }

        public void Execute()
        {
            Dice.Roll();

            int result = Dice.Current.Result;
            if (result != 7)
                provider.Distribute(result);
        }

    }
}

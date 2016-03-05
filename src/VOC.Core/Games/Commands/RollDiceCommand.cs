using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Games.Commands
{
    public class RollDiceCommand : IGameCommand
    {
        private readonly IRawmaterialProvider provider;
        public IDice Dice { get; }


        public RollDiceCommand(IDice dice, IRawmaterialProvider provider)
        {
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

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

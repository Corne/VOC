using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public class HighRollTurn : ITurn, IHighRollTurn
    {
        private readonly IDice dice;

        public HighRollTurn(IPlayer player, IDice dice)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));
            Player = player;
            this.dice = dice;
        }

        public IPlayer Player { get; }

        public event EventHandler Ended;

        public int Result { get; private set; }

        public bool CanExecute(GameCommand command)
        {
            return command == GameCommand.RollDice;
        }

        public void AfterExecute(GameCommand command)
        {
            if (!CanExecute(command)) return;

            Result = dice.Current.Result;
            Ended?.Invoke(this, EventArgs.Empty);
        }
    }
}

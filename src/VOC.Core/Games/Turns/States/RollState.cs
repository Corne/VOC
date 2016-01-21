using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns.States
{
    public class RollState : ITurnState
    {
        private readonly IDice dice;
        private readonly ITurn turn;

        public RollState(ITurn turn, IDice dice)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));

            this.turn = turn;
            this.dice = dice;
        }

        public IEnumerable<StateCommand> Commands
        {
            get { return new StateCommand[] { StateCommand.RollDice, StateCommand.PlayDevelopmentCard }; }
        }

        public void Start()
        {
            Stop();
            dice.Rolled += Dice_Rolled;
        }

        private void Dice_Rolled(object sender, DiceRoll roll)
        {
            Stop();

            if (roll.Result == 7)
                turn.SetState<RobberDiscardState>();
            else
                turn.NextFlowState();
        }

        public void Stop()
        {
            dice.Rolled -= Dice_Rolled;
        }
    }
}

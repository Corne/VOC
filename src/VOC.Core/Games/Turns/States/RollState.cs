using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns.States
{
    public class RollState : ITurnState, IFlowSate
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

        public bool Completed { get; private set; }

        public void AfterExecute(StateCommand command)
        {
            //todo DevelopmentCard?
            if (command != StateCommand.RollDice)
                return;

            Completed = true;
            if (dice.Current.Result == 7)
                turn.SetState<RobberDiscardState>();
            else
                turn.NextFlowState();
        }


    }
}

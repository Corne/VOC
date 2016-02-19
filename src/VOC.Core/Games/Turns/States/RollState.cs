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
        private readonly IGameTurn turn;

        public RollState(IGameTurn turn, IDice dice)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (dice == null)
                throw new ArgumentNullException(nameof(dice));

            this.turn = turn;
            this.dice = dice;
        }

        public IEnumerable<GameCommand> Commands
        {
            get { return new GameCommand[] { GameCommand.RollDice, GameCommand.PlayDevelopmentCard }; }
        }

        public bool Completed { get; private set; }

        public void AfterExecute(GameCommand command)
        {
            //todo DevelopmentCard?
            if (command != GameCommand.RollDice)
                return;

            Completed = true;
            if (dice.Current.Result == 7)
                turn.SetState<RobberDiscardState>();
            else
                turn.NextFlowState();
        }


    }
}

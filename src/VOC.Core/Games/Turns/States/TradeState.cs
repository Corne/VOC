using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class TradeState : ITurnState, IFlowSate
    {
        private readonly ITurn turn;

        public TradeState(ITurn turn)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            this.turn = turn;
        }

        public IEnumerable<StateCommand> Commands
        {
            get
            {
                return new StateCommand[] {
                    StateCommand.Trade,
                    StateCommand.PlayDevelopmentCard,
                    StateCommand.NextState
                };
            }
        }

        public bool Completed { get; private set; }

        public void AfterExecute(StateCommand command)
        {
            if (command == StateCommand.NextState)
            {
                Completed = true;
                turn.NextFlowState();
            }
        }
    }
}

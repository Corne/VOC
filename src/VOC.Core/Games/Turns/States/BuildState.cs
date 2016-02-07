using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class BuildState : ITurnState, IFlowSate
    {
        private readonly ITurn turn;

        public BuildState(ITurn turn)
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
                    StateCommand.BuildRoad,
                    StateCommand.BuildEstablisment,
                    StateCommand.UpdgradeEstablisment,
                    StateCommand.PlayDevelopmentCard,
                    StateCommand.NextState
                };
            }
        }

        public bool Completed { get; set; }

        public void AfterExecute(StateCommand command)
        {
            if (command == StateCommand.NextState)
            {
                turn.NextFlowState();
                Completed = true;
            }
        }

    }
}

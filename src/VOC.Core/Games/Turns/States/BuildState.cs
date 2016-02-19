using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class BuildState : ITurnState, IFlowSate
    {
        private readonly IGameTurn turn;

        public BuildState(IGameTurn turn)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            this.turn = turn;
        }

        public IEnumerable<GameCommand> Commands
        {
            get
            {
                return new GameCommand[] {
                    GameCommand.BuildRoad,
                    GameCommand.BuildEstablisment,
                    GameCommand.UpdgradeEstablisment,
                    GameCommand.PlayDevelopmentCard,
                    GameCommand.NextState
                };
            }
        }

        public bool Completed { get; set; }

        public void AfterExecute(GameCommand command)
        {
            if (command == GameCommand.NextState)
            {
                Completed = true;
                turn.NextFlowState();
            }
        }

    }
}

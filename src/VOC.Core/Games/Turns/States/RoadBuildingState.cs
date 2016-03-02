using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class RoadBuildingState : ITurnState
    {
        private readonly IGameTurn turn;
        private int buildCounter = 0;
        public RoadBuildingState(IGameTurn turn)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            this.turn = turn;
        }

        public IEnumerable<GameCommand> Commands
        {
            get { yield return GameCommand.BuildRoad; }
        }

        public void AfterExecute(GameCommand command)
        {
            if (command != GameCommand.BuildRoad)
                return;

            buildCounter++;

            if (buildCounter == 2)
                turn.NextFlowState();
        }
    }
}

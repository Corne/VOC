using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class YearOfPlentyState : ITurnState
    {
        private readonly IGameTurn turn;

        public YearOfPlentyState(IGameTurn turn)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            this.turn = turn;
        }

        public IEnumerable<GameCommand> Commands { get { yield return GameCommand.YearOfPlenty; } }

        public void AfterExecute(GameCommand command)
        {
            if (command == GameCommand.YearOfPlenty)
                turn.NextFlowState();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;

namespace VOC.Core.Games.Turns.States
{
    public class RobberStealState : ITurnState
    {
        private readonly ITurn turn;
        private readonly IBoard board;

        public RobberStealState(ITurn turn, IBoard board)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (board == null)
                throw new ArgumentNullException(nameof(board));
            if (board.Robber == null)
                throw new ArgumentNullException(nameof(board.Robber));
            this.turn = turn;

            //CvB todo maybe cleaner to get those values from a class that handles the actual stealing?
            var stealablePlayers = board.GetPlayers(board.Robber.CurrentTile);
            if (!stealablePlayers.Any() || stealablePlayers.All(p => p == turn.Player))
                turn.NextFlowState();
        }

        public IEnumerable<StateCommand> Commands
        {
            get
            {
                return new[] { StateCommand.StealResource };
            }
        }

        public void AfterExecute(StateCommand command)
        {
            if (command == StateCommand.StealResource)
                turn.NextFlowState();
        }
    }
}

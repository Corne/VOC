using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns.States
{
    public class MoveRobberState : ITurnState
    {
        private readonly IGameTurn turn;
        private readonly IRobber robber;

        public MoveRobberState(IGameTurn turn, IRobber robber)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (robber == null)
                throw new ArgumentNullException(nameof(robber));

            this.turn = turn;
            this.robber = robber;
        }

        public IEnumerable<GameCommand> Commands
        {
            get { return new GameCommand[] { GameCommand.MoveRobber }; }
        }

        public void AfterExecute(GameCommand command)
        {
            if (command == GameCommand.MoveRobber)
                turn.SetState<RobberStealState>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class TradeState : ITurnState, IFlowSate
    {
        private readonly IGameTurn turn;

        public TradeState(IGameTurn turn)
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
                    GameCommand.Trade,
                    GameCommand.PlayDevelopmentCard,
                    GameCommand.NextState
                };
            }
        }

        public bool Completed { get; private set; }

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

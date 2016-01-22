using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns.States
{
    public class RobberDiscardState : ITurnState
    {
        private readonly ITurn turn;
        private readonly IEnumerable<IPlayer> players;

        public RobberDiscardState(ITurn turn, IEnumerable<IPlayer> players)
        {
            this.turn = turn;
            this.players = players;
        }

        public IEnumerable<StateCommand> Commands
        {
            get
            {
                return new StateCommand[] { StateCommand.DiscardResources };
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}

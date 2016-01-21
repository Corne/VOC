using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class TradeState : ITurnState
    {
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

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}

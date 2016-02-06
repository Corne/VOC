using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class RobberStealState : ITurnState
    {
        public IEnumerable<StateCommand> Commands
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AfterExecute(StateCommand command)
        {
            throw new NotImplementedException();
        }
    }
}

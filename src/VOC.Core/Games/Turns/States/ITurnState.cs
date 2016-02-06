using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns.States
{
    public interface ITurnState
    {
        /// <summary>
        /// List of commands that can be executed during this state
        /// </summary>
        IEnumerable<StateCommand> Commands { get; }


        void AfterExecute(StateCommand command);

    }
}

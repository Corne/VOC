using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class BuildState : ITurnState, IFlowSate
    {
        public IEnumerable<StateCommand> Commands
        {
            get
            {
                return new StateCommand[] {
                    StateCommand.BuildRoad,
                    StateCommand.BuildEstablisment,
                    StateCommand.UpdgradeEstablisment,
                    StateCommand.PlayDevelopmentCard,
                    StateCommand.NextState //maybe cleaner to have a seperate end turn?
                };
            }
        }

        public bool Completed { get; set; }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}

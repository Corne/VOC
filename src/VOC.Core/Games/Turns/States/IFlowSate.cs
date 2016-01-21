using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public interface IFlowSate : ITurnState
    {
        bool Completed { get; }
    }
}

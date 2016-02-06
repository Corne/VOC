using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    /// <summary>
    /// List of commands user can execute during a turn
    /// Each state should specifiy which command can be executed during that state
    /// </summary>
    public enum StateCommand
    {
        RollDice,
        BuildRoad,
        BuildEstablisment,
        UpdgradeEstablisment,
        MoveRobber,
        PlayDevelopmentCard,
        Trade,
        NextState,
        DiscardResources,
        StealResource,
    }
}

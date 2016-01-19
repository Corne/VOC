using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns.States;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public interface ITurn
    {
        IPlayer Player { get; }
        ITurnState State { get; }

        bool DevelopmentCardPlayed { get; }
    }
}

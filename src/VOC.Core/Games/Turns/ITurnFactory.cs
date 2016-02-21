using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public interface ITurnFactory
    {
        T Create<T>(IPlayer player) where T : ITurn;
    }
}

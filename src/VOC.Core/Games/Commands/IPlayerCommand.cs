using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public interface IPlayerCommand
    {
        /// <summary>
        /// Player executing the command
        /// </summary>
        IPlayer Player { get; }
        GameCommand Type { get; }
        void Execute();
    }
}

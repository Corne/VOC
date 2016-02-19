using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public class TurnProvider : ITurnProvider
    {

        private readonly ISet<IPlayer> players;
        public TurnProvider(ISet<IPlayer> players)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));
            this.players = players;
        }

        public ITurn GetNext()
        {
            return null;
        }
    }
}

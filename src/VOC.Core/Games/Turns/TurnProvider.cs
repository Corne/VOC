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
        private readonly ITurnFactory factory;
        public TurnProvider(ISet<IPlayer> players, ITurnFactory factory)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));
            if (players.Count < 2)
                throw new ArgumentException("Turn provider expects at least 2 players");
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            this.players = players;
            this.factory = factory;
        }

        public ITurn GetNext()
        {
            return factory.Create<IHighRollTurn>(players.First());
        }
    }
}

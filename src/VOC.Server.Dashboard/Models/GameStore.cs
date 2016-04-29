using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Server.Dashboard.Models
{
    public class GameStore : IGameStore
    {
        private readonly Dictionary<Guid, Game> _games = new Dictionary<Guid, Game>();

        public IEnumerable<Game> Games { get { return _games.Values.ToList(); } }

        public void Add(Game game)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            if (_games.ContainsKey(game.Id))
                return;

            _games[game.Id] = game;

            //todo listen as observer to the game
        }

        public void Remove(Guid id)
        {
            _games.Remove(id);
        }
    }
}

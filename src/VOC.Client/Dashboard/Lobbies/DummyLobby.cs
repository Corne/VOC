using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Users;

namespace VOC.Client.Dashboard.Lobbies
{
    public class DummyLobby : ILobby
    {
        private readonly HashSet<Player> _players = new HashSet<Player>();

        public DummyLobby(Player moderator)
        {
            if (moderator == null)
                throw new ArgumentNullException(nameof(moderator));

            Moderator = moderator;
            _players.Add(moderator);
        }

        public Player Moderator { get; }
        public IEnumerable<Player> Players { get { return _players.ToList().AsReadOnly(); } }

        public event EventHandler<Player> PlayerJoined;
        public event EventHandler<Player> PlayerLeft;
    }
}

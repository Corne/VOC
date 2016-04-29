using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Lobbies
{
    public interface ILobby
    {
        Player Moderator { get; }
        IEnumerable<Player> Players { get; }

        event EventHandler<Player> PlayerJoined;
        event EventHandler<Player> PlayerLeft;
    }
}

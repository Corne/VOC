using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Dashboard.Lobbies;
using VOC.Client.WPF.Main;

namespace VOC.Client.WPF.Dashboard.Lobbies
{
    public class LobbyViewModel : IContentViewModel
    {
        private readonly ILobby lobby;

        public LobbyViewModel(ILobby lobby)
        {
            if (lobby == null)
                throw new ArgumentNullException(nameof(lobby));

            this.lobby = lobby;

            foreach (var player in lobby.Players)
                Players.Add(player);

            lobby.PlayerJoined += Lobby_PlayerJoined;
            lobby.PlayerLeft += Lobby_PlayerLeft;
        }

        public ObservableCollection<Player> Players { get; } = new ObservableCollection<Player>();

        private void Lobby_PlayerLeft(object sender, Player p)
        {
            Players.Remove(p);
        }

        private void Lobby_PlayerJoined(object sender, Player p)
        {
            Players.Add(p);
        }


        public Task OnClose()
        {
            return Task.FromResult(0);
        }

        public Task OnNavigate()
        {
            return Task.FromResult(0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Dashboard.Lobbies;
using VOC.Client.WPF.Main;

namespace VOC.Client.WPF.Dashboard.Lobbies
{
    public class LobbyViewModel : IContentViewModel
    {
        private readonly Lobby lobby;

        public LobbyViewModel(Lobby lobby)
        {
            this.lobby = lobby;
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

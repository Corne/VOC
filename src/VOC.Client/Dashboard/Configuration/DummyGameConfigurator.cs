using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Dashboard.Lobbies;

namespace VOC.Client.Dashboard.Configuration
{
    public class DummyGameConfigurator : IGameConfigurator
    {
        public Task<ILobby> CreateLobby(GameConfiguration configuration, int port)
        {
            return Task.FromResult((ILobby)null);
        }
    }
}

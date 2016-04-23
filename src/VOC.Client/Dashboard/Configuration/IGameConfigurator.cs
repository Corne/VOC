using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Dashboard.Lobbies;

namespace VOC.Client.Dashboard.Configuration
{
    public interface IGameConfigurator
    {
        Task<ILobby> CreateLobby(GameConfiguration configuration, int port);
    }
}

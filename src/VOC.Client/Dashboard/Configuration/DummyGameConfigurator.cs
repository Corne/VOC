using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Dashboard.Lobbies;
using VOC.Client.Users;

namespace VOC.Client.Dashboard.Configuration
{
    public class DummyGameConfigurator : IGameConfigurator
    {
        private readonly IUser user;

        public DummyGameConfigurator(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            this.user = user;
        }

        public Task<ILobby> CreateLobby(GameConfiguration configuration, int port)
        {
            var moderator = new Player(user.Id, user.Name);
            ILobby lobby = new DummyLobby(moderator);
            return Task.FromResult(lobby);
        }
    }
}

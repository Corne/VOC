using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Dashboard.Lobbies;
using VOC.Client.Users;
using static VOC.Client.Dashboard.Games.GameStore;

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

        public async Task<ILobby> CreateLobby(GameConfiguration configuration, int port)
        {
            using (var client = new HttpClient())
            {
                var data = new GameData() { Id = Guid.NewGuid(), IP = "127.0.0.1", Port = 1337, Name = "Hello World Game" };
                var response = await client.PostAsync(URL, data, new JsonMediaTypeFormatter());
            }

            var moderator = new Player(user.Id, user.Name);
            ILobby lobby = new DummyLobby(moderator);
            return lobby;
        }
    }
}

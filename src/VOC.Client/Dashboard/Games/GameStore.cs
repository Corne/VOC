using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VOC.Client.Dashboard.Games
{
    public class GameStore : IGameStore
    {
        public static readonly string URL = "http://localhost:54318/api/games";

        public IEnumerable<IGame> Games { get; private set; }

        public async Task<bool> Load()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(URL);
                if (response.IsSuccessStatusCode)
                {
                    var games = await response.Content.ReadAsAsync<IEnumerable<GameData>>();
                    Games = games.Select(g => new Game(g.Name, new ConnectionInfo(IPAddress.Parse(g.IP), g.Port))).ToList();
                }
                return response.IsSuccessStatusCode;
            }
        }

        public async Task Create(Game game)
        {
            using (var client = new HttpClient())
            {
                var data = new GameData() { Id = Guid.NewGuid(), IP = "127.0.0.1", Port = 1337, Name = "Hello World Game" };
                var response = await client.PostAsync(URL, data, new JsonMediaTypeFormatter());
            }
        }

        //temp solution, maybe cleaner to have a shared library?
        public class GameData
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public string IP { get; set; }
            public int Port { get; set; }

            public int CurrentPlayers { get; set; }
            public int MaxPlayes { get; set; }
        }
    }
}

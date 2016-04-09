using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Data.Games
{
    public class DummyGame : IGame
    {
        public DummyGame()
        {
            Name = $"Game: {Guid.NewGuid()}";
            ConnectionInfo = new ConnectionInfo(new IPAddress(new byte[] { 127, 0, 0, 1 }), 1337);
        }

        public ConnectionInfo ConnectionInfo { get; }
        public string Name { get; }
    }
}

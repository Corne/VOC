using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Data.Games;

namespace VOC.Client.WPF.Dashboard
{
    public class GameViewModel
    {
        private readonly IGame game;

        public GameViewModel(IGame game)
        {
            this.game = game;
        }

        public string Description { get { return game.Name; } }

        public string Connection
        {
            get
            {
                string ip = game.ConnectionInfo.IPAddress.ToString();
                int port = game.ConnectionInfo.Port;
                return $"{ip}:{port}";
            }
        }
    }
}

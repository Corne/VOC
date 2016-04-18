using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Configuration
{
    public class GameConfiguration
    {
        public GameConfiguration(IMap map, int players)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (players < map.MinPlayers || players > map.MaxPlayers)
                throw new ArgumentException("Number of players should be between map min and max");
            Map = map;
            TotalPlayers = players;
        }

        public int TotalPlayers { get; }
        public IMap Map { get; }
    }
}

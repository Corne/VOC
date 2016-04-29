using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VOC.Server.Dashboard.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string IP { get; set; }
        public int Port { get; set; }

        public int CurrentPlayers { get; set; }
        public int MaxPlayes { get; set; }
    }
}

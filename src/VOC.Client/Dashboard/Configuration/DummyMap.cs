using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Configuration
{
    public class DummyMap : IMap
    {
        public DummyMap(string name)
        {
            Name = name;
        }

        public int MaxPlayers { get { return 4; } }
        public int MinPlayers { get { return 3; } }

        public string Name { get; }
    }
}

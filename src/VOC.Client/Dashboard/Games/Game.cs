using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Games
{
    public class Game : IGame
    {

        public Game(string name, ConnectionInfo connectionIno)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (connectionIno == null)
                throw new ArgumentNullException(nameof(connectionIno));

            Name = name;
            ConnectionInfo = connectionIno;
        }

        public ConnectionInfo ConnectionInfo { get; }
        public string Name { get; }
    }
}

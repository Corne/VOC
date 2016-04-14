using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Games
{
    public interface IGame
    {
        string Name { get; }

        ConnectionInfo ConnectionInfo { get; }
    }
}

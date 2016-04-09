using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Data.Games
{
    public interface IGameStore
    {
        Task Load();

        IEnumerable<IGame> Games { get; }
    }
}

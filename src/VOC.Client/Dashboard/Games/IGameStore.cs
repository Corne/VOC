using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Games
{
    public interface IGameStore
    {
        /// <summary>
        /// Load all games
        /// </summary>
        /// <returns></returns>
        Task<bool> Load();

        IEnumerable<IGame> Games { get; }
    }
}

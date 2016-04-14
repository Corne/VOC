using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Games
{
    public class DummyGameStore : IGameStore
    {
        private static readonly Random rand = new Random();

        public IEnumerable<IGame> Games { get; private set; }

        public Task Load()
        {
            Games = Enumerable.Range(0, rand.Next(20)).Select(i => new DummyGame()).ToList().AsReadOnly();
            return Task.FromResult(0);
        }
    }
}

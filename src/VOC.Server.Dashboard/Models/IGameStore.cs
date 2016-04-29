using System;
using System.Collections.Generic;

namespace VOC.Server.Dashboard.Models
{
    public interface IGameStore
    {
        IEnumerable<Game> Games { get; }

        void Add(Game game);
        void Remove(Guid id);
    }
}
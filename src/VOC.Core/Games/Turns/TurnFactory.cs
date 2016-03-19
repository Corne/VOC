using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public class TurnFactory : ITurnFactory
    {
        private readonly ILifetimeScope scope;
        public TurnFactory(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public T Create<T>(IPlayer player) where T : ITurn
        {
            return scope.Resolve<T>(new TypedParameter(typeof(IPlayer), player));
        }
    }
}

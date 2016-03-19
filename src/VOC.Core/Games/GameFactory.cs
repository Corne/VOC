using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using VOC.Core.Games.Turns;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games
{
    public class GameFactory : IDisposable
    {
        private readonly ContainerBuilder builder;
        private readonly IContainer container;

        private readonly ISet<ILifetimeScope> childscopes = new HashSet<ILifetimeScope>();

        public GameFactory()
        {
            builder = new ContainerBuilder();

            builder.Register((c, p) =>
            {
                var players = p.TypedAs<ISet<IPlayer>>();
                return new Game(players, c.Resolve<ITurnProvider>(p));
            }).As<IGame>();
            builder.RegisterType<TurnProvider>().As<ITurnProvider>();
            builder.RegisterType<TurnFactory>().As<ITurnFactory>();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Turn"))/*t.IsSubclassOf(typeof(ITurn))*/
                .AsSelf().AsImplementedInterfaces();

            builder.RegisterType<Dice>().UsingConstructor()
                .As<IDice>().InstancePerLifetimeScope();

            container = builder.Build();
        }

        public IGame Create(ISet<IPlayer> players)
        {
            //CVB TODO: SHOULD CLEAN UP GAMES EARLIER
            var scope = container.BeginLifetimeScope();
            childscopes.Add(scope);

            return scope.Resolve<IGame>(new TypedParameter(typeof(ISet<IPlayer>), players));
        }

        public void Dispose()
        {

            foreach(var scope in childscopes)
            {
                scope.Dispose();
            }
            container.Dispose();
        }

    }
}

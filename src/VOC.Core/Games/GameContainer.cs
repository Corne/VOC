using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using VOC.Core.Boards;
using VOC.Core.Games.Commands;
using VOC.Core.Games.Turns;
using VOC.Core.Items;
using VOC.Core.Players;
using VOC.Core.Trading;

namespace VOC.Core.Games
{
    public class GameContainer : IDisposable
    {
        private readonly ContainerBuilder builder;
        private readonly IContainer container;

        private readonly IDictionary<IGame, ILifetimeScope> childscopes = new Dictionary<IGame, ILifetimeScope>();

        public GameContainer()
        {
            builder = new ContainerBuilder();

            builder.Register((c, p) =>
            {
                var players = p.TypedAs<ISet<IPlayer>>();
                return new Game(players, c.Resolve<ITurnProvider>(p), c.Resolve<IBank>());
            }).As<IGame>().InstancePerLifetimeScope();

            builder.RegisterType<TurnProvider>().As<ITurnProvider>();
            builder.RegisterType<TurnFactory>().As<ITurnFactory>();
            builder.RegisterType<CommandFactory>();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Turn"))/*t.IsSubclassOf(typeof(ITurn))*/
                .AsSelf().AsImplementedInterfaces();

            builder.RegisterType<Board>()
                .As<IBoard>().InstancePerLifetimeScope();

            builder.RegisterType<DefaultBoardBuilder>().As<IBoardBuilder>();

            builder.RegisterType<Dice>().UsingConstructor()
                .As<IDice>().InstancePerLifetimeScope();

            builder.RegisterType<Bank>()
                .As<IBank>().InstancePerLifetimeScope();


            container = builder.Build();
        }

        public IGame Create(ISet<IPlayer> players)
        {
            var scope = container.BeginLifetimeScope();
            var game = scope.Resolve<IGame>(new TypedParameter(typeof(ISet<IPlayer>), players));
            childscopes[game] = scope;

            return game;
        }

        public CommandFactory GetCommandFactory(IGame game)
        {
            if (!childscopes.ContainsKey(game))
                throw new ArgumentException("Game not found");
            return childscopes[game].Resolve<CommandFactory>();
        }

        public void Cleanup(IGame game)
        {
            if (childscopes.ContainsKey(game))
            {
                var scope = childscopes[game];
                childscopes.Remove(game);
                scope.Dispose();
            }
        }

        public void Dispose()
        {
            while (childscopes.Any())
            {
                Cleanup(childscopes.First().Key);
            }
            container.Dispose();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns
{
    public class TurnProviderTest
    {
        [Fact]
        public void TurnProviderCantBeCreatedWithoutPlayers()
        {
            var factory = new Mock<ITurnFactory>();
            Assert.Throws<ArgumentNullException>(() => new TurnProvider(null, factory.Object));
        }


        private ISet<IPlayer> CreatePlayers(int count)
        {
            var players = Enumerable.Range(0, count).Select(i => new Mock<IPlayer>().Object);
            return new HashSet<IPlayer>(players);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ExpectExceptionIfPlayerCountBelow2(int count)
        {
            var factory = new Mock<ITurnFactory>();
            var players = CreatePlayers(count);
            Assert.Throws<ArgumentException>(() => new TurnProvider(players, factory.Object));
        }

        [Fact]
        public void TurnProviderCantBeCreatedWithoutFactory()
        {
            var players = CreatePlayers(2);
            Assert.Throws<ArgumentNullException>(() => new TurnProvider(players, null));
        }

        private Mock<ITurnFactory> CreateFactory()
        {
            var factory = new Mock<ITurnFactory>();
            factory.Setup(f => f.Create<IHighRollTurn>(It.IsAny<IPlayer>()))
                .Returns<IPlayer>(p =>
                {
                    var turn = new Mock<IHighRollTurn>();
                    turn.Setup(t => t.Player).Returns(p);
                    turn.Setup(t => t.Result).Returns(5);
                    return turn.Object;
                });
            factory.Setup(f => f.Create<IBuildTurn>(It.IsAny<IPlayer>()))
                .Returns<IPlayer>(p =>
                {
                    var turn = new Mock<IBuildTurn>();
                    turn.Setup(t => t.Player).Returns(p);
                    return turn.Object;
                });
            factory.Setup(f => f.Create<IGameTurn>(It.IsAny<IPlayer>()))
                .Returns<IPlayer>(p =>
                {
                    var turn = new Mock<IGameTurn>();
                    turn.Setup(t => t.Player).Returns(p);
                    return turn.Object;
                });
            return factory;
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void GetNextReturnsHighRollTurnForEachPlayerOnce(int count)
        {
            var players = CreatePlayers(count);
            var factory = CreateFactory();
            var provider = new TurnProvider(players, factory.Object);

            for (int i = 0; i < count; i++)
            {
                var turn = provider.GetNext();
                Assert.IsAssignableFrom<IHighRollTurn>(turn);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void ExpectForEachPlayer2BuildTurnsAfterHighRollTurn(int count)
        {
            var players = CreatePlayers(count);
            var factory = CreateFactory();
            var provider = new TurnProvider(players, factory.Object);

            for (int i = 0; i < count; i++)
            {
                provider.GetNext();
            }

            for(int i=0; i<count*2; i++)
            {
                var turn = provider.GetNext();
                Assert.IsAssignableFrom<IBuildTurn>(turn);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        public void ExpectEveryTurnAfterBuildTurnToBeGameTurn(int count)
        {
            var players = CreatePlayers(count);
            var factory = CreateFactory();
            var provider = new TurnProvider(players, factory.Object);

            for (int i = 0; i < count; i++)
            {
                provider.GetNext();
            }

            for (int i = 0; i < count * 2; i++)
            {
                provider.GetNext();
            }

            for(int i =0; i < count*count; i++)
            {
                var turn = provider.GetNext();
                Assert.IsAssignableFrom<IGameTurn>(turn);
            }
        }
    }
}

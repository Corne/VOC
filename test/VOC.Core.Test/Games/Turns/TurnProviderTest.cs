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

        private Mock<ITurnFactory> CreateFactory(IPlayer player)
        {
            var factory = new Mock<ITurnFactory>();
            factory.Setup(f => f.Create<IHighRollTurn>(player))
                .Returns<IPlayer>(p =>
                {
                    var turn = new Mock<IHighRollTurn>();
                    turn.Setup(t => t.Player).Returns(p);
                    return turn.Object;
                });
            return factory;
        }

        [Fact]
        public void GetNextReturnsTurnWithHighRollProvider()
        {
            var players = CreatePlayers(2);
            var factory = CreateFactory(players.First());
            var provider = new TurnProvider(players, factory.Object);

            var turn = provider.GetNext();
            Assert.IsAssignableFrom<IHighRollTurn>(turn);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games;
using VOC.Core.Games.Commands;
using VOC.Core.Players;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class BuyDevelopmentCardCommandTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get {
                yield return new object[] { null, new Mock<IGame>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantConstructWithNullParameter(IPlayer player, IGame game)
        {
            Assert.Throws<ArgumentNullException>(() => new BuyDevelopmentCardCommand(player, game));
        }

        [Fact]
        public void BuyDevelopmentCardCallsGame()
        {
            var game = new Mock<IGame>();
            var player = new Mock<IPlayer>();
            var command = new BuyDevelopmentCardCommand(player.Object, game.Object);
            command.Execute();

            game.Verify(g => g.BuyDevelopmentCard(player.Object));
        }
    }
}

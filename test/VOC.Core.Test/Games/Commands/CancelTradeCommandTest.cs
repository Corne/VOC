using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Commands;
using VOC.Core.Players;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class CancelTradeCommandTest
    {
        public static IEnumerable<object> InvalidConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<ITrade>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }

        [Theory, MemberData(nameof(InvalidConstruction))]
        public void ConstructionFailsWithNullParameter(IPlayer player, ITrade trade)
        {
            Assert.Throws<ArgumentNullException>(() => new CancelTradeCommand(player, trade));
        }

        [Fact]
        public void OnlyOwnerCanCancelTrade()
        {
            var owner = new Mock<IPlayer>();
            var player = new Mock<IPlayer>();
            var trade = new Mock<ITrade>();
            trade.Setup(t => t.Owner).Returns(owner.Object);

            Assert.Throws<ArgumentException>(() => new CancelTradeCommand(player.Object, trade.Object));
        }

        [Fact]
        public void ExecuteCancelsTrade()
        {
            var player = new Mock<IPlayer>();
            var trade = new Mock<ITrade>();
            trade.Setup(t => t.Owner).Returns(player.Object);

            var command = new CancelTradeCommand(player.Object, trade.Object);
            command.Execute();

            trade.Verify(t => t.Cancel());
        }
    }
}

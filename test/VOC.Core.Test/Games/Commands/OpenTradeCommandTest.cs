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
    public class OpenTradeCommandTest
    {
        public static IEnumerable<object> InvalidConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<IMarket>().Object, new Mock<ITrade>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null, new Mock<ITrade>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, new Mock<IMarket>().Object, null };
            }
        }

        [Theory, MemberData(nameof(InvalidConstruction))]
        public void ExpectConstructionIfFailOnNullParameter(IPlayer player, IMarket market, ITrade trade)
        {
            Assert.Throws<ArgumentNullException>(() => new OpenTradeCommand(player, market, trade));
        }

        [Fact]
        public void ExecuteOpensTradeOnMarkter()
        {
            var player = new Mock<IPlayer>();
            var market = new Mock<IMarket>();
            var trade = new Mock<ITrade>();

            var command = new OpenTradeCommand(player.Object, market.Object, trade.Object);
            command.Execute();

            market.Verify(m => m.OpenTrade(trade.Object));
        }
    }
}

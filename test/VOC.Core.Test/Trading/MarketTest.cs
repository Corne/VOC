using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Trading
{
    public class MarketTest
    {
        [Fact]
        public void CantOpentNullTrade()
        {
            var market = new Market();
            Assert.Throws<ArgumentNullException>(() => market.OpenTrade(null));
        }

        [Theory]
        [InlineData(TradeState.Canceled)]
        [InlineData(TradeState.Processed)]
        public void CantOpenTradeThatIsNotOpen(TradeState state)
        {
            var market = new Market();
            var trade = new Mock<ITrade>();
            trade.Setup(t => t.State).Returns(state);

            Assert.Throws<ArgumentException>(() => market.OpenTrade(trade.Object));
        }

        [Fact]
        public void OpenTradeAddsToRequestList()
        {
            var market = new Market();
            var trade = new Mock<ITrade>();
            trade.Setup(t => t.State).Returns(TradeState.Open);

            market.OpenTrade(trade.Object);
            Assert.Contains(trade.Object, market.ActiveTrades);
        }

        [Fact]
        public void OpentSameTradeTwiceGivesNoEffect()
        {
            var market = new Market();
            var trade = new Mock<ITrade>();
            trade.Setup(t => t.State).Returns(TradeState.Open);

            market.OpenTrade(trade.Object);
            market.OpenTrade(trade.Object);

            Assert.Contains(trade.Object, market.ActiveTrades);
            Assert.True(market.ActiveTrades.Count(r => r == trade.Object) == 1);
        }

        [Fact]
        public void ActiveTradesIsEmptyListIfNoTrades()
        {
            var market = new Market();
            Assert.Equal(new ITrade[] { }, market.ActiveTrades);
        }

        [Fact]
        public void ActiveTradesOnlyShowesOpenTrades()
        {
            var market = new Market();
            var trade1 = new Mock<ITrade>();
            trade1.Setup(t => t.State).Returns(TradeState.Open);
            var trade2 = new Mock<ITrade>();
            trade2.Setup(t => t.State).Returns(TradeState.Open);
            var trade3 = new Mock<ITrade>();
            trade3.Setup(t => t.State).Returns(TradeState.Open);

            market.OpenTrade(trade1.Object);
            market.OpenTrade(trade2.Object);
            market.OpenTrade(trade3.Object);

            trade2.Setup(t => t.State).Returns(TradeState.Canceled);
            trade3.Setup(t => t.State).Returns(TradeState.Processed);

            Assert.Equal(new ITrade[] { trade1.Object }, market.ActiveTrades);
        }
    }
}

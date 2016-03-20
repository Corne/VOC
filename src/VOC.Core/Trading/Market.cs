using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace VOC.Core.Trading
{
    public class Market : IMarket
    {
        private static ILog logger = LogManager.GetLogger(nameof(Market));

        private readonly HashSet<ITrade> trades = new HashSet<ITrade>();

        public IEnumerable<ITrade> ActiveTrades
        {
            get { return trades.Where(t => t.State == TradeState.Open).ToList(); }
        }

        public ITrade Find(Guid guid)
        {
            return trades.SingleOrDefault(t => t.Id == guid);
        }

        public void OpenTrade(ITrade trade)
        {
            if (trade == null)
                throw new ArgumentNullException(nameof(trade));
            if (trade.State != TradeState.Open)
                throw new ArgumentException("Can't open trade on the market, if the trade is not in an open state");
            trades.Add(trade);

            logger.Info($"Trade opened");
        }
    }
}

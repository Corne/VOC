using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Trading
{
    public interface IMarket
    {
        ITrade Find(Guid guid);
        IEnumerable<ITrade> ActiveTrades { get; }
        void OpenTrade(ITrade trade);
    }
}

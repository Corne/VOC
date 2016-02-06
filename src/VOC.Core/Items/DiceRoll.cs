using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items
{
    public class DiceRoll
    {
        public DiceRoll(IEnumerable<int> dieValues)
        {
            Result = dieValues.Sum();
            DieValues = dieValues;
        }
        public int Result { get; }
        public IEnumerable<int> DieValues { get; }
    }
}

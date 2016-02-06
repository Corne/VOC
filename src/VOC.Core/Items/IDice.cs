using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items
{
    public interface IDice
    {
        void Roll();
        DiceRoll Current { get; }
    }
}

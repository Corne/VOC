using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items
{
    public class Die : IDie
    {
        private readonly Random random;
        public Die(Random random)
        {
            this.random = random;
        }

        public int Throw()
        {
            return random.Next(1, 7);
        }
    }
}

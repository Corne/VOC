using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items;
using Xunit;

namespace VOC.Core.Test.Items
{
    public class DieTest
    {
        [Fact]
        public void ThrowTest()
        {
            var die = new Die();
            int result = die.Throw();

            Assert.True(result >= 1 && result <= 6, $"Expected value between 1 and 6, actual: {result}");
        }
    }
}

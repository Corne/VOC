using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Commands;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class NextStateCommandTest
    {
        [Fact]
        public void CantBeCreatedWithoutPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => new NextStateCommand(null));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Commands;
using VOC.Core.Items;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class HighRollCommandTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<IDice>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }
        
        [Theory, MemberData(nameof(NullConstruction))]
        public void CantConstructWithNull(IPlayer player, IDice dice)
        {
            Assert.Throws<ArgumentNullException>(() => new HighRollCommand(player, dice));
        }

        [Fact]
        public void ExecuteRollsDice()
        {
            var player = new Mock<IPlayer>();
            var dice = new Mock<IDice>();
            var command = new HighRollCommand(player.Object, dice.Object);

            command.Execute();
            dice.Verify(d => d.Roll());
        }
    }
}

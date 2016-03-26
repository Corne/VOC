using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Commands;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class BuyResourceCommandTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<IBank>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantConstructWithNullParameter(IPlayer player, IBank bank)
        {
            Assert.Throws<ArgumentNullException>(() => new BuyResourceCommand(player, bank, MaterialType.Grain, MaterialType.Wool));
        }

        [Fact]
        public void ExpectExceptionIfOfferAndRequestAreOfSameType()
        {
            var player = new Mock<IPlayer>();
            var bank = new Mock<IBank>();
            Assert.Throws<ArgumentException>(() => new BuyResourceCommand(player.Object, bank.Object, MaterialType.Wool, MaterialType.Wool));
        }

        [Fact]
        public void ExecuteCallsBank()
        {
            var player = new Mock<IPlayer>();
            var bank = new Mock<IBank>();
            var command = new BuyResourceCommand(player.Object, bank.Object, MaterialType.Wool, MaterialType.Lumber);
            command.Execute();

            bank.Verify(b => b.BuyResource(MaterialType.Wool, MaterialType.Lumber, player.Object));
        }
    }
}

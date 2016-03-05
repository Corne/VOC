using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Commands;
using VOC.Core.Items;
using VOC.Core.Items.RawMaterials;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class RollDiceCommandTest
    {
        [Fact]
        public void RollDiceCommandCantBeCreatedWithoutDice()
        {
            var provider = new Mock<IRawmaterialProvider>();
            Assert.Throws<ArgumentNullException>(() => new RollDiceCommand(null, provider.Object));
        }

        [Fact]
        public void RollDiceCommandCantBeCreatedWithoutProvider()
        {
            var dice = new Mock<IDice>();
            Assert.Throws<ArgumentNullException>(() => new RollDiceCommand(dice.Object, null));
        }


        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(8)]
        [InlineData(12)]
        public void ExecuteProvidesMaterialsIfResultNot7(int value)
        {
            var dice = new Mock<IDice>();
            var provider = new Mock<IRawmaterialProvider>();
            dice.Setup(d => d.Current).Returns(new DiceRoll(new int[] { value }));

            var command = new RollDiceCommand(dice.Object, provider.Object);
            command.Execute();

            dice.Verify(d => d.Roll());
            provider.Verify(p => p.Distribute(value), Times.Once);
        }

        [Fact]
        public void ExpectNoDistributeIfDiceResult7()
        {
            var dice = new Mock<IDice>();
            var provider = new Mock<IRawmaterialProvider>();
            dice.Setup(d => d.Current).Returns(new DiceRoll(new int[] { 7 }));

            var command = new RollDiceCommand(dice.Object, provider.Object);
            command.Execute();

            provider.Verify(p => p.Distribute(It.IsAny<int>()), Times.Never);
        }
    }
}

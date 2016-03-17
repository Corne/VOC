using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Commands;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class YearOfPlentyCommandTest
    {
        [Fact]
        public void CantConstructWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => new YearOfPlentyCommand(null, MaterialType.Brick, MaterialType.Grain));
        }

        public static IEnumerable<object> InvalidMaterials
        {
            get
            {
                yield return new object[] { (MaterialType)(-1) };
                yield return new object[] { MaterialType.Sea };
                yield return new object[] { MaterialType.Unsourced };
            }
        }

        [Theory, MemberData(nameof(InvalidMaterials))]
        public void Material1ShouldBeValid(MaterialType material)
        {
            var player = new Mock<IPlayer>();
            Assert.Throws<ArgumentException>(() => new YearOfPlentyCommand(player.Object, material, MaterialType.Grain));
        }

        [Theory, MemberData(nameof(InvalidMaterials))]
        public void Material2ShouldBeValid(MaterialType material)
        {
            var player = new Mock<IPlayer>();
            Assert.Throws<ArgumentException>(() => new YearOfPlentyCommand(player.Object, MaterialType.Grain, material));
        }

        [Fact]
        public void ExpectMaterialsToBeAddedToPlayer()
        {
            var player = new Player("Bob");
            var command = new YearOfPlentyCommand(player, MaterialType.Grain, MaterialType.Wool);
            command.Execute();

            Assert.Equal(new MaterialType[] { MaterialType.Grain, MaterialType.Wool }, player.Inventory.Select(i => i.Type));
        }
    }
}

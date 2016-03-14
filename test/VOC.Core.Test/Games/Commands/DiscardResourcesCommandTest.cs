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
    public class DiscardResourcesCommandTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new MaterialType[] { MaterialType.Brick, MaterialType.Grain } };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantConstructWithNull(IPlayer player, MaterialType[] materials)
        {
            Assert.Throws<ArgumentNullException>(() => new DiscardResourcesCommand(player, materials));
        }


        [Fact]
        public void ExpectExceptionIfPlayerMoreThen7Resources()
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.Inventory).Returns(Enumerable.Range(0, 6).Select(i => new Mock<IRawMaterial>().Object));
            var materials = new MaterialType[] { MaterialType.Grain, MaterialType.Lumber };

            Assert.Throws<InvalidOperationException>(() => new DiscardResourcesCommand(player.Object, materials));
        }

        [Theory]
        [InlineData(7, 3)]
        [InlineData(8, 4)]
        [InlineData(9, 4)]
        [InlineData(10, 5)]
        [InlineData(11, 5)]
        public void ExpectMaterialCountToBeHalfOfPlayerInventory(int inventoryCount, int materialCount)
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.Inventory).Returns(Enumerable.Range(0, inventoryCount).Select(i => new Mock<IRawMaterial>().Object));
            var materials = Enumerable.Range(0, materialCount).Select(i => MaterialType.Grain);

            Assert.NotNull(new DiscardResourcesCommand(player.Object, materials));
        }

        [Theory]
        [InlineData(7, 2)]
        [InlineData(7, 4)]
        [InlineData(8, 3)]
        [InlineData(8, 5)]
        [InlineData(9, 3)]
        [InlineData(9, 5)]
        public void ExpectFailedConstructionIfMaterialCountNotHalfOfPlayerInventory(int inventoryCount, int materialCount)
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.Inventory).Returns(Enumerable.Range(0, inventoryCount).Select(i => new Mock<IRawMaterial>().Object));
            var materials = Enumerable.Range(0, materialCount).Select(i => MaterialType.Grain);

            Assert.Throws<ArgumentException>(() => new DiscardResourcesCommand(player.Object, materials));
        }

        [Fact]
        public void ExecuteTakesResourcesFromPlayer()
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.Inventory).Returns(Enumerable.Range(0, 7).Select(i => new Mock<IRawMaterial>().Object));
            var materials = new MaterialType[] { MaterialType.Grain, MaterialType.Lumber, MaterialType.Lumber };

            var command = new DiscardResourcesCommand(player.Object, materials);
            command.Execute();

            player.Verify(p => p.TakeResources(materials));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Items;
using VOC.Core.Items.RawMaterials;
using Xunit;

namespace VOC.Core.Test.Items
{
    public class RobberTest
    {
        [Fact]
        public void ConstructionFailsNullTile()
        {
            Assert.Throws<ArgumentNullException>(() => new Robber(null));
        }

        [Theory]
        [InlineData(MaterialType.Brick)]
        [InlineData(MaterialType.Grain)]
        [InlineData(MaterialType.Lumber)]
        [InlineData(MaterialType.Ore)]
        [InlineData(MaterialType.Sea)]
        [InlineData(MaterialType.Wool)]
        public void InitialTileShouldBeDesert(MaterialType type)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.Rawmaterial).Returns(type);

            Assert.Throws<ArgumentException>(() => new Robber(tile.Object));
        }

        [Fact]
        public void TestRobberConstruction()
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.Rawmaterial).Returns(MaterialType.Unsourced);

            var robber = new Robber(tile.Object);
            Assert.Equal(tile.Object, robber.CurrentTile);
        }

        [Fact]
        public void MoveRobberNullTileFails()
        {
            var initial = new Mock<ITile>();
            initial.Setup(t => t.Rawmaterial).Returns(MaterialType.Unsourced);

            var robber = new Robber(initial.Object);
            Assert.Throws<ArgumentNullException>(() => robber.Move(null));
        }

        [Fact]
        public void CantMoveRobberToTheSea()
        {
            var initial = new Mock<ITile>();
            initial.Setup(t => t.Rawmaterial).Returns(MaterialType.Unsourced);

            var input = new Mock<ITile>();
            input.Setup(t => t.Rawmaterial).Returns(MaterialType.Sea);

            var robber = new Robber(initial.Object);
            Assert.Throws<ArgumentException>(() => robber.Move(input.Object));
        }

        [Theory]
        [InlineData(MaterialType.Brick)]
        [InlineData(MaterialType.Grain)]
        [InlineData(MaterialType.Lumber)]
        [InlineData(MaterialType.Ore)]
        [InlineData(MaterialType.Unsourced)]
        [InlineData(MaterialType.Wool)]
        public void MoveTest(MaterialType type)
        {
            var initial = new Mock<ITile>();
            initial.Setup(t => t.Rawmaterial).Returns(MaterialType.Unsourced);

            var input = new Mock<ITile>();
            input.Setup(t => t.Rawmaterial).Returns(type);

            var robber = new Robber(initial.Object);

            robber.Move(input.Object);

            Assert.Equal(input.Object, robber.CurrentTile);
        }


    }
}

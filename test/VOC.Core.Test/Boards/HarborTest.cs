using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;
using Xunit;

namespace VOC.Core.Test.Boards
{
    public class HarborTest
    {
        [Fact]
        public void HarborCantBeConstructedWithoutEdge()
        {
            var tile = new Mock<ITile>();
            Assert.Throws<ArgumentNullException>(() => new Harbor(MaterialType.Brick, null, tile.Object));
        }

        [Fact]
        public void HarborCantBeConstructedWithoutTile()
        {
            var edge = new Mock<IEdge>();
            Assert.Throws<ArgumentNullException>(() => new Harbor(MaterialType.Brick, edge.Object, null));
        }

        [Fact]
        public void HarborCantBeConstructedWithSeaMaterial()
        {
            var tile = new Mock<ITile>();
            var edge = new Mock<IEdge>();
            tile.Setup(t => t.Rawmaterial).Returns(MaterialType.Sea);
            tile.Setup(t => t.IsAdjacentTo(edge.Object)).Returns(true);
            edge.Setup(e => e.IsAdjacentTo(tile.Object)).Returns(true);
            Assert.Throws<ArgumentException>(() => new Harbor(MaterialType.Sea, edge.Object, tile.Object));
        }

        [Fact]
        public void HarborCantBeConstructedWithUnkownMaterial()
        {
            var tile = new Mock<ITile>();
            var edge = new Mock<IEdge>();
            tile.Setup(t => t.Rawmaterial).Returns(MaterialType.Sea);
            tile.Setup(t => t.IsAdjacentTo(edge.Object)).Returns(true);
            edge.Setup(e => e.IsAdjacentTo(tile.Object)).Returns(true);
            MaterialType material = (MaterialType)100;
            Assert.Throws<ArgumentException>(() => new Harbor(material, edge.Object, tile.Object));
        }

        [Theory]
        [InlineData(MaterialType.Brick)]
        [InlineData(MaterialType.Grain)]
        [InlineData(MaterialType.Lumber)]
        [InlineData(MaterialType.Ore)]
        [InlineData(MaterialType.Unsourced)]
        [InlineData(MaterialType.Wool)]
        public void HarborTileShouldBeSeaTile(MaterialType material)
        {
            var tile = new Mock<ITile>();
            var edge = new Mock<IEdge>();
            tile.Setup(t => t.Rawmaterial).Returns(material);
            tile.Setup(t => t.IsAdjacentTo(edge.Object)).Returns(true);
            edge.Setup(e => e.IsAdjacentTo(tile.Object)).Returns(true);

            Assert.Throws<ArgumentException>(() => new Harbor(MaterialType.Ore, edge.Object, tile.Object));
        }

        [Fact]
        public void HarborEdgeAndTileShouldBeAdjacent()
        {
            var tile = new Mock<ITile>();
            var edge = new Mock<IEdge>();
            tile.Setup(t => t.Rawmaterial).Returns(MaterialType.Sea);
            //1 of those should be called, because when 1 is true the other should also be true
            tile.Setup(t => t.IsAdjacentTo(edge.Object)).Returns(false);
            edge.Setup(e => e.IsAdjacentTo(tile.Object)).Returns(false);

            Assert.Throws<ArgumentException>(() => new Harbor(MaterialType.Ore, edge.Object, tile.Object));
        }


        [Fact]
        public void HarborConstructionTest()
        {
            var tile = new Mock<ITile>();
            var edge = new Mock<IEdge>();
            tile.Setup(t => t.Rawmaterial).Returns(MaterialType.Sea);
            tile.Setup(t => t.IsAdjacentTo(edge.Object)).Returns(true);
            edge.Setup(e => e.IsAdjacentTo(tile.Object)).Returns(true);

            var harbor = new Harbor(MaterialType.Ore, edge.Object, tile.Object);

            Assert.Equal(MaterialType.Ore, harbor.Discount);
            Assert.Equal(edge.Object, harbor.Edge);
            Assert.Equal(tile.Object, harbor.Tile);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Establishments;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Establishments
{
    public class EstablismentTest
    {
        [Fact]
        public void ConstructionSetsLevelToSettlement()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            var establisment = new Establishment(player.Object, vertex.Object);

            Assert.Equal(EstablishmentLevel.Settlement, establisment.Level);
        }

        [Fact]
        public void UpgradeFailsIfPlayerDoesntHaveResources()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            player.Setup(p => p.HasResources(Establishment.UPGRADE_RESOURCES)).Returns(false);
            var establisment = new Establishment(player.Object, vertex.Object);

            Assert.Throws<InvalidOperationException>(() => establisment.Upgrade());
            Assert.Equal(EstablishmentLevel.Settlement, establisment.Level);
            player.Verify(p => p.TakeResources(Establishment.UPGRADE_RESOURCES), Times.Never);
        }
        
        [Fact]
        public void UpgradeSetsLevelToCity()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            player.Setup(p => p.HasResources(Establishment.UPGRADE_RESOURCES)).Returns(true);
            var establisment = new Establishment(player.Object, vertex.Object);

            establisment.Upgrade();

            Assert.Equal(EstablishmentLevel.City, establisment.Level);
        }

        [Fact]
        public void UpgradeRemovedResourcesFromOwner()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            player.Setup(p => p.HasResources(Establishment.UPGRADE_RESOURCES)).Returns(true);
            var establisment = new Establishment(player.Object, vertex.Object);

            establisment.Upgrade();
            Assert.Equal(EstablishmentLevel.City, establisment.Level);
            player.Verify(p => p.TakeResources(Establishment.UPGRADE_RESOURCES), Times.Once);
        }

        [Fact]
        public void UpdgradeFailsIfLevelAlreadyCity()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            player.Setup(p => p.HasResources(Establishment.UPGRADE_RESOURCES)).Returns(true);

            var establisment = new Establishment(player.Object, vertex.Object);

            establisment.Upgrade();
            Assert.Throws<InvalidOperationException>(() => establisment.Upgrade());
            player.Verify(p => p.TakeResources(Establishment.UPGRADE_RESOURCES), Times.Once);

        }

        [Fact]
        public void HarvestValidatesThatEstbalismentAdjacentsTile()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            var tile = new Mock<ITile>();

            vertex.Setup(v => v.IsAdjacentTo(tile.Object)).Returns(true);
            var establisment = new Establishment(player.Object, vertex.Object);

            establisment.Harvest(tile.Object);

            vertex.Verify(v => v.IsAdjacentTo(tile.Object));
        }

        [Fact]
        public void HarvestThrowsExceptionIfTileIsNotAdjacent()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            var tile = new Mock<ITile>();

            vertex.Setup(v => v.IsAdjacentTo(tile.Object)).Returns(false);
            var establisment = new Establishment(player.Object, vertex.Object);

            Assert.Throws<ArgumentException>(() => establisment.Harvest(tile.Object));
        }

        /// <summary>
        /// Tile creates the material
        /// </summary>
        [Fact]
        public void EstablismentShouldFarmTile()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            var tileMock = new Mock<ITile>();
            tileMock.Setup(t => t.Rawmaterial).Returns(MaterialType.Grain);

            var tile = tileMock.Object;
            vertex.Setup(v => v.IsAdjacentTo(tile)).Returns(true);

            var establisment = new Establishment(player.Object, vertex.Object);
            establisment.Harvest(tile);

            tileMock.Verify(t => t.Farm());
        }

        [Fact]
        public void HarvestAddsXResourcesToPlayer()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();
            var tileMock = new Mock<ITile>();
            tileMock.Setup(t => t.Rawmaterial).Returns(MaterialType.Grain);
            tileMock.Setup(t => t.Farm()).Returns(new Mock<IRawMaterial>().Object);

            var tile = tileMock.Object;
            vertex.Setup(v => v.IsAdjacentTo(tile)).Returns(true);

            var establisment = new Establishment(player.Object, vertex.Object);

            establisment.Harvest(tile);
            player.Verify(p => p.AddResources(It.IsAny<IRawMaterial>()), Times.Once());
        }

        [Fact]
        public void HarvestUpgradedEstablismentAdds2ResourcesToPlayer()
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.UPGRADE_RESOURCES)).Returns(true);

            var vertex = new Mock<IVertex>();
            var tileMock = new Mock<ITile>();
            tileMock.Setup(t => t.Rawmaterial).Returns(MaterialType.Grain);
            tileMock.Setup(t => t.Farm()).Returns(new Mock<IRawMaterial>().Object);

            var tile = tileMock.Object;
            vertex.Setup(v => v.IsAdjacentTo(tile)).Returns(true);

            var establisment = new Establishment(player.Object, vertex.Object);
            establisment.Upgrade();
            establisment.Harvest(tile);

            player.Verify(p => p.AddResources(It.IsAny<IRawMaterial>()), Times.Exactly(2));
        }
    }
}

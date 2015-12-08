using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Establishments;
using VOC.Core.Games;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games
{
    //www.spelregels-online.nl/j-k-l/de-kolonisten-van-catan/de-kolonisten-van-catan
    public class RawMaterialProviderTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        [InlineData(7)]
        public void DistributeArgumentTest(int value)
        {
            var board = new Mock<IBoard>();
            var provider = new RawMaterialProvider(board.Object);

            Assert.Throws<ArgumentException>(() => provider.Distrubte(value));
        }

        [Fact]
        public void DistributeRetrievesTilesWithSameValue()
        {
            var board = new Mock<IBoard>();
            var provider = new RawMaterialProvider(board.Object);

            provider.Distrubte(3);

            board.Verify(b => b.GetTiles(3));
        }

        //Get farms/cities on given tiles
        [Fact]
        public void DistrubteRetrievesEstablishmentForEachTile()
        {
            ITile[] tiles = {
                new Mock<ITile>().Object,
                new Mock<ITile>().Object,
                new Mock<ITile>().Object,
            };

            var board = new Mock<IBoard>();
            board.Setup(b => b.GetTiles(It.IsAny<int>())).Returns(tiles.Select(t => t));

            var provider = new RawMaterialProvider(board.Object);
            provider.Distrubte(3);

            foreach (ITile tile in tiles)
            {
                board.Verify(b => b.GetEstablishments(tile));
            }
        }

        [Fact]
        public void DistrubteNotifiesEachEstablismentToHarvestTile()
        {
            ITile[] tiles = {
                new Mock<ITile>().Object,
                new Mock<ITile>().Object,
                new Mock<ITile>().Object,
            };

            // 1 establisment can be bound to multiple tiles
            // the provider doesn't care if the board does not allow to have adjacent tiles with the same number
            var establisment1 = new Mock<IEstablishment>();
            var establisment2 = new Mock<IEstablishment>();

            var board = new Mock<IBoard>();
            board.Setup(b => b.GetTiles(It.IsAny<int>())).Returns(tiles.Select(t => t));
            board.Setup(b => b.GetEstablishments(tiles[0])).Returns(new[] { establisment2.Object, establisment1.Object });
            board.Setup(b => b.GetEstablishments(tiles[1])).Returns(new IEstablishment[] { });
            board.Setup(b => b.GetEstablishments(tiles[2])).Returns(new[] { establisment1.Object });

            var provider = new RawMaterialProvider(board.Object);
            provider.Distrubte(3);

            establisment1.Verify(e => e.Harvest(tiles[0]));
            establisment1.Verify(e => e.Harvest(tiles[2]));
            establisment2.Verify(e => e.Harvest(tiles[0]));

            establisment1.Verify(e => e.Harvest(It.IsAny<ITile>()), Times.Exactly(2));
            establisment2.Verify(e => e.Harvest(It.IsAny<ITile>()), Times.Once);
        }
    }
}

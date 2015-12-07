using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Games;
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

        //Get players with farms/cities on given tiles
    }
}

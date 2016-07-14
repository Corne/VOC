using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.WPF.Game.Board;
using Xunit;

namespace VOC.Client.WPF.Test.Game.Board
{
    public class TileViewModelTest
    {
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-5, -5)]
        public void XandYShouldBePositive(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => new TileViewModel(x, y));
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(10,0)]
        [InlineData(0,10)]
        [InlineData(5,6)]
        public void TestConstruction(int x, int y)
        {
            var viewmodel = new TileViewModel(x, y);
            Assert.Equal(x, viewmodel.X);
            Assert.Equal(y, viewmodel.Y);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.WPF.Game.Board;
using Xunit;

namespace VOC.Client.WPF.Test.Game.Board
{
    public class BoardViewModelTest
    {
        public static IEnumerable<object[]> NullInput
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { new TileViewModel[] { null } };
                yield return new object[] { new TileViewModel[] { new TileViewModel(1, 3), new TileViewModel(5, 2), null, new TileViewModel(6, 7) } };
            }
        }

        [Theory, MemberData(nameof(NullInput))]
        public void CantBeConstructedWithNull(TileViewModel[] tiles)
        {
            Assert.Throws<ArgumentNullException>(() => new BoardViewModel(tiles));
        }

        public static IEnumerable<object[]> TotalTestInputs
        {
            get
            {
                yield return new object[] { new int[0], 1 };
                yield return new object[] { new[] { 1 }, 2 };
                yield return new object[] { new[] { 1, 5 }, 6 };
                yield return new object[] { new[] { 1, 5, 34, 25 }, 35 };
            }
        }

        [Theory, MemberData(nameof(TotalTestInputs))]
        public void XTotalTest(IEnumerable<int> xValues, int expected)
        {
            var tiles = xValues.Select(x => new TileViewModel(x, 1));
            var viewmodel = new BoardViewModel(tiles);
            Assert.Equal(expected, viewmodel.TotalX);
        }

        [Theory, MemberData(nameof(TotalTestInputs))]
        public void YTotalTest(IEnumerable<int> yValues, int expected)
        {
            var tiles = yValues.Select(y => new TileViewModel(1, y));
            var viewmodel = new BoardViewModel(tiles);
            Assert.Equal(expected, viewmodel.TotalY);
        }
    }
}

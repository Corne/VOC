using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;
using Xunit;

namespace VOC.Core.Test.Boards
{
    public class TileTest
    {
        [Theory]
        [InlineData(MaterialType.Brick)]
        [InlineData(MaterialType.Grain)]
        [InlineData(MaterialType.Lumber)]
        [InlineData(MaterialType.Ore)]
        [InlineData(MaterialType.Wool)]
        [InlineData(MaterialType.Unsourced)] //NOT SURE about this one
        public void FarmTest(MaterialType type)
        {
            var tile = new Tile(1, 2, 3, type);
            IRawMaterial material = tile.Farm();

            Assert.Equal(type, material.Type);
        }
    }
}

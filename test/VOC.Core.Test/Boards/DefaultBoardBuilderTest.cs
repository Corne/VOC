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
    public class DefaultBoardBuilderTest
    {


        [Fact]
        public void DefaultBoardHas19Tiles()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial != MaterialType.Sea).ToArray();
            Assert.Equal(19, tiles.Length);
        }

        [Fact]
        public void DefaultBoardHas1Desert()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial == MaterialType.Unsourced).ToArray();
            Assert.Equal(1, tiles.Length);
        }

        [Fact]
        public void DefaultBoardHas3BrickTiles()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial == MaterialType.Brick).ToArray();
            Assert.Equal(3, tiles.Length);
        }

        [Fact]
        public void DefaultBoardHas4WoolTiles()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial == MaterialType.Wool).ToArray();
            Assert.Equal(4, tiles.Length);
        }

        [Fact]
        public void DefaultBoardHas3OreTiles()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial == MaterialType.Ore).ToArray();
            Assert.Equal(3, tiles.Length);
        }

        [Fact]
        public void DefaultBoardHas4GrainTiles()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial == MaterialType.Grain).ToArray();
            Assert.Equal(4, tiles.Length);
        }

        [Fact]
        public void DefaultBoardHas4LumberTiles()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial == MaterialType.Lumber).ToArray();
            Assert.Equal(4, tiles.Length);
        }

        /// <summary>
        /// By non rotated image: each row(5) * 2 + top(4) + bottem(4)
        /// Which will provide all shared edges/vertices for border tiles
        /// </summary>
        [Fact]
        public void DefaultBoardShouldHave18SeaTiles()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            var tiles = builder.Tiles.Where(t => t.Rawmaterial == MaterialType.Sea).ToArray();
            Assert.Equal(18, tiles.Length);
        }

        [Fact]
        public void DefaultBoardHasCorrectNumbers()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();
            //array would be cleaner...
            Assert.Equal(1, builder.Tiles.Count(t => t.Number == 2));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 3));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 4));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 5));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 6));
            Assert.Equal(1, builder.Tiles.Count(t => t.Number == 7));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 8));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 9));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 10));
            Assert.Equal(2, builder.Tiles.Count(t => t.Number == 11));
            Assert.Equal(1, builder.Tiles.Count(t => t.Number == 12));
        }

        /// <summary>
        /// Edges are shared for each tile, so each coordinate should have 3
        /// </summary>
        [Fact]
        public void TestEachTileHas3Edges()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            foreach(var tile in builder.Tiles)
            {
                var edges = builder.Edges.Where(edge => edge.X == tile.X && edge.Y == tile.Y).ToArray();

                Assert.Equal(3, edges.Length);
                Assert.Equal(new[] { EdgeSide.East, EdgeSide.North, EdgeSide.West }, edges.Select(e => e.Side).ToArray());
            }
        }

        /// <summary>
        /// Each tile should have a vertex left and right, the top and bottem vertices are shared from other tiles
        /// </summary>
        [Fact]
        public void TestEachTileHas2Vertices()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            foreach (var tile in builder.Tiles)
            {
                var vertices = builder.Vertices.Where(vertice => vertice.X == tile.X && vertice.Y == tile.Y).ToArray();

                Assert.Equal(2, vertices.Length);
                Assert.Equal(new[] { VertexTileSide.Left, VertexTileSide.Right }, vertices.Select(e => e.Side).ToArray());
            }
        }

        [Fact]
        public void DefaultBoardHas9Harbors()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            Assert.Equal(9, builder.Harbors.Count());
            Assert.Equal(4, builder.Harbors.Count(h => h.Discount == MaterialType.Unsourced));

            foreach(var value in Enum.GetValues(typeof(MaterialType)).OfType<MaterialType>()
                .Except(new MaterialType[] { MaterialType.Unsourced, MaterialType.Sea }))
            {
                Assert.Equal(1, builder.Harbors.Count(h => h.Discount == value));
            }
        }

        [Fact]
        public void EachHarborHasUniqueTile()
        {
            var builder = new DefaultBoardBuilder();
            builder.Build();

            foreach (var harbor in builder.Harbors)
            {
                Assert.Equal(1, builder.Harbors.Count(h => h.Tile == harbor.Tile));
            }
        }
    }
}

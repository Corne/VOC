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


        [Fact]
        public void NotAdjacentToNullEdge()
        {
            var tile = new Tile(4, 4, 4, MaterialType.Grain);
            bool result = tile.IsAdjacentTo((IEdge)null);
            Assert.False(result);
        }

        [Fact]
        public void NotAdjacentToNullTile()
        {
            var tile = new Tile(4, 4, 4, MaterialType.Grain);
            bool result = tile.IsAdjacentTo((ITile)null);
            Assert.False(result);
        }

        [Fact]
        public void NotAdjacentToNullVertex()
        {
            var tile = new Tile(4, 4, 4, MaterialType.Grain);
            bool result = tile.IsAdjacentTo((IVertex)null);
            Assert.False(result);
        }

        /// <summary>
        /// We expect tile to be adjacent to North Edge with same X and Y, and X and Y-1
        /// </summary>
        [Theory]
        [InlineData(0, 0, 0, 0, true)]
        [InlineData(0, 0, 0, -1, true)]
        [InlineData(0, 0, 0, 1, false)]
        [InlineData(0, 0, 0, -2, false)]
        [InlineData(0, 0, 1, 0, false)]
        [InlineData(0, 0, -1, 0, false)]

        [InlineData(7, 4, 7, 4, true)]
        [InlineData(7, 4, 7, 3, true)]
        [InlineData(7, 4, 7, 5, false)]
        [InlineData(7, 4, 7, 2, false)]
        [InlineData(7, 4, 8, 4, false)]
        [InlineData(7, 4, 6, 4, false)]
        [InlineData(7, 4, 8, 5, false)]
        [InlineData(7, 4, 6, 3, false)]
        public void IsAdjacentToNorthEdges(int x, int y, int edgeX, int edgeY, bool expected)
        {
            var edge = new Mock<IEdge>();
            edge.Setup(e => e.X).Returns(edgeX);
            edge.Setup(e => e.Y).Returns(edgeY);
            edge.Setup(e => e.Side).Returns(EdgeSide.North);

            var tile = new Tile(x, y, 5, MaterialType.Grain);

            bool result = tile.IsAdjacentTo(edge.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nTile(X: {x}, Y: {y}, Right); Edge(X: {edgeX}, Y: {edgeY}");
        }

        /// <summary>
        /// We expect tile to be adjacent to North Edge with same X and Y, and X+1 and Y-1
        /// </summary>
        [Theory]
        [InlineData(0, 0, 0, 0, true)]
        [InlineData(0, 0, 1, -1, true)]
        [InlineData(0, 0, 1, 0, false)]
        [InlineData(0, 0, 0, 1, false)]
        [InlineData(0, 0, 1, 1, false)]
        [InlineData(0, 0, -1, 1, false)]
        [InlineData(0, 0, -1, 0, false)]

        [InlineData(6, 9, 6, 9, true)]
        [InlineData(6, 9, 7, 8, true)]
        [InlineData(6, 9, 7, 9, false)]
        [InlineData(6, 9, 6, 8, false)]
        [InlineData(6, 9, 6, 10, false)]
        [InlineData(6, 9, 8, 7, false)]
        [InlineData(6, 9, 5, 9, false)]
        [InlineData(6, 9, 9, 6, false)]

        public void IsAdjacentToWestEdges(int x, int y, int edgeX, int edgeY, bool expected)
        {
            var edge = new Mock<IEdge>();
            edge.Setup(e => e.X).Returns(edgeX);
            edge.Setup(e => e.Y).Returns(edgeY);
            edge.Setup(e => e.Side).Returns(EdgeSide.West);

            var tile = new Tile(x, y, 5, MaterialType.Grain);

            bool result = tile.IsAdjacentTo(edge.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nTile(X: {x}, Y: {y}, Right); Edge(X: {edgeX}, Y: {edgeY}");
        }

        /// <summary>
        /// We expect tile to be adjacent to North Edge with same X and Y, and X-1 and Y
        /// </summary>
        [Theory]
        [InlineData(0, 0, 0, 0, true)]
        [InlineData(0, 0, -1, 0, true)]
        [InlineData(0, 0, 1, 0, false)]
        [InlineData(0, 0, 0, 1, false)]
        [InlineData(0, 0, -1, -1, false)]
        [InlineData(0, 0, -2, 0, false)]

        [InlineData(4, 8, 4, 8, true)]
        [InlineData(4, 8, 3, 8, true)]
        [InlineData(4, 8, 2, 8, false)]
        [InlineData(4, 8, 5, 8, false)]
        [InlineData(4, 8, 4, 9, false)]
        [InlineData(4, 8, 3, 9, false)]
        public void IsAdjacentToEastEdges(int x, int y, int edgeX, int edgeY, bool expected)
        {
            var edge = new Mock<IEdge>();
            edge.Setup(e => e.X).Returns(edgeX);
            edge.Setup(e => e.Y).Returns(edgeY);
            edge.Setup(e => e.Side).Returns(EdgeSide.East);

            var tile = new Tile(x, y, 5, MaterialType.Grain);

            bool result = tile.IsAdjacentTo(edge.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nTile(X: {x}, Y: {y}, Right); Edge(X: {edgeX}, Y: {edgeY}");
        }

        /// <summary>
        /// (x,y+1) (x+1,y) (x+1,y-1) (x,y-1) (x-1,y) (x-1,y+1)
        /// </summary>
        [Theory]
        [InlineData(0, 0, 0, 1, true)]
        [InlineData(0, 0, 1, 0, true)]
        [InlineData(0, 0, 1, -1, true)]
        [InlineData(0, 0, 0, -1, true)]
        [InlineData(0, 0, -1, 0, true)]
        [InlineData(0, 0, -1, 1, true)]
        [InlineData(0, 0, 0, 0, false)]//not adjacent to self?
        [InlineData(0, 0, 1, 1, false)]
        [InlineData(0, 0, -1, -1, false)]
        [InlineData(0, 0, 2, 0, false)]
        [InlineData(0, 0, 0, 2, false)]
        [InlineData(0, 0, -2, 0, false)]
        [InlineData(0, 0, 0, -2, false)]

        [InlineData(5, 13, 5, 14, true)]
        [InlineData(5, 13, 6, 13, true)]
        [InlineData(5, 13, 6, 12, true)]
        [InlineData(5, 13, 5, 12, true)]
        [InlineData(5, 13, 4, 13, true)]
        [InlineData(5, 13, 4, 14, true)]
        [InlineData(5, 13, 5, 13, false)]//not adjacent to self?
        [InlineData(5, 13, 6, 14, false)]
        [InlineData(5, 13, 4, 12, false)]
        [InlineData(5, 13, 7, 13, false)]
        [InlineData(5, 13, 3, 13, false)]
        [InlineData(5, 13, 5, 15, false)]
        [InlineData(5, 13, 5, 11, false)]
        //test our algorithm, that we don't a valid adjacent value, because diff <= 1
        [InlineData(10, 0, 5, 5, false)]
        [InlineData(10, -10, 0, 0, false)]
        public void IsAdjacentToTiles(int x, int y, int inputX, int inputY, bool expected)
        {
            var input = new Mock<ITile>();
            input.Setup(i => i.X).Returns(inputX);
            input.Setup(i => i.Y).Returns(inputY);

            var tile = new Tile(x, y, 5, MaterialType.Ore);
            bool result = tile.IsAdjacentTo(input.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nTile(X: {x}, Y: {y}, Right); Tile(X: {inputX}, Y: {inputY}");
        }

        [Theory]
        [InlineData(0, 0, 0, 0, VertexTileSide.Left, true)]
        [InlineData(0, 0, 0, 0, VertexTileSide.Right, true)]
        [InlineData(0, 0, 1, 1, VertexTileSide.Left, false)]
        [InlineData(0, 0, 1, 1, VertexTileSide.Right, false)]

        [InlineData(3, 5, 3,5, VertexTileSide.Left, true)]
        [InlineData(3, 5, 3,5, VertexTileSide.Right, true)]
        [InlineData(3, 5, 2,4, VertexTileSide.Left, false)]
        [InlineData(3, 5, 2,4, VertexTileSide.Right, false)]
        [InlineData(3, 5, 4,6, VertexTileSide.Left, false)]
        [InlineData(3, 5, 4,6, VertexTileSide.Right, false)]
        public void IsAdjacentToVertexOwnTile(int x, int y, int vertexX, int vertexY, VertexTileSide side, bool expected)
        {
            var vertex = new Mock<IVertex>();
            vertex.Setup(i => i.X).Returns(vertexX);
            vertex.Setup(i => i.Y).Returns(vertexY);
            vertex.Setup(v => v.Side).Returns(side);

            var tile = new Tile(x, y, 5, MaterialType.Ore);
            bool result = tile.IsAdjacentTo(vertex.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nTile(X: {x}, Y: {y}, Right); Vertex(X: {vertexX}, Y: {vertexY}, side: {side})");
        }

        //Top-Left:(x-1,y+1,R) Top-Right(x+1,y,L) 
        [Theory]
        [InlineData(0, 0, -1, 1, VertexTileSide.Right, true)]
        [InlineData(0, 0, 1, 0, VertexTileSide.Left, true)]
        [InlineData(0, 0, -2, 1, VertexTileSide.Right, false)]
        [InlineData(0, 0, -1, 2, VertexTileSide.Right, false)]
        [InlineData(0, 0, 2, 0, VertexTileSide.Left, false)]

        [InlineData(6, 7, 5, 8, VertexTileSide.Right, true)]
        [InlineData(6, 7, 7, 7, VertexTileSide.Left, true)]
        [InlineData(6, 7, 4, 8, VertexTileSide.Right, false)]
        [InlineData(6, 7, 5, 9, VertexTileSide.Right, false)]
        [InlineData(6, 7, 8, 7, VertexTileSide.Left, false)]
        public void IsAdjacentToTopVertex(int x, int y, int vertexX, int vertexY, VertexTileSide side, bool expected)
        {
            var vertex = new Mock<IVertex>();
            vertex.Setup(i => i.X).Returns(vertexX);
            vertex.Setup(i => i.Y).Returns(vertexY);
            vertex.Setup(v => v.Side).Returns(side);

            var tile = new Tile(x, y, 5, MaterialType.Ore);
            bool result = tile.IsAdjacentTo(vertex.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nTile(X: {x}, Y: {y}, Right); Vertex(X: {vertexX}, Y: {vertexY}, Side: {side})");
        }

        //Bottom-Left:(x-1, y, R) Bottom-Right:(x+1, y-1, L)
        [Theory()]
        [InlineData(0, 0, -1, 0, VertexTileSide.Right, true)]
        [InlineData(0, 0, 1,-1, VertexTileSide.Left, true)]
        [InlineData(0, 0, -2, 0, VertexTileSide.Right, false)]
        [InlineData(0, 0, 1,-2, VertexTileSide.Left, false)]
        [InlineData(0, 0, 2, -1, VertexTileSide.Left, false)]

        [InlineData(6, 7, 5, 7, VertexTileSide.Right, true)]
        [InlineData(6, 7, 7, 6, VertexTileSide.Left, true)]
        [InlineData(6, 7, 4, 7, VertexTileSide.Right, false)]
        [InlineData(6, 7, 7, 5, VertexTileSide.Left, false)]
        [InlineData(6, 7, 8, 6, VertexTileSide.Left, false)]
        public void IsAdjacentToBottomVertex(int x, int y, int vertexX, int vertexY, VertexTileSide side, bool expected)
        {
            var vertex = new Mock<IVertex>();
            vertex.Setup(i => i.X).Returns(vertexX);
            vertex.Setup(i => i.Y).Returns(vertexY);
            vertex.Setup(v => v.Side).Returns(side);

            var tile = new Tile(x, y, 5, MaterialType.Ore);
            bool result = tile.IsAdjacentTo(vertex.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nTile(X: {x}, Y: {y}, Right); Vertex(X: {vertexX}, Y: {vertexY}");
        }
    }
}

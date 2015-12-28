using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using Xunit;

namespace VOC.Core.Test.Boards
{
    public class EdgeTest
    {
        [Fact]
        public void NotAdjacentToNullEdge()
        {
            var edge = new Edge(4, 4, EdgeSide.East);
            bool result = edge.IsAdjacentTo((IEdge)null);
            Assert.False(result);
        }

        [Fact]
        public void NotAdjacentToNullTile()
        {
            var edge = new Edge(4, 4, EdgeSide.East);
            bool result = edge.IsAdjacentTo((ITile)null);
            Assert.False(result);
        }

        [Fact]
        public void NotAdjacentToNullVertex()
        {
            var edge = new Edge(4, 4, EdgeSide.East);
            bool result = edge.IsAdjacentTo((IVertex)null);
            Assert.False(result);
        }

        [Theory]
        [InlineData(0, 0, EdgeSide.East, 0, 0, true)]
        [InlineData(0, 0, EdgeSide.North, 0, 0, true)]
        [InlineData(0, 0, EdgeSide.West, 0, 0, true)]

        [InlineData(5, 9, EdgeSide.East, 5, 9, true)]
        [InlineData(5, 9, EdgeSide.North, 5, 9, true)]
        [InlineData(5, 9, EdgeSide.West, 5, 9, true)]

        [InlineData(5, 9, EdgeSide.East, 4, 8, false)]
        [InlineData(5, 9, EdgeSide.North, 4, 8, false)]
        [InlineData(5, 9, EdgeSide.West, 4, 8, false)]
        [InlineData(5, 9, EdgeSide.East, 6, 10, false)]
        [InlineData(5, 9, EdgeSide.North, 6, 10, false)]
        [InlineData(5, 9, EdgeSide.West, 6, 10, false)]
        public void TileWithSameXYIsAdjacent(int edgeX, int edgeY, EdgeSide side, int tileX, int tileY, bool expected)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(tileX);
            tile.Setup(t => t.Y).Returns(tileY);

            var edge = new Edge(edgeX, edgeY, side);
            bool result = edge.IsAdjacentTo(tile.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, {side}); Tile(X: {tileX}, Y: {tileY}");
        }

        //expect tile with x-1 and y+1 to be true
        [Theory]
        [InlineData(0, 0, -1, 1, true)]
        [InlineData(4, 8, 3, 9, true)]
        [InlineData(-5, -10, -6, -9, true)]

        [InlineData(0, 0, 0, 1, false)]
        [InlineData(0, 0, 1, 0, false)]

        [InlineData(4, 8, 4, 9, false)]
        [InlineData(4, 8, 5, 8, false)]
        public void TestTileAdjacentCaseWest(int edgeX, int edgeY, int tileX, int tileY, bool expected)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(tileX);
            tile.Setup(t => t.Y).Returns(tileY);

            var edge = new Edge(edgeX, edgeY, EdgeSide.West);
            bool result = edge.IsAdjacentTo(tile.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, West); Tile(X: {tileX}, Y: {tileY}");
        }

        [Theory]
        [InlineData(0, 0, 0, 1, true)]
        [InlineData(4, 8, 4, 9, true)]
        [InlineData(-5, -10, -5, -9, true)]

        [InlineData(0, 0, -1, 1, false)]
        [InlineData(0, 0, 1, 0, false)]

        [InlineData(4, 8, 3, 9, false)]
        [InlineData(4, 8, 5, 8, false)]
        public void TestTileAdjacentCaseNorth(int edgeX, int edgeY, int tileX, int tileY, bool expected)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(tileX);
            tile.Setup(t => t.Y).Returns(tileY);

            var edge = new Edge(edgeX, edgeY, EdgeSide.North);
            bool result = edge.IsAdjacentTo(tile.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, Right); Tile(X: {tileX}, Y: {tileY}");
        }

        [Theory]
        [InlineData(0, 0, 1, 0, true)]
        [InlineData(4, 8, 5, 8, true)]
        [InlineData(-5, -10, -4, -10, true)]

        [InlineData(0, 0, -1, 1, false)]
        [InlineData(0, 0, 0, 1, false)]

        [InlineData(4, 8, 3, 9, false)]
        [InlineData(4, 8, 4, 9, false)]
        public void TestTileAdjacentCaseEast(int edgeX, int edgeY, int tileX, int tileY, bool expected)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(tileX);
            tile.Setup(t => t.Y).Returns(tileY);

            var edge = new Edge(edgeX, edgeY, EdgeSide.East);
            bool result = edge.IsAdjacentTo(tile.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, Right); Tile(X: {tileX}, Y: {tileY}");
        }

        // (x-1, y, E) (x-1, y, N) (x-1, y+1, E) (x, y, N)
        [Theory]
        [InlineData(0, 0, -1, 0, EdgeSide.East, true)]
        [InlineData(0, 0, -1, 0, EdgeSide.North, true)]
        [InlineData(0, 0, -1, 1, EdgeSide.East, true)]
        [InlineData(0, 0, 0, 0, EdgeSide.North, true)]

        [InlineData(0, 0, 0, 0, EdgeSide.East, false)]
        [InlineData(0, 0, 0, 0, EdgeSide.West, false)]
        [InlineData(0, 0, -1, 0, EdgeSide.West, false)]
        [InlineData(0, 0, -1, 1, EdgeSide.West, false)]
        [InlineData(0, 0, -1, 1, EdgeSide.North, false)]

        [InlineData(5, 8, 4, 8, EdgeSide.East, true)]
        [InlineData(5, 8, 4, 8, EdgeSide.North, true)]
        [InlineData(5, 8, 4, 9, EdgeSide.East, true)]
        [InlineData(5, 8, 5, 8, EdgeSide.North, true)]

        [InlineData(5, 8, 5, 8, EdgeSide.East, false)]
        [InlineData(5, 8, 5, 8, EdgeSide.West, false)]
        [InlineData(5, 8, 4, 8, EdgeSide.West, false)]
        [InlineData(5, 8, 4, 9, EdgeSide.West, false)]
        [InlineData(5, 8, 4, 9, EdgeSide.North, false)]
        public void TestEdgeAdjacentCaseWest(int edgeX, int edgeY, int inputX, int inputY, EdgeSide inputSide, bool expected)
        {
            var input = new Mock<IEdge>();
            input.Setup(t => t.X).Returns(inputX);
            input.Setup(t => t.Y).Returns(inputY);
            input.Setup(t => t.Side).Returns(inputSide);

            var edge = new Edge(edgeX, edgeY, EdgeSide.West);
            bool result = edge.IsAdjacentTo(input.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, West); Edge(X: {inputX}, Y: {inputY}, {inputSide})");
        }

        //(x,y,W) (x, y, E), (x-1, y+1, E), (x+1, y, W)
        [Theory]
        [InlineData(0, 0, 0, 0, EdgeSide.West, true)]
        [InlineData(0, 0, 0, 0, EdgeSide.East, true)]
        [InlineData(0, 0, -1, 1, EdgeSide.East, true)]
        [InlineData(0, 0, 1, 0, EdgeSide.West, true)]

        [InlineData(0, 0, 0, 0, EdgeSide.North, false)]
        [InlineData(0, 0, -1, 1, EdgeSide.North, false)]
        [InlineData(0, 0, -1, 1, EdgeSide.West, false)]
        [InlineData(0, 0, 1, 0, EdgeSide.North, false)]
        [InlineData(0, 0, 1, 0, EdgeSide.East, false)]

        [InlineData(5, 8, 5, 8, EdgeSide.West, true)]
        [InlineData(5, 8, 5, 8, EdgeSide.East, true)]
        [InlineData(5, 8, 4, 9, EdgeSide.East, true)]
        [InlineData(5, 8, 6, 8, EdgeSide.West, true)]

        [InlineData(5, 8, 5, 8, EdgeSide.North, false)]
        [InlineData(5, 8, 4, 9, EdgeSide.North, false)]
        [InlineData(5, 8, 4, 9, EdgeSide.West, false)]
        [InlineData(5, 8, 6, 8, EdgeSide.North, false)]
        [InlineData(5, 8, 6, 8, EdgeSide.East, false)]
        public void TestEdgeAdjacentCaseNorth(int edgeX, int edgeY, int inputX, int inputY, EdgeSide inputSide, bool expected)
        {
            var input = new Mock<IEdge>();
            input.Setup(t => t.X).Returns(inputX);
            input.Setup(t => t.Y).Returns(inputY);
            input.Setup(t => t.Side).Returns(inputSide);

            var edge = new Edge(edgeX, edgeY, EdgeSide.North);
            bool result = edge.IsAdjacentTo(input.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, North); Edge(X: {inputX}, Y: {inputY}, {inputSide})");
        }

        //(x+1, y-1, W) (x+1, y-1, N) (x+1, y, W) (x, y, N)
        [Theory]
        [InlineData(0, 0, 1, -1, EdgeSide.West, true)]
        [InlineData(0, 0, 1, -1, EdgeSide.North, true)]
        [InlineData(0, 0, 1, 0, EdgeSide.West, true)]
        [InlineData(0, 0, 0, 0, EdgeSide.North, true)]

        [InlineData(0, 0, 1, -1, EdgeSide.East, false)]
        [InlineData(0, 0, 1, 0, EdgeSide.East, false)]
        [InlineData(0, 0, 1, 0, EdgeSide.North, false)]
        [InlineData(0, 0, 0, 0, EdgeSide.East, false)]
        [InlineData(0, 0, 0, 0, EdgeSide.West, false)]

        [InlineData(5, 8, 6, 7, EdgeSide.West, true)]
        [InlineData(5, 8, 6, 7, EdgeSide.North, true)]
        [InlineData(5, 8, 6, 8, EdgeSide.West, true)]
        [InlineData(5, 8, 5, 8, EdgeSide.North, true)]

        [InlineData(5, 8, 6, 7, EdgeSide.East, false)]
        [InlineData(5, 8, 6, 8, EdgeSide.East, false)]
        [InlineData(5, 8, 6, 8, EdgeSide.North, false)]
        [InlineData(5, 8, 5, 8, EdgeSide.East, false)]
        [InlineData(5, 8, 5, 8, EdgeSide.West, false)]
        public void TestEdgeAdjacentCaseEast(int edgeX, int edgeY, int inputX, int inputY, EdgeSide inputSide, bool expected)
        {
            var input = new Mock<IEdge>();
            input.Setup(t => t.X).Returns(inputX);
            input.Setup(t => t.Y).Returns(inputY);
            input.Setup(t => t.Side).Returns(inputSide);

            var edge = new Edge(edgeX, edgeY, EdgeSide.East);
            bool result = edge.IsAdjacentTo(input.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, East); Edge(X: {inputX}, Y: {inputY}, {inputSide})");
        }

        [Theory]
        [InlineData(0, 0, 0, 0, VertexTileSide.Left, true)]
        [InlineData(0, 0, -1, 1, VertexTileSide.Right, true)]
        [InlineData(0, 0, -1, 1, VertexTileSide.Left, false)]
        [InlineData(0, 0, 0, 0, VertexTileSide.Right, false)]
        [InlineData(0, 0, 1, -1, VertexTileSide.Right, false)]
        [InlineData(0, 0, 1, -1, VertexTileSide.Left, false)]

        [InlineData(6, 9, 6, 9, VertexTileSide.Left, true)]
        [InlineData(6, 9, 5, 10, VertexTileSide.Right, true)]
        [InlineData(6, 9, 6, 9, VertexTileSide.Right, false)]
        [InlineData(6, 9, 5, 10, VertexTileSide.Left, false)]
        [InlineData(6, 9, 7, 8, VertexTileSide.Left, false)]
        [InlineData(6, 9, 7, 8, VertexTileSide.Right, false)]

        public void TestVertexAdjacentCaseWest(int edgeX, int edgeY, int inputX, int inputY, VertexTileSide tileSide, bool expected)
        {
            var vertex = new Mock<IVertex>();
            vertex.Setup(t => t.X).Returns(inputX);
            vertex.Setup(t => t.Y).Returns(inputY);
            vertex.Setup(t => t.Side).Returns(tileSide);

            var edge = new Edge(edgeX, edgeY, EdgeSide.West);
            bool result = edge.IsAdjacentTo(vertex.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, West); Vertex(X: {inputX}, Y: {inputY}, {tileSide})");
        }

        [Theory]
        [InlineData(0, 0, -1, 1, VertexTileSide.Right, true)]
        [InlineData(0, 0, 1, 0, VertexTileSide.Left, true)]
        [InlineData(0, 0, -1, 1, VertexTileSide.Left, false)]
        [InlineData(0, 0, 1, 0, VertexTileSide.Right, false)]
        [InlineData(0, 0, 0, 1, VertexTileSide.Left, false)]
        [InlineData(0, 0, 1, -1, VertexTileSide.Right, false)]

        [InlineData(5, 3, 4, 4, VertexTileSide.Right, true)]
        [InlineData(5, 3, 6, 3, VertexTileSide.Left, true)]
        [InlineData(5, 3, 4, 4, VertexTileSide.Left, false)]
        [InlineData(5, 3, 6, 3, VertexTileSide.Right, false)]
        [InlineData(5, 3, 5, 4, VertexTileSide.Left, false)]
        [InlineData(5, 3, 6, 2, VertexTileSide.Right, false)]
        public void TestVertexAdjacentCaseNorth(int edgeX, int edgeY, int inputX, int inputY, VertexTileSide tileSide, bool expected)
        {
            var vertex = new Mock<IVertex>();
            vertex.Setup(t => t.X).Returns(inputX);
            vertex.Setup(t => t.Y).Returns(inputY);
            vertex.Setup(t => t.Side).Returns(tileSide);

            var edge = new Edge(edgeX, edgeY, EdgeSide.North);
            bool result = edge.IsAdjacentTo(vertex.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, North); Vertex(X: {inputX}, Y: {inputY}, {tileSide})");
        }

        [Theory]
        [InlineData(0, 0, 1, 0, VertexTileSide.Left, true)]
        [InlineData(0, 0, 0, 0, VertexTileSide.Right, true)]

        [InlineData(0, 0, 1, 0, VertexTileSide.Right, false)]
        [InlineData(0, 0, 0, 0, VertexTileSide.Left, false)]
        [InlineData(0, 0, 0, 1, VertexTileSide.Left, false)]

        [InlineData(4, 9, 5, 9, VertexTileSide.Left, true)]
        [InlineData(4, 9, 4, 9, VertexTileSide.Right, true)]

        [InlineData(4, 9, 5, 9, VertexTileSide.Right, false)]
        [InlineData(4, 9, 4, 9, VertexTileSide.Left, false)]
        [InlineData(4, 9, 5, 10, VertexTileSide.Left, false)]
        public void TestVertexAdjacetnCaseEast(int edgeX, int edgeY, int inputX, int inputY, VertexTileSide tileSide, bool expected)
        {
            var vertex = new Mock<IVertex>();
            vertex.Setup(t => t.X).Returns(inputX);
            vertex.Setup(t => t.Y).Returns(inputY);
            vertex.Setup(t => t.Side).Returns(tileSide);

            var edge = new Edge(edgeX, edgeY, EdgeSide.East);
            bool result = edge.IsAdjacentTo(vertex.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nEdge(X: {edgeX}, Y: {edgeY}, East); Vertex(X: {inputX}, Y: {inputY}, {tileSide})");
        }
    }
}

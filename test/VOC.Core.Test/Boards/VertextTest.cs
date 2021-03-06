﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using Xunit;

namespace VOC.Core.Test.Boards
{
    public class VertextTest
    {
        [Fact]
        public void IsAdjacentToNullTile()
        {
            var vertex = new Vertex(1, 3, VertexTileSide.Left);
            Assert.False(vertex.IsAdjacentTo((ITile)null));
        }


        [Theory]
        [InlineData(0, 0, VertexTileSide.Left)]
        [InlineData(0, 0, VertexTileSide.Right)]
        [InlineData(3, 3, VertexTileSide.Left)]
        [InlineData(3, 3, VertexTileSide.Right)]
        [InlineData(10, 3, VertexTileSide.Left)]
        [InlineData(3, 5, VertexTileSide.Right)]
        [InlineData(-1, 100, VertexTileSide.Right)]
        public void IsAdjacentToTileWithSameXY(int x, int y, VertexTileSide side)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(x);
            tile.Setup(t => t.Y).Returns(y);

            var vertex = new Vertex(x, y, side);
            Assert.True(vertex.IsAdjacentTo(tile.Object));
        }

        /// <summary>
        /// The board is build from bottom to top
        /// so we expect (x+1,y) and (x+1, y-1) to be true if vertexTileSide is Right
        /// </summary>
        [Theory]
        [InlineData(0, 0, 1, 0, true)]
        [InlineData(0, 0, 1, -1, true)]
        [InlineData(0, 0, 1, 1, false)]
        [InlineData(0, 0, 0, 1, false)]
        [InlineData(0, 0, 0, -1, false)]
        [InlineData(0, 0, -1, -1, false)]
        [InlineData(0, 0, 1, -2, false)]

        [InlineData(3, 5, 4, 5, true)]
        [InlineData(3, 5, 4, 4, true)]
        [InlineData(3, 5, 4, 3, false)]
        [InlineData(3, 5, 4, 6, false)]
        [InlineData(3, 5, 5, 4, false)]
        [InlineData(3, 5, 2, 5, false)]
        [InlineData(3, 5, 2, 4, false)]

        [InlineData(-3, -6, -2, -6, true)]
        [InlineData(-3, -6, -2, -7, true)]
        public void IsAdjacentToTilesRightIfTileSideRight(int vertexX, int vertexY, int tileX, int tileY, bool expected)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(tileX);
            tile.Setup(t => t.Y).Returns(tileY);

            var vertex = new Vertex(vertexX, vertexY, VertexTileSide.Right);

            bool result = vertex.IsAdjacentTo(tile.Object);
            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nVertex(X: {vertexX}, Y: {vertexY}, Right); Tile(X: {tileX}, Y: {tileY}");
        }


        /// <summary>
        /// The board is build from bottom to top
        /// so we expect (x-1,y) and (x-1, y-1) to be true if vertexTileSide is Left
        /// </summary>
        [Theory]
        [InlineData(0, 0, -1, 0, true)]
        [InlineData(0, 0, -1, -1, true)]
        [InlineData(0, 0, 0, -1, false)]
        [InlineData(0, 0, -1, 1, false)]
        [InlineData(0, 0, -1, -2, false)]

        [InlineData(3, 5, 2, 5, true)]
        [InlineData(3, 5, 2, 4, true)]
        [InlineData(3, 5, 4, 5, false)]
        [InlineData(3, 5, 4, 4, false)]

        [InlineData(-3, -6, -4, -6, true)]
        [InlineData(-3, -6, -4, -7, true)]
        [InlineData(-3, -6, -2, -6, false)]
        [InlineData(-3, -6, -2, -7, false)]
        public void IsAdjacentToTilesLeftIfTileSideLeft(int vertexX, int vertexY, int tileX, int tileY, bool expected)
        {
            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(tileX);
            tile.Setup(t => t.Y).Returns(tileY);

            var vertex = new Vertex(vertexX, vertexY, VertexTileSide.Left);
            bool result = vertex.IsAdjacentTo(tile.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nVertext(X: {vertexX}, Y: {vertexY}, Left); Tile(X: {tileX}, Y: {tileY}");
        }

        [Fact]
        public void IsAdjacentToNullEdge()
        {
            var vertex = new Vertex(1, 3, VertexTileSide.Left);
            Assert.False(vertex.IsAdjacentTo((IEdge)null));
        }

        /// <summary>
        /// (x,y,L) → (x,y,W) (x-1,y,E) (x-1,y,N)
        /// </summary>
        [Theory]
        [InlineData(0, 0, 0, 0, EdgeSide.West, true)]
        [InlineData(0, 0, -1, 0, EdgeSide.East, true)]
        [InlineData(0, 0, -1, 0, EdgeSide.North, true)]

        [InlineData(6, 10, 6, 10, EdgeSide.West, true)]
        [InlineData(6, 10, 5, 10, EdgeSide.East, true)]
        [InlineData(6, 10, 5, 10, EdgeSide.North, true)]

        [InlineData(-5, -40, -5, -40, EdgeSide.West, true)]
        [InlineData(-5, -40, -6, -40, EdgeSide.East, true)]
        [InlineData(-5, -40, -6, -40, EdgeSide.North, true)]

        [InlineData(6, 10, 5, 10, EdgeSide.West, false)]
        [InlineData(6, 10, 6, 10, EdgeSide.East, false)]
        [InlineData(6, 10, 6, 10, EdgeSide.North, false)]
        [InlineData(-5, -40, -5, -41, EdgeSide.West, false)]
        [InlineData(-5, -40, -6, -41, EdgeSide.East, false)]
        [InlineData(-5, -40, -6, -41, EdgeSide.North, false)]
        [InlineData(-5, -40, -5, -39, EdgeSide.West, false)]
        [InlineData(-5, -40, -6, -39, EdgeSide.East, false)]
        [InlineData(-5, -40, -6, -39, EdgeSide.North, false)]
        public void IsAdjacentToEdgeCaseLeft(int vertexX, int vertexY, int edgeX, int edgeY, EdgeSide side, bool expected)
        {
            var edge = new Mock<IEdge>();
            edge.Setup(e => e.X).Returns(edgeX);
            edge.Setup(e => e.Y).Returns(edgeY);
            edge.Setup(e => e.Side).Returns(side);

            var vertex = new Vertex(vertexX, vertexY, VertexTileSide.Left);

            bool result = vertex.IsAdjacentTo(edge.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nVertext(X: {vertexX}, Y: {vertexY}, Left); Edge(X: {edgeX}, Y: {edgeY}, Side: {side}");
        }

        /// <summary>
        /// (x,y,R) → (x+1,y-1,N) (x+1,y-1,W) (x,y,E)
        /// </summary>
        [Theory]
        [InlineData(0, 0, 1, -1, EdgeSide.West, true)]
        [InlineData(0, 0, 0, 0, EdgeSide.East, true)]
        [InlineData(0, 0, 1, -1, EdgeSide.North, true)]

        [InlineData(6, 10, 7, 9, EdgeSide.West, true)]
        [InlineData(6, 10, 6, 10, EdgeSide.East, true)]
        [InlineData(6, 10, 7, 9, EdgeSide.North, true)]

        [InlineData(-5, -40, -4, -41, EdgeSide.West, true)]
        [InlineData(-5, -40, -5, -40, EdgeSide.East, true)]
        [InlineData(-5, -40, -4, -41, EdgeSide.North, true)]

        [InlineData(6, 10, 6, 10, EdgeSide.West, false)]
        [InlineData(6, 10, 7, 9, EdgeSide.East, false)]
        [InlineData(6, 10, 6, 10, EdgeSide.North, false)]

        [InlineData(6, 10, 6, 9, EdgeSide.West, false)]
        [InlineData(6, 10, 5, 10, EdgeSide.East, false)]
        [InlineData(6, 10, 6, 9, EdgeSide.North, false)]

        [InlineData(6, 10, 7, 10, EdgeSide.West, false)]
        [InlineData(6, 10, 6, 11, EdgeSide.East, false)]
        [InlineData(6, 10, 7, 10, EdgeSide.North, false)]
        public void IsAdjacentTOEdgeCaseRight(int vertexX, int vertexY, int edgeX, int edgeY, EdgeSide side, bool expected)
        {
            var edge = new Mock<IEdge>();
            edge.Setup(e => e.X).Returns(edgeX);
            edge.Setup(e => e.Y).Returns(edgeY);
            edge.Setup(e => e.Side).Returns(side);

            var vertex = new Vertex(vertexX, vertexY, VertexTileSide.Right);

            bool result = vertex.IsAdjacentTo(edge.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nVertext(X: {vertexX}, Y: {vertexY}, Right); Edge(X: {edgeX}, Y: {edgeY}, Side: {side}");

        }

        [Fact]
        public void IsAdjacentToVertexNull()
        {
            var vertex = new Vertex(1, 2, VertexTileSide.Left);
            Assert.False(vertex.IsAdjacentTo((IVertex)null));
        }

        /// <summary>
        /// (x,y,L) → (x-1,y+1,R) (x-1,y,R) (x-2,y+1,R) 
        /// </summary>
        [Theory]
        [InlineData(0, 0, -1, 1, VertexTileSide.Right, true)]
        [InlineData(0, 0, -1, 0, VertexTileSide.Right, true)]
        [InlineData(0, 0, -2, 1, VertexTileSide.Right, true)]
        [InlineData(0, 0, -1, 1, VertexTileSide.Left, false)]
        [InlineData(0, 0, -1, 0, VertexTileSide.Left, false)]
        [InlineData(0, 0, -2, 1, VertexTileSide.Left, false)]

        [InlineData(3, 5, 2, 6, VertexTileSide.Right, true)]
        [InlineData(3, 5, 2, 5, VertexTileSide.Right, true)]
        [InlineData(3, 5, 1, 6, VertexTileSide.Right, true)]
        [InlineData(3, 5, 1, 5, VertexTileSide.Right, false)]
        [InlineData(3, 5, 3, 6, VertexTileSide.Right, false)]
        [InlineData(3, 5, 2, 7, VertexTileSide.Right, false)]
        [InlineData(3, 5, 2, 4, VertexTileSide.Right, false)]
        public void IsAdjacentToVertexCaseLeft(int vertexX, int vertexY, int inputX, int inputY, VertexTileSide inputSide, bool expected)
        {
            var input = new Mock<IVertex>();
            input.Setup(v => v.X).Returns(inputX);
            input.Setup(v => v.Y).Returns(inputY);
            input.Setup(v => v.Side).Returns(inputSide);

            var vertex = new Vertex(vertexX, vertexY, VertexTileSide.Left);
            bool result = vertex.IsAdjacentTo(input.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nVertext(X: {vertexX}, Y: {vertexY}, Left); Vertex(X: {inputX}, Y: {inputY}, Side: {inputSide}");
        }

        /// <summary>
        /// (x,y,R) → (x+1,y-1,L) (x+1,y,L) (x+2,y-1,L)
        /// </summary>
        [Theory]
        [InlineData(0, 0, 1, -1, VertexTileSide.Left, true)]
        [InlineData(0, 0, 1, 0, VertexTileSide.Left, true)]
        [InlineData(0, 0, 2, -1, VertexTileSide.Left, true)]
        [InlineData(0, 0, 1, -1, VertexTileSide.Right, false)]
        [InlineData(0, 0, 1, 0, VertexTileSide.Right, false)]
        [InlineData(0, 0, 2, -1, VertexTileSide.Right, false)]

        [InlineData(3, 5, 4, 4, VertexTileSide.Left, true)]
        [InlineData(3, 5, 4, 5, VertexTileSide.Left, true)]
        [InlineData(3, 5, 5, 4, VertexTileSide.Left, true)]
        [InlineData(3, 5, 5, 5, VertexTileSide.Left, false)]
        [InlineData(3, 5, 3, 5, VertexTileSide.Left, false)]
        [InlineData(3, 5, 5, 3, VertexTileSide.Left, false)]
        [InlineData(3, 5, 3, 3, VertexTileSide.Left, false)]
        public void IsAdjacentToVertexCaseRight(int vertexX, int vertexY, int inputX, int inputY, VertexTileSide inputSide, bool expected)
        {
            var input = new Mock<IVertex>();
            input.Setup(v => v.X).Returns(inputX);
            input.Setup(v => v.Y).Returns(inputY);
            input.Setup(v => v.Side).Returns(inputSide);

            var vertex = new Vertex(vertexX, vertexY, VertexTileSide.Right);
            bool result = vertex.IsAdjacentTo(input.Object);

            Assert.True(result == expected, $"Result: {result}, Expected: {expected}\nVertext(X: {vertexX}, Y: {vertexY}, Right); Vertex(X: {inputX}, Y: {inputY}, Side: {inputSide}");
        }
    }
}

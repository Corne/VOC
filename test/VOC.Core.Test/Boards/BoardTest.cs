using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Boards
{
    public class BoardTest
    {
        //Use the default board as test data
        private static readonly IBoardBuilder builder = new DefaultBoardBuilder();

        [Fact]
        public void BoardWillBeCreatedInConstructor()
        {
            var builder = new Mock<IBoardBuilder>();
            var board = new Board(builder.Object);

            builder.Verify(b => b.Build());
        }

        [Fact]
        public void GetTilesTest()
        {
            var board = new Board(builder);
            var tiles = board.GetTiles(4);

            Assert.Equal(2, tiles.Count());
            Assert.Equal(new[] { MaterialType.Lumber, MaterialType.Brick }, tiles.Select(t => t.Rawmaterial));
        }

        [Fact]
        public void GetTilesReturnsEmptyListIfNoMatchingTiles()
        {
            var board = new Board(builder);
            var tiles = board.GetTiles(13);

            Assert.Equal(new ITile[0], tiles);
        }


        [Fact]
        public void BuildEstablismentNullVertexException()
        {
            var player = new Mock<IPlayer>();
            var board = new Board(builder);

            Assert.Throws<ArgumentNullException>(() => board.BuildEstablisment(null, player.Object));
        }


        [Fact]
        public void BuildEstablismentNonExisitingVertexException()
        {
            var player = new Mock<IPlayer>();
            //mock can never be on the board
            var vertex = new Mock<IVertex>();
            var board = new Board(builder);

            Assert.Throws<ArgumentException>(() => board.BuildEstablisment(vertex.Object, player.Object));
        }

        [Fact]
        public void BuildEstablismentNullPlayerExcpetion()
        {
            var board = new Board(builder);
            var vertex = builder.Vertices.First(t => t.X == 0 && t.Y == 0);

            Assert.Throws<ArgumentNullException>(() => board.BuildEstablisment(vertex, null));
        }

        [Fact]
        public void BuildEstablismentTest()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            var vertex = builder.Vertices.First(t => t.X == 1 && t.Y == 1);

            var result = board.BuildEstablisment(vertex, player.Object);
            Assert.Contains(result, board.Establisments);
            Assert.Equal(player.Object, result.Owner);
            Assert.Equal(vertex, result.Vertex);
        }

        [Fact]
        public void BuidFailsIfAlreadyEstablismentOnVertex()
        {
            var board = new Board(builder);
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();
            var vertex = builder.Vertices.First(t => t.X == 1 && t.Y == 1);

            board.BuildEstablisment(vertex, player1.Object);

            Assert.Throws<ArgumentException>(() => board.BuildEstablisment(vertex, player2.Object));
        }

        [Fact]
        public void BuildFailsIfTryingToBuildOnVertexSurroundedBySea()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();

            //get first tile where all adjacent tiles are sea
            var seaVertex = board.Vertices
                .First(v => builder.Tiles.Where(t => t.IsAdjacentTo(v))
                    .All(t => t.Rawmaterial == MaterialType.Sea));

            Assert.Throws<ArgumentException>(() => board.BuildEstablisment(seaVertex, player.Object));
        }

        [Fact]
        public void GetEstablismentNullExcpetion()
        {
            var board = new Board(builder);
            Assert.Throws<ArgumentNullException>(() => board.GetEstablishments(null));
        }

        [Fact]
        public void GetEstablishmentsWillReturnForAllAdjacentTiles()
        {
            var board = new Board(builder);

            var vertex = board.Vertices.Single(v => v.X == -1 && v.Y == 1 && v.Side == VertexTileSide.Right);
            var player = new Mock<IPlayer>();

            //add an establisment
            var establisment = board.BuildEstablisment(vertex, player.Object);

            var tile1 = new Mock<ITile>();
            tile1.Setup(t => t.X).Returns(0);
            tile1.Setup(t => t.Y).Returns(0);

            var tile2 = new Mock<ITile>();
            tile2.Setup(t => t.X).Returns(0);
            tile2.Setup(t => t.Y).Returns(1);

            var tile3 = new Mock<ITile>();
            tile3.Setup(t => t.X).Returns(-1);
            tile3.Setup(t => t.Y).Returns(1);
            //validate we get it back, if we ask for it from all aligining tiles
            var result1 = board.GetEstablishments(tile1.Object);
            var result2 = board.GetEstablishments(tile2.Object);
            var result3 = board.GetEstablishments(tile3.Object);

            Assert.Contains(establisment, result1);
            Assert.Contains(establisment, result2);
            Assert.Contains(establisment, result3);
        }

        [Fact]
        public void GetEstablismentsWillReturnAllEstablismentOnATile()
        {
            var board = new Board(builder);

            var vertex1 = board.Vertices.Single(v => v.X == -1 && v.Y == 1 && v.Side == VertexTileSide.Right);
            var vertex2 = board.Vertices.Single(v => v.X == -1 && v.Y == 1 && v.Side == VertexTileSide.Left);
            var player = new Mock<IPlayer>();

            //add an establisment
            var establisment1 = board.BuildEstablisment(vertex1, player.Object);
            var establisment2 = board.BuildEstablisment(vertex2, player.Object);

            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(-1);
            tile.Setup(t => t.Y).Returns(1);

            var result = board.GetEstablishments(tile.Object);
            Assert.Equal(new[] { establisment1, establisment2 }, result);
        }
    }
}

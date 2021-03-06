﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Establishments;
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
        public void BoardCantBeConstructedWithoutBuilder()
        {
            Assert.Throws<ArgumentNullException>(() => new Board(null));
        }

        [Fact]
        public void BoardWillBeCreatedInConstructor()
        {
            var board = new Board(builder);

            Assert.NotEmpty(board.Tiles);
            Assert.NotEmpty(board.Vertices);
            Assert.NotEmpty(board.Edges);
            Assert.NotEmpty(board.Harbors);
            Assert.NotNull(board.Robber);
        }

        [Fact]
        public void GetTilesTest()
        {
            var board = new Board(builder);
            var tiles = board.GetResourceTiles(4);

            Assert.Equal(2, tiles.Count());
            Assert.Equal(new[] { MaterialType.Lumber, MaterialType.Brick }, tiles.Select(t => t.Rawmaterial));
        }

        [Fact]
        public void GetTilesIgnoresRobberTiles()
        {
            var board = new Board(builder);
            board.Robber.Move(board.Tiles.Single(t => t.X == 0 && t.Y == 0));
            var tiles = board.GetResourceTiles(11);

            Assert.Equal(1, tiles.Count());
            Assert.Equal(new[] { MaterialType.Lumber }, tiles.Select(t => t.Rawmaterial));
        }

        [Fact]
        public void GetTilesReturnsEmptyListIfNoMatchingTiles()
        {
            var board = new Board(builder);
            var tiles = board.GetResourceTiles(13);

            Assert.Equal(new ITile[0], tiles);
        }


        [Fact]
        public void BuildEstablismentNullVertexException()
        {
            var player = new Mock<IPlayer>();
            var board = new Board(builder);

            Assert.Throws<ArgumentNullException>(() => board.BuildEstablishment(null, player.Object));
        }


        [Fact]
        public void BuildEstablismentNonExisitingVertexException()
        {
            var player = new Mock<IPlayer>();

            //mock can never be on the board
            var vertex = new Mock<IVertex>();
            var board = new Board(builder);

            Assert.Throws<ArgumentException>(() => board.BuildEstablishment(vertex.Object, player.Object));
        }

        [Fact]
        public void BuildEstablismentNullPlayerExcpetion()
        {
            var board = new Board(builder);
            var vertex = builder.Vertices.First(t => t.X == 0 && t.Y == 0);

            Assert.Throws<ArgumentNullException>(() => board.BuildEstablishment(vertex, null));
        }



        [Fact]
        public void BuildEstablismentTest()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var vertex = builder.Vertices.First(t => t.X == 1 && t.Y == 1);
            var result = board.BuildEstablishment(vertex, player.Object);
            Assert.Contains(result, board.Establishments);
            Assert.Equal(player.Object, result.Owner);
            Assert.Equal(vertex, result.Vertex);
        }



        [Fact]
        public void BuildEstablishmentFailsIfAlreadyEstablismentOnVertex()
        {
            var board = new Board(builder);
            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var vertex = builder.Vertices.First(t => t.X == 1 && t.Y == 1);

            board.BuildEstablishment(vertex, player1.Object);

            Assert.Throws<ArgumentException>(() => board.BuildEstablishment(vertex, player2.Object));
        }

        [Fact]
        public void BuildEstablishmentFailsIfAnAdjacentTileHasEstablishment()
        {
            var board = new Board(builder);

            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var vertex1 = builder.Vertices.First(t => t.X == 1 && t.Y == 1);
            var vertex2 = builder.Vertices.First(t => t.IsAdjacentTo(vertex1));
            board.BuildEstablishment(vertex1, player1.Object);

            Assert.Throws<ArgumentException>(() => board.BuildEstablishment(vertex2, player2.Object));
        }

        [Fact]
        public void BuildEstablishmentFailsIfTryingToBuildOnVertexSurroundedBySea()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            //get first tile where all adjacent tiles are sea
            var seaVertex = board.Vertices
                .First(v => builder.Tiles.Where(t => t.IsAdjacentTo(v))
                    .All(t => t.Rawmaterial == MaterialType.Sea));

            Assert.Throws<ArgumentException>(() => board.BuildEstablishment(seaVertex, player.Object));
        }

        [Fact]
        public void CanOnlyBuild2EstablishmentsWithoutAdjacentRoad()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var vertex1 = builder.Vertices.First(t => t.X == 1 && t.Y == 1);
            var vertex2 = builder.Vertices.First(t => t.X == -2 && t.Y == 1);
            var vertex3 = builder.Vertices.First(t => t.X == 3 && t.Y == -1);
            board.BuildEstablishment(vertex1, player.Object);
            board.BuildEstablishment(vertex2, player.Object);
            Assert.Throws<InvalidOperationException>(() => board.BuildEstablishment(vertex3, player.Object));
        }


        [Fact]
        public void BuildRoadNullEdgeException()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();

            Assert.Throws<ArgumentNullException>(() => board.BuildRoad(null, player.Object));
        }

        [Fact]
        public void BuildRoadNullOwnerException()
        {
            var board = new Board(builder);
            var edge = board.Edges.First(e => e.X == 0 && e.Y == 0);

            Assert.Throws<ArgumentNullException>(() => board.BuildRoad(edge, null));
        }

        [Fact]
        public void BuildRoadFailsIfNoAdjacentEdgesOrEstablismentsOfThePlayer()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            var edge = board.Edges.First(e => e.X == 0 && e.Y == 0 && e.Side == EdgeSide.West);

            Assert.Throws<ArgumentException>(() => board.BuildRoad(edge, player.Object));
        }



        [Fact]
        public void BuildRoadSuccesIfAdjcanentToEstablisment()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var edge = board.Edges.First(e => e.X == 0 && e.Y == 0 && e.Side == EdgeSide.West);
            var vertex = board.Vertices.First(v => v.X == 0 && v.Y == 0 && v.Side == VertexTileSide.Left);

            board.BuildEstablishment(vertex, player.Object);
            var road = board.BuildRoad(edge, player.Object);

            Assert.Contains(road, board.Roads);
            Assert.NotNull(road);
            Assert.Equal(edge, road.Edge);
            Assert.Equal(player.Object, road.Owner);
        }



        [Fact]
        public void BuildRoadSuccesIfAdjcentToDifferentPlayerRoad()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);


            var edge1 = board.Edges.First(e => e.X == 0 && e.Y == 0 && e.Side == EdgeSide.West);
            var edge2 = board.Edges.First(e => e.X == -1 && e.Y == 1 && e.Side == EdgeSide.East);
            var vertex = board.Vertices.First(v => v.X == 0 && v.Y == 0 && v.Side == VertexTileSide.Left);

            board.BuildEstablishment(vertex, player.Object);
            board.BuildRoad(edge1, player.Object);

            var road = board.BuildRoad(edge2, player.Object);

            Assert.Contains(road, board.Roads);
            Assert.NotNull(road);
            Assert.Equal(edge2, road.Edge);
            Assert.Equal(player.Object, road.Owner);
        }

        [Fact]
        public void BuildRoadFailsIfAdjcanentEstablismentFromDifferentPlayer()
        {
            var board = new Board(builder);
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var edge = board.Edges.First(e => e.X == 0 && e.Y == 0 && e.Side == EdgeSide.West);
            var vertex = board.Vertices.First(v => v.X == 0 && v.Y == 0 && v.Side == VertexTileSide.Left);

            board.BuildEstablishment(vertex, player2.Object);
            Assert.Throws<ArgumentException>(() => board.BuildRoad(edge, player1.Object));
        }

        [Fact]
        public void BuildRoadFailIfAlreadyRoadOnThatEdge()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var edge = board.Edges.First(e => e.X == 0 && e.Y == 0 && e.Side == EdgeSide.West);
            var vertex = board.Vertices.First(v => v.X == 0 && v.Y == 0 && v.Side == VertexTileSide.Left);

            board.BuildEstablishment(vertex, player.Object);
            board.BuildRoad(edge, player.Object);
            Assert.Throws<ArgumentException>(() => board.BuildRoad(edge, player.Object));
        }

        [Fact]
        public void BuildRoadFailIfEdgeIsBetweenSeaTiles()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);


            var vertex = board.Vertices.First(v =>
            {
                var tiles = board.Tiles.Where(t => t.IsAdjacentTo(v));
                return tiles.Count() == 3 && tiles.Count(t => t.Rawmaterial == MaterialType.Sea) == 2;
            });
            var edge = board.Edges.First(e => board.Tiles.Where(t => t.IsAdjacentTo(e)).All(t => t.Rawmaterial == MaterialType.Sea));

            board.BuildEstablishment(vertex, player.Object);
            Assert.Throws<ArgumentException>(() => board.BuildRoad(edge, player.Object));
        }

        [Fact]
        public void BuildRoadFailIfEdgeNotOnBoard()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var edge = new Mock<IEdge>();
            edge.Setup(e => e.X).Returns(0);
            edge.Setup(e => e.Y).Returns(0);
            edge.Setup(e => e.Side).Returns(EdgeSide.West);
            var vertex = board.Vertices.First(v => v.X == 0 && v.Y == 0 && v.Side == VertexTileSide.Left);

            board.BuildEstablishment(vertex, player.Object);
            Assert.Throws<ArgumentException>(() => board.BuildRoad(edge.Object, player.Object));
        }

        [Fact]
        public void GetEstablismentNullExcpetion()
        {
            var board = new Board(builder);
            Assert.Throws<ArgumentNullException>(() => board.GetEstablishments((ITile)null));
        }

        [Fact]
        public void GetEstablishmentsWillReturnForAllAdjacentTiles()
        {
            var board = new Board(builder);

            var vertex = board.Vertices.Single(v => v.X == -1 && v.Y == 1 && v.Side == VertexTileSide.Right);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            //add an establisment
            var establisment = board.BuildEstablishment(vertex, player.Object);

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
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            //add an establisment
            var establisment1 = board.BuildEstablishment(vertex1, player.Object);
            var establisment2 = board.BuildEstablishment(vertex2, player.Object);

            var tile = new Mock<ITile>();
            tile.Setup(t => t.X).Returns(-1);
            tile.Setup(t => t.Y).Returns(1);

            var result = board.GetEstablishments(tile.Object);
            Assert.Equal(new[] { establisment1, establisment2 }, result);
        }

        [Fact]
        public void CantGetHarborsForNullPlayer()
        {
            var board = new Board(builder);
            Assert.Throws<ArgumentNullException>(() => board.GetHarbors(null));
        }

        [Fact]
        public void ExpectEmptyArrayIfNoEstablismentAdjacentToHarbor()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            var result = board.GetHarbors(player.Object);

            Assert.Equal(new IHarbor[] { }, result);
        }

        [Fact]
        public void ExpectSingleIf1EstablismentAdjacentToHarbor()
        {
            var board = new Board(builder);
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            IHarbor harbor = board.Harbors.First();
            IVertex vertex = board.Vertices.First(v => v.IsAdjacentTo(harbor.Edge));
            board.BuildEstablishment(vertex, player.Object);
            IEnumerable<IHarbor> result = board.GetHarbors(player.Object);

            Assert.Equal(new IHarbor[] { harbor }, result);
        }

        [Fact]
        public void ExpectNoResultIfEstablismentIsFromDifferentPlayer()
        {
            var board = new Board(builder);
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            IHarbor harbor = board.Harbors.First();
            IVertex vertex = board.Vertices.First(v => v.IsAdjacentTo(harbor.Edge));
            board.BuildEstablishment(vertex, player1.Object);

            IEnumerable<IHarbor> result = board.GetHarbors(player2.Object);

            Assert.Equal(new IHarbor[] { }, result);
        }

        [Fact]
        public void ExpectAllHarborsToReturnIfAdjacent()
        {
            var board = new Board(builder);
            var result = new List<IHarbor>();

            foreach (var harbor in board.Harbors)
            {
                var player = new Mock<IPlayer>();
                player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);
                IVertex vertex = board.Vertices.First(v => v.IsAdjacentTo(harbor.Edge));
                board.BuildEstablishment(vertex, player.Object);
                result.AddRange(board.GetHarbors(player.Object));
            }

            Assert.Equal(board.Harbors, result);
        }

        [Fact]
        public void GetPlayersExceptionIfTileNull()
        {
            var board = new Board(builder);
            Assert.Throws<ArgumentNullException>(() => board.GetPlayers(null));
        }

        [Fact]
        public void GetPlayersTest()
        {
            var board = new Board(builder);
            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var tile = builder.Tiles.First(t => t.X == 1 && t.Y == 1);
            var vertex1 = board.Vertices.First(v => v.IsAdjacentTo(tile) && v.Side == VertexTileSide.Left && v.X == 1 && v.Y == 1);
            var vertex2 = board.Vertices.First(v => v.IsAdjacentTo(tile) && v.Side == VertexTileSide.Right && v.X == 1 && v.Y == 1);
            board.BuildEstablishment(vertex1, player1.Object);
            board.BuildEstablishment(vertex2, player2.Object);

            var players = board.GetPlayers(tile);

            Assert.Equal(new[] { player1.Object, player2.Object }, players);
        }

        [Fact]
        public void GetPlayersGivesNoDuplicates()
        {
            var board = new Board(builder);
            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var tile = builder.Tiles.First(t => t.X == 1 && t.Y == 1);
            var vertex1 = board.Vertices.First(v => v.IsAdjacentTo(tile) && v.Side == VertexTileSide.Left && v.X == 1 && v.Y == 1);
            var vertex2 = board.Vertices.First(v => v.IsAdjacentTo(tile) && v.Side == VertexTileSide.Right && v.X == 1 && v.Y == 1);
            board.BuildEstablishment(vertex1, player1.Object);
            board.BuildEstablishment(vertex2, player1.Object);

            var players = board.GetPlayers(tile);

            Assert.Equal(new[] { player1.Object }, players);
        }

        [Fact]
        public void LongestRoadFailsIfPlayerNull()
        {
            var board = new Board(builder);
            Assert.Throws<ArgumentNullException>(() => board.GetLongestRoad(null));
        }

        public class EdgeCoordinate
        {
            public EdgeCoordinate(int x, int y, EdgeSide side)
            {
                X = x;
                Y = y;
                Side = side;
            }

            public int X { get; }
            public int Y { get; }
            public EdgeSide Side { get; }
        }

        public static IEnumerable<object> LongestRoadCases
        {
            get
            {
                yield return new object[] { new EdgeCoordinate[0], 0 };
                yield return new object[] { new[] { new EdgeCoordinate(0, 0, EdgeSide.West) }, 1 };
                yield return new object[] { new[] {
                    new EdgeCoordinate(0, 0, EdgeSide.West),
                    new EdgeCoordinate(-1, 0, EdgeSide.North),
                }, 2 };
                yield return new object[] { new[] {
                    new EdgeCoordinate(0, 0, EdgeSide.West),
                    new EdgeCoordinate(-1, 0, EdgeSide.North),
                    new EdgeCoordinate(1, 0, EdgeSide.East), //this road should not be counted, because it's not connected
                }, 2 };
                yield return new object[] { new[] {
                    new EdgeCoordinate(1, 0, EdgeSide.East), //this road should not be counted, because it's not connected
                    new EdgeCoordinate(0, 0, EdgeSide.West),
                    new EdgeCoordinate(-1, 0, EdgeSide.North),
                }, 2 };
                yield return new object[] { new[] {
                    new EdgeCoordinate(0, 0, EdgeSide.West),
                    new EdgeCoordinate(-1, 0, EdgeSide.North),
                    new EdgeCoordinate(1, 0, EdgeSide.East), //this road should not be counted, because it's not connected
                    new EdgeCoordinate(-1, 0, EdgeSide.West),
                    new EdgeCoordinate(-2, 0, EdgeSide.East),
                }, 4 };
                yield return new object[] { new[] {
                    //1 of those 3 should not be counted because of branches
                    new EdgeCoordinate(0, 0, EdgeSide.West),
                    new EdgeCoordinate(-1, 0, EdgeSide.North),
                    new EdgeCoordinate(-1, 0, EdgeSide.East),
                }, 2 };
                yield return new object[] { new[] {
                    new EdgeCoordinate(0, 0, EdgeSide.West),
                    new EdgeCoordinate(-1, 0, EdgeSide.North),
                    new EdgeCoordinate(-1, 0, EdgeSide.East),
                    new EdgeCoordinate(0, -1, EdgeSide.West),
                    new EdgeCoordinate(-2, 1, EdgeSide.East),
                    new EdgeCoordinate(-1, -1, EdgeSide.East),
                    new EdgeCoordinate(0, 0, EdgeSide.North),
                    new EdgeCoordinate(0, -2, EdgeSide.North),
                }, 6 };
            }
        }


        [Theory, MemberData(nameof(LongestRoadCases))]
        public void GetLongestRoadTest(IEnumerable<EdgeCoordinate> coordinates, int expectedResult)
        {
            var board = new Board(builder);
            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);
            //Carefull with setup, all roads should be connected to those 2 establishments
            var vertex1 = builder.Vertices.First(t => t.X == 0 && t.Y == 0 && t.Side == VertexTileSide.Left);
            var vertex2 = builder.Vertices.First(t => t.X == 2 && t.Y == 0 && t.Side == VertexTileSide.Left);
            board.BuildEstablishment(vertex1, player1.Object);
            board.BuildEstablishment(vertex2, player1.Object);

            foreach (var coordinate in coordinates)
            {
                var edge = board.FindEdge(coordinate.X, coordinate.Y, coordinate.Side);
                board.BuildRoad(edge, player1.Object);
            }

            var result = board.GetLongestRoad(player1.Object);
            Assert.Equal(expectedResult, result.Count());
        }
    }
}

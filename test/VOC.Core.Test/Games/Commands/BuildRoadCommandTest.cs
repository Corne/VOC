using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Games.Commands;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class BuildRoadCommandTest
    {
        [Fact]
        public void CantBeCreatedWithoutPlayer()
        {
            var board = new Mock<IBoard>();
            var edge = new Mock<IEdge>();
            Assert.Throws<ArgumentNullException>(() => new BuildRoadCommand(null, board.Object, edge.Object));
        }

        [Fact]
        public void CantBeCreatedWithoutBoard()
        {
            var player = new Mock<IPlayer>();
            var edge = new Mock<IEdge>();
            Assert.Throws<ArgumentNullException>(() => new BuildRoadCommand(player.Object, null, edge.Object));
        }

        [Fact]
        public void CantBeCreatedWithoutEdge()
        {
            var player = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            Assert.Throws<ArgumentNullException>(() => new BuildRoadCommand(player.Object, board.Object, null));
        }

        [Fact]
        public void BuildRoadRemovesResourcesFromPlayerIfSucceeded()
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Road.BUILD_RESOURCES)).Returns(true);
            var board = new Mock<IBoard>();
            var edge = new Mock<IEdge>();

            var command = new BuildRoadCommand(player.Object, board.Object, edge.Object);
            command.Execute();

            board.Verify(b => b.BuildRoad(edge.Object, player.Object));
            player.Verify(p => p.TakeResources(Road.BUILD_RESOURCES));
        }

        [Fact]
        public void BuildRoadFailsIfPlayerHasNotEnoughResources()
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.HasResources(Road.BUILD_RESOURCES)).Returns(false);
            var board = new Mock<IBoard>();
            var edge = new Mock<IEdge>();

            var command = new BuildRoadCommand(player.Object, board.Object, edge.Object);
            
            Assert.Throws<InvalidOperationException>(() => command.Execute());
            board.Verify(b => b.BuildRoad(edge.Object, player.Object), Times.Never);
            player.Verify(p => p.TakeResources(Road.BUILD_RESOURCES), Times.Never);
        }
    }
}

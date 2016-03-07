using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Establishments;
using VOC.Core.Games.Commands;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class BuildEstablishmentCommandTest
    {
        [Fact]
        public void CantBeCreatedWithoutPlayer()
        {
            var board = new Mock<IBoard>();
            var vertex = new Mock<IVertex>();
            Assert.Throws<ArgumentNullException>(() => new BuildEstablishmentCommand(null, board.Object, vertex.Object));

        }

        [Fact]
        public void CantBeCreatedWithoutBoard()
        {
            var player = new Mock<IPlayer>();
            var vertex = new Mock<IVertex>();

            Assert.Throws<ArgumentNullException>(() => new BuildEstablishmentCommand(player.Object, null, vertex.Object));
        }

        [Fact]
        public void CantBeCreatedWithoutVertex()
        {
            var player = new Mock<IPlayer>();
            var board = new Mock<IBoard>();

            Assert.Throws<ArgumentNullException>(() => new BuildEstablishmentCommand(player.Object, board.Object, null));
        }

        [Fact]
        public void ExecuteFailsIfPlayerNotEnoughtResources()
        {
            var player = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            var vertex = new Mock<IVertex>();

            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(false);

            var command = new BuildEstablishmentCommand(player.Object, board.Object, vertex.Object);
            Assert.Throws<InvalidOperationException>(() => command.Execute());

            board.Verify(b => b.BuildEstablishment(vertex.Object, player.Object), Times.Never);
            player.Verify(p => p.TakeResources(Establishment.BUILD_RESOURCES), Times.Never);
        }

        [Fact]
        public void ExecuteTest()
        {
            var player = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            var vertex = new Mock<IVertex>();

            player.Setup(p => p.HasResources(Establishment.BUILD_RESOURCES)).Returns(true);

            var command = new BuildEstablishmentCommand(player.Object, board.Object, vertex.Object);
            command.Execute();

            board.Verify(b => b.BuildEstablishment(vertex.Object, player.Object));
            player.Verify(p => p.TakeResources(Establishment.BUILD_RESOURCES));
        }
    }
}

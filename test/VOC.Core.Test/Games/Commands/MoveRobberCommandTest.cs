using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Games.Commands;
using VOC.Core.Items;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class MoveRobberCommandTest
    {
        [Fact]
        public void CommandCantBeCreatedWithoutPlayer()
        {
            var robber = new Mock<IRobber>();
            var tile = new Mock<ITile>();
            Assert.Throws<ArgumentNullException>(() => new MoveRobberCommand(null, robber.Object, tile.Object));
        }

        [Fact]
        public void CommandCantBeCreatedWithoutRobber()
        {
            var player = new Mock<IPlayer>();
            var tile = new Mock<ITile>();
            Assert.Throws<ArgumentNullException>(() => new MoveRobberCommand(player.Object, null, tile.Object));

        }

        [Fact]
        public void CommandCantBeCreatedWithoutTile()
        {
            var player = new Mock<IPlayer>();
            var robber = new Mock<IRobber>();
            Assert.Throws<ArgumentNullException>(() => new MoveRobberCommand(player.Object, robber.Object, null));
        }

        [Fact]
        public void ExecuteTest()
        {
            var player = new Mock<IPlayer>();
            var robber = new Mock<IRobber>();
            var tile = new Mock<ITile>();

            var command = new MoveRobberCommand(player.Object, robber.Object, tile.Object);
            command.Execute();

            robber.Verify(r => r.Move(tile.Object));
        }
    }
}

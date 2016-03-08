using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Establishments;
using VOC.Core.Games.Commands;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class UpgradeEstablishmentCommandTest
    {
        [Fact]
        public void CommandCantBeCreatedWithoutPlayer()
        {
            var establishment = new Mock<IEstablishment>();
            Assert.Throws<ArgumentNullException>(() => new UpgradeEstablishmentCommand(null, establishment.Object));
        }

        [Fact]
        public void CommandCantBeCreatedWithoutEstablisment()
        {
            var player = new Mock<IPlayer>();
            Assert.Throws<ArgumentNullException>(() => new UpgradeEstablishmentCommand(player.Object, null));
        }

        [Fact]
        public void ExecuteFailsIfPlayerNotEnoughResources()
        {
            var player = new Mock<IPlayer>();
            var establishment = new Mock<IEstablishment>();
            player.Setup(p => p.HasResources(Establishment.UPGRADE_RESOURCES)).Returns(false);

            var command = new UpgradeEstablishmentCommand(player.Object, establishment.Object);
            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void ExecuteTest()
        {
            var player = new Mock<IPlayer>();
            var establishment = new Mock<IEstablishment>();
            player.Setup(p => p.HasResources(Establishment.UPGRADE_RESOURCES)).Returns(true);

            var command = new UpgradeEstablishmentCommand(player.Object, establishment.Object);
            command.Execute();

            establishment.Verify(e => e.Upgrade(), Times.Once);
            player.Verify(p => p.TakeResources(Establishment.UPGRADE_RESOURCES), Times.Once);
        }
    }
}

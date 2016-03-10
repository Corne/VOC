using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Commands;
using VOC.Core.Games.Turns;
using VOC.Core.Items.Cards;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class PlayDevelopmentCardCommandTest
    {
        [Fact]
        public void CommandCantBeCreatedWithoutPlayer()
        {
            var turn = new Mock<IGameTurn>();
            var card = new Mock<IDevelopmentCard>();
            Assert.Throws<ArgumentNullException>(() => new PlayDevelopmentCardCommand(null, turn.Object, card.Object));
        }

        [Fact]
        public void CommandCantBeCreatedWithoutTurn()
        {
            var player = new Mock<IPlayer>();
            var card = new Mock<IDevelopmentCard>();
            Assert.Throws<ArgumentNullException>(() => new PlayDevelopmentCardCommand(player.Object, null, card.Object));
        }

        [Fact]
        public void CommandCantBeCreatedWithoutCard()
        {
            var player = new Mock<IPlayer>();
            var turn = new Mock<IGameTurn>();
            Assert.Throws<ArgumentNullException>(() => new PlayDevelopmentCardCommand(player.Object, turn.Object, null));
        }

        [Fact]
        public void ExecuteFailsIfCardNotPlayable()
        {
            var player = new Mock<IPlayer>();
            var turn = new Mock<IGameTurn>();
            var card = new Mock<IDevelopmentCard>();
            card.Setup(c => c.Playable).Returns(false);

            var command = new PlayDevelopmentCardCommand(player.Object, turn.Object, card.Object);
            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void ExecuteTest()
        {
            var player = new Mock<IPlayer>();
            var turn = new Mock<IGameTurn>();
            var card = new Mock<IDevelopmentCard>();
            card.SetupAllProperties();
            card.Setup(c => c.Playable).Returns(true);

            var command = new PlayDevelopmentCardCommand(player.Object, turn.Object, card.Object);
            command.Execute();

            turn.Verify(t => t.PlayDevelopmentCard(card.Object));
            Assert.True(card.Object.Played);
        }
    }
}

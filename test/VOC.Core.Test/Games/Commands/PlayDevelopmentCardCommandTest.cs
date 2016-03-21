using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games;
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
            var game = new Mock<IGame>();
            var card = new Mock<IDevelopmentCard>();
            Assert.Throws<ArgumentNullException>(() => new PlayDevelopmentCardCommand(null, game.Object, card.Object));
        }

        [Fact]
        public void CommandCantBeCreatedWithoutGame()
        {
            var player = new Mock<IPlayer>();
            var card = new Mock<IDevelopmentCard>();
            Assert.Throws<ArgumentNullException>(() => new PlayDevelopmentCardCommand(player.Object, null, card.Object));
        }

        [Fact]
        public void CommandCantBeCreatedWithoutCard()
        {
            var player = new Mock<IPlayer>();
            var game = new Mock<IGame>();
            Assert.Throws<ArgumentNullException>(() => new PlayDevelopmentCardCommand(player.Object, game.Object, null));
        }

        [Fact]
        public void ExecuteFailsIfCardNotPlayable()
        {
            var player = new Mock<IPlayer>();
            var game = new Mock<IGame>();
            var card = new Mock<IDevelopmentCard>();
            card.Setup(c => c.Playable).Returns(false);

            var command = new PlayDevelopmentCardCommand(player.Object, game.Object, card.Object);
            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Fact]
        public void ExecuteTest()
        {
            var player = new Mock<IPlayer>();
            var game = new Mock<IGame>();
            var card = new Mock<IDevelopmentCard>();
            card.SetupAllProperties();
            card.Setup(c => c.Playable).Returns(true);

            var command = new PlayDevelopmentCardCommand(player.Object, game.Object, card.Object);
            command.Execute();

            game.Verify(t => t.PlayDevelopmentCard(card.Object));
            Assert.True(card.Object.Played);
        }
    }
}

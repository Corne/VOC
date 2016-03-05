using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games;
using VOC.Core.Games.Commands;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games
{
    public class GameTest
    {
        [Fact]
        public void GameCantBeCreatedWithoutPlayers()
        {
            var provider = new Mock<ITurnProvider>();
            Assert.Throws<ArgumentNullException>(() => new Game(null, provider.Object));
        }

        private ISet<IPlayer> CreateFakePlayers(int count)
        {
            var players = Enumerable.Range(0, count).Select(i => new Mock<IPlayer>().Object);
            return new HashSet<IPlayer>(players);
        }

        [Fact]
        public void GameCantBeCreatedWithoutTurnProvider()
        {
            var players = CreateFakePlayers(2);
            Assert.Throws<ArgumentNullException>(() => new Game(players, null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void GameShouldHaveShouldHave2To4PlayersException(int playerCount)
        {
            var players = CreateFakePlayers(playerCount);
            var provider = new Mock<ITurnProvider>();
            Assert.Throws<ArgumentException>(() => new Game(players, provider.Object));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConstructionSucceedsWith2To4Players(int playerCount)
        {
            var players = CreateFakePlayers(playerCount);
            var provider = new Mock<ITurnProvider>();
            var game = new Game(players, provider.Object);

            Assert.Equal(players, game.Players);
        }

        [Fact]
        public void StartShouldCallProviderForFirstTurn()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            provider.Setup(p => p.GetNext()).Returns(new Mock<ITurn>().Object);

            var game = new Game(players, provider.Object);

            int turnSwitches = 0;
            game.TurnStarted += (sender, args) => { turnSwitches++; };

            game.Start();

            provider.Verify(p => p.GetNext());
            Assert.Equal(1, turnSwitches);
        }

        [Fact]
        public void SecondStartShouldCallShouldDoNothing()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            provider.Setup(p => p.GetNext()).Returns(new Mock<ITurn>().Object);

            var game = new Game(players, provider.Object);

            int turnSwitches = 0;
            game.TurnStarted += (sender, args) => { turnSwitches++; };

            game.Start();
            game.Start();

            provider.Verify(p => p.GetNext(), Times.Once);
            Assert.Equal(1, turnSwitches);
        }

        [Fact]
        public void GameShouldGoToNextTurnIfTurnEnds()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var game = new Game(players, provider.Object);

            int turnSwitches = 0;
            game.TurnStarted += (sender, args) => { turnSwitches++; };

            game.Start();

            turn.Raise(t => t.Ended += null, EventArgs.Empty);

            provider.Verify(p => p.GetNext(), Times.Exactly(2));
            Assert.Equal(2, turnSwitches);
        }

        [Fact]
        public void ExecuteFailsIfCommandNull()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var game = new Game(players, provider.Object);
            game.Start();

            Assert.Throws<ArgumentNullException>(() => game.Execute(null));
        }
        
        [Fact]
        public void ExecuteFailsIfPlayerNotEqualsCurrentTurnPlayer()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            turn.Setup(t => t.Player).Returns(players.First());
            turn.Setup(t => t.CanExecute(It.IsAny<GameCommand>())).Returns(true);

            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var command = new Mock<IPlayerCommand>();
            command.Setup(c => c.Player).Returns(players.Skip(1).First());

            var game = new Game(players, provider.Object);
            game.Start();

            Assert.Throws<InvalidOperationException>(() => game.Execute(command.Object));
        }

        [Fact]
        public void ExecuteFailsIfCurrentTurnCantExecuteCommand()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            var player = players.First();
            turn.Setup(t => t.Player).Returns(player);
            turn.Setup(t => t.CanExecute(It.IsAny<GameCommand>())).Returns(false);

            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var command = new Mock<IPlayerCommand>();
            command.Setup(c => c.Player).Returns(player);

            var game = new Game(players, provider.Object);
            game.Start();

            Assert.Throws<ArgumentException>(() => game.Execute(command.Object));
        }

        [Fact]
        public void ExecuteFailsIfGameNotStarted()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            turn.Setup(t => t.Player).Returns(players.First());
            turn.Setup(t => t.CanExecute(It.IsAny<GameCommand>())).Returns(true);

            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var command = new Mock<IPlayerCommand>();
            command.Setup(c => c.Player).Returns(players.First());

            var game = new Game(players, provider.Object);

            Assert.Throws<InvalidOperationException>(() => game.Execute(command.Object));
        }

        [Fact]
        public void ExecuteTest()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            turn.Setup(t => t.Player).Returns(players.First());
            turn.Setup(t => t.CanExecute(It.IsAny<GameCommand>())).Returns(true);

            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var command = new Mock<IPlayerCommand>();
            command.Setup(c => c.Player).Returns(players.First());

            var game = new Game(players, provider.Object);
            game.Start();

            game.Execute(command.Object);

            command.Verify(c => c.Execute());
            turn.Verify(t => t.AfterExecute(It.IsAny<GameCommand>()));
        }
    }
}

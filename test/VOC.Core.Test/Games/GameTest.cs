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
using VOC.Core.Items.Cards;
using VOC.Core.Players;
using VOC.Core.Trading;
using Xunit;

namespace VOC.Core.Test.Games
{
    public class GameTest
    {
        [Fact]
        public void GameCantBeCreatedWithoutPlayers()
        {
            var provider = new Mock<ITurnProvider>();
            var bank = new Mock<IBank>();
            Assert.Throws<ArgumentNullException>(() => new Game(null, provider.Object, bank.Object));
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
            var bank = new Mock<IBank>();

            Assert.Throws<ArgumentNullException>(() => new Game(players, null, bank.Object));
        }

        [Fact]
        public void GameCantBeCreatedWithoutBank()
        {
            var players = CreateFakePlayers(2);
            var provider = new Mock<ITurnProvider>();
            Assert.Throws<ArgumentNullException>(() => new Game(players, provider.Object, null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void GameShouldHaveShouldHave2To4PlayersException(int playerCount)
        {
            var players = CreateFakePlayers(playerCount);
            var provider = new Mock<ITurnProvider>();
            var bank = new Mock<IBank>();

            Assert.Throws<ArgumentException>(() => new Game(players, provider.Object, bank.Object));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ConstructionSucceedsWith2To4Players(int playerCount)
        {
            var players = CreateFakePlayers(playerCount);
            var provider = new Mock<ITurnProvider>();
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);

            Assert.Equal(players, game.Players);
        }

        [Fact]
        public void StartShouldCallProviderForFirstTurn()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            provider.Setup(p => p.GetNext()).Returns(new Mock<ITurn>().Object);

            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);

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
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);

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
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);

            int turnSwitches = 0;
            game.TurnStarted += (sender, args) => { turnSwitches++; };

            game.Start();

            turn.Raise(t => t.Ended += null, EventArgs.Empty);

            provider.Verify(p => p.GetNext(), Times.Exactly(2));
            Assert.Equal(2, turnSwitches);
        }

        [Fact]
        public void GameShouldUpdateAchievementsWhenTurnEnds()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);

            int turnSwitches = 0;
            game.TurnStarted += (sender, args) => { turnSwitches++; };

            game.Start();

            turn.Raise(t => t.Ended += null, EventArgs.Empty);

            bank.Verify(b => b.UpdateAchievements(It.IsAny<IPlayer>()));
        }

        [Fact]
        public void ExecuteFailsIfCommandNull()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
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
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            Assert.Throws<InvalidOperationException>(() => game.Execute(command.Object));
        }

        [Fact]
        public void ExecuteSucceedsWithDifferentPlayerIfCommandIsTrade()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            turn.Setup(t => t.Player).Returns(players.First());
            turn.Setup(t => t.CanExecute(It.IsAny<GameCommand>())).Returns(true);

            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var command = new Mock<IPlayerCommand>();
            command.Setup(c => c.Player).Returns(players.Skip(1).First());
            command.Setup(c => c.Type).Returns(GameCommand.Trade);
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();
            game.Execute(command.Object);

            command.Verify(c => c.Execute());
            turn.Verify(t => t.AfterExecute(It.IsAny<GameCommand>()));
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
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
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
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object); ;

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
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            game.Execute(command.Object);

            command.Verify(c => c.Execute());
            turn.Verify(t => t.AfterExecute(It.IsAny<GameCommand>()));
        }

        [Fact]
        public void CantPlayDevelopmentCardIfCurrentTurnIsNotGameTurn()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var card = new Mock<IDevelopmentCard>();
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            Assert.Throws<InvalidOperationException>((() => game.PlayDevelopmentCard(card.Object)));
        }

        [Fact]
        public void PlayDevelopmentCardCallsCurrentTurn()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<IGameTurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var card = new Mock<IDevelopmentCard>();
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            game.PlayDevelopmentCard(card.Object);

            turn.Verify(t => t.PlayDevelopmentCard(card.Object));
        }

        [Fact]
        public void BuyDevelopmentCardFailsIfPlayerNull()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<IGameTurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var card = new Mock<IDevelopmentCard>();
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            Assert.Throws<ArgumentNullException>(() => game.BuyDevelopmentCard(null));
        }

        [Fact]
        public void BuyDevelomentCardFailsIfCurrentTurnNOGameTurn()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<ITurn>();
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var card = new Mock<IDevelopmentCard>();
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            var player = new Mock<IPlayer>();

            Assert.Throws<InvalidOperationException>(() => game.BuyDevelopmentCard(player.Object));
        }

        [Fact]
        public void BuyDevelopmentCardFailsIfPlayerNotCurrentTurnPlayer()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<IGameTurn>();
            var player = new Mock<IPlayer>();

            turn.Setup(t => t.Player).Returns(new Mock<IPlayer>().Object);
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var card = new Mock<IDevelopmentCard>();
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            Assert.Throws<InvalidOperationException>(() => game.BuyDevelopmentCard(player.Object));
        }

        [Fact]
        public void BuyDevelopmentCardTest()
        {
            var players = CreateFakePlayers(3);
            var provider = new Mock<ITurnProvider>();
            var turn = new Mock<IGameTurn>();
            var player = new Mock<IPlayer>();

            turn.Setup(t => t.Player).Returns(player.Object);
            provider.Setup(p => p.GetNext()).Returns(turn.Object);

            var card = new Mock<IDevelopmentCard>();
            var bank = new Mock<IBank>();

            var game = new Game(players, provider.Object, bank.Object);
            game.Start();

            game.BuyDevelopmentCard(player.Object);

            bank.Verify(b => b.BuyDevelopmentCard(player.Object, turn.Object));
        }


    }
}

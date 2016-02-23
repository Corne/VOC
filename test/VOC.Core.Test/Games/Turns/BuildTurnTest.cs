using System;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns
{
    public class BuildTurnTest
    {
        [Fact]
        public void BuildTurnCantBeConstructedWithoutPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => new BuildTurn(null));
        }

        [Fact]
        public void BuildTurnConstructionSucceeds()
        {
            var player = new Mock<IPlayer>();
            var turn = new BuildTurn(player.Object);
            Assert.Equal(player.Object, turn.Player);
        }

        [Fact]
        public void ExpectBuildEstablismentToBeExecutable()
        {
            var player = new Mock<IPlayer>();
            var turn = new BuildTurn(player.Object);

            Assert.True(turn.CanExecute(GameCommand.BuildEstablisment));
            Assert.False(turn.CanExecute(GameCommand.BuildRoad));
        }

        [Fact]
        public void ExpectBuildRoadTeBeExecutableAfterEstablimentBuild()
        {
            var player = new Mock<IPlayer>();
            var turn = new BuildTurn(player.Object);

            turn.AfterExecute(GameCommand.BuildEstablisment);

            Assert.False(turn.CanExecute(GameCommand.BuildEstablisment));
            Assert.True(turn.CanExecute(GameCommand.BuildRoad));
        }

        [Fact]
        public void ExpectBuildTurnToEndAfterRoadAndEstablismentBuildExecuted()
        {
            var player = new Mock<IPlayer>();
            var turn = new BuildTurn(player.Object);

            bool ended = false;
            turn.Ended += (sender, args) => { ended = true; };

            turn.AfterExecute(GameCommand.BuildEstablisment);
            turn.AfterExecute(GameCommand.BuildRoad);

            Assert.True(ended);
            Assert.False(turn.CanExecute(GameCommand.BuildEstablisment));
            Assert.False(turn.CanExecute(GameCommand.BuildRoad));
        }
    }
}

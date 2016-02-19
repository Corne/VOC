using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Items;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns
{
    public class HighRollTurnTest
    {
        [Fact]
        public void HighRollTurnCantBeCreatedWithoutPlayer()
        {
            var dice = new Mock<IDice>();
            Assert.Throws<ArgumentNullException>(() => new HighRollTurn(null, dice.Object));
        }

        [Fact]
        public void HighrollTurnCantBeCreatedWithoutDice()
        {
            var player = new Mock<IPlayer>();
            Assert.Throws<ArgumentNullException>(() => new HighRollTurn(player.Object, null));
        }

        [Fact]
        public void ExpectResultToBeSetOnAfterExecute()
        {
            var player = new Mock<IPlayer>();
            var dice = new Mock<IDice>();
            dice.Setup(d => d.Current).Returns(new DiceRoll(new int[] { 3, 4 }));

            var turn = new HighRollTurn(player.Object, dice.Object);
            bool ended = false;
            turn.Ended += (sender, args) => { ended = true; };

            turn.AfterExecute(GameCommand.RollDice);

            Assert.Equal(7, turn.Result);
            Assert.True(ended);
        }

        public static IEnumerable<object> InvalidCommands
        {
            get { return Enum.GetValues(typeof(GameCommand))
                    .Cast<GameCommand>()
                    .Except(new GameCommand[] { GameCommand.RollDice })
                    .Select(x => new object[] { x }); }
        }

        [Theory, MemberData("InvalidCommands")]
        public void ExpectNothingToHappenIfCommandNotRollDice(GameCommand command)
        {
            var player = new Mock<IPlayer>();
            var dice = new Mock<IDice>();
            dice.Setup(d => d.Current).Returns(new DiceRoll(new int[] { 3, 4 }));

            var turn = new HighRollTurn(player.Object, dice.Object);
            bool ended = false;
            turn.Ended += (sender, args) => { ended = true; };

            turn.AfterExecute(command);

            Assert.Equal(0, turn.Result);
            Assert.False(ended);
        }
    }
}

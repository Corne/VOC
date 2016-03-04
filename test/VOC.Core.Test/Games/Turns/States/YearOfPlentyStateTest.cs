using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using Xunit;

namespace VOC.Core.Test.Games.Turns.States
{
    public class YearOfPlentyStateTest
    {
        [Fact]
        public void CantBeCreatedWithoutTurn()
        {
            Assert.Throws<ArgumentNullException>(() => new YearOfPlentyState(null));
        }

        [Fact]
        public void ExpectNextFlowStateAfterYearOfPlentyExecuted()
        {
            var turn = new Mock<IGameTurn>();
            var state = new YearOfPlentyState(turn.Object);

            state.AfterExecute(GameCommand.YearOfPlenty);
            turn.Verify(t => t.NextFlowState(), Times.Once);
        }

        public static IEnumerable<object> UnusedStateCommands
        {
            get
            {
                return Enum.GetValues(typeof(GameCommand))
                  .Cast<GameCommand>()
                  .Except(new[] { GameCommand.YearOfPlenty })
                  .Select(x => new object[] { x });
            }
        }

        [Theory, MemberData(nameof(UnusedStateCommands))]
        public void ExpectNothingToHappenIfCommandNotYearofPlenty(GameCommand command)
        {
            var turn = new Mock<IGameTurn>();
            var state = new YearOfPlentyState(turn.Object);

            state.AfterExecute(command);
            turn.Verify(t => t.NextFlowState(), Times.Never);
        }
    }
}

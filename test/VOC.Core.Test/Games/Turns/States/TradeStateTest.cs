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
    public class TradeStateTest
    {
        [Fact]
        public void TradeStateCantBeConstructedWithoutTurn()
        {
            Assert.Throws<ArgumentNullException>(() => new TradeState(null));
        }

        [Fact]
        public void ExpectNextFlowStateIfNExtStateCommnad()
        {
            var turn = new Mock<ITurn>();
            var state = new TradeState(turn.Object);

            state.AfterExecute(StateCommand.NextState);

            Assert.True(state.Completed);
            turn.Verify(t => t.NextFlowState());
        }

        public static IEnumerable<object> UnusedStateCommands
        {
            get
            {
                return Enum.GetValues(typeof(StateCommand))
                  .Cast<StateCommand>()
                  .Except(new[] { StateCommand.NextState })
                  .Select(x => new object[] { x });
            }
        }

        [Theory, MemberData(nameof(UnusedStateCommands))]
        public void ExpectNothingToHappenIfCommandNotNextState(StateCommand command)
        {
            var turn = new Mock<ITurn>();
            var state = new TradeState(turn.Object);

            state.AfterExecute(command);

            turn.Verify(t => t.NextFlowState(), Times.Never);
            Assert.False(state.Completed);
        }
    }
}

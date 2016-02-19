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
    public class BuildStateTest
    {
        [Fact]
        public void StateCantBeCreatedWithoutTurn()
        {
            Assert.Throws<ArgumentNullException>(() => new BuildState(null));
        }

        [Fact]
        public void ExpectNextFlowStateAfterNextStateCommand()
        {
            var turn = new Mock<IGameTurn>();
            var state = new BuildState(turn.Object);

            state.AfterExecute(GameCommand.NextState);

            turn.Verify(t => t.NextFlowState());
            Assert.True(state.Completed);
        }

        public static IEnumerable<object> UnusedStateCommands
        {
            get
            {
                return Enum.GetValues(typeof(GameCommand))
                  .Cast<GameCommand>()
                  .Except(new[] { GameCommand.NextState })
                  .Select(x => new object[] { x });
            }
        }

        [Theory, MemberData(nameof(UnusedStateCommands))]
        public void ExpectNothingToHappenIfCommandNotNextState(GameCommand command)
        {
            var turn = new Mock<IGameTurn>();
            var state = new BuildState(turn.Object);

            state.AfterExecute(command);

            turn.Verify(t => t.NextFlowState(), Times.Never);
            Assert.False(state.Completed);
        }
    }
}

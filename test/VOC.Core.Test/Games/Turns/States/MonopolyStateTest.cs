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
    public class MonopolyStateTest
    {
        [Fact]
        public void MonopolyStateCantBeCreatedWithoutGameTurn()
        {
            Assert.Throws<ArgumentNullException>(() => new MonopolyState(null));
        }

        [Fact]
        public void ExpectNextFlowStateAfterMonoplyExecuted()
        {
            var turn = new Mock<IGameTurn>();
            var state = new MonopolyState(turn.Object);

            state.AfterExecute(GameCommand.Monopoly);
            turn.Verify(t => t.NextFlowState(), Times.Once);
        }

        public static IEnumerable<object> UnusedStateCommands
        {
            get
            {
                return Enum.GetValues(typeof(GameCommand))
                  .Cast<GameCommand>()
                  .Except(new[] { GameCommand.Monopoly })
                  .Select(x => new object[] { x });
            }
        }

        [Theory, MemberData(nameof(UnusedStateCommands))]
        public void ExpectNothingToHappenIfCommandNotMonopoly(GameCommand command)
        {
            var turn = new Mock<IGameTurn>();
            var state = new MonopolyState(turn.Object);

            state.AfterExecute(command);
            turn.Verify(t => t.NextFlowState(), Times.Never);
        }
    }
}

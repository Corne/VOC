using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using Xunit;

namespace VOC.Core.Test.Games.Turns.States
{
    public class MoveRobberStateTest
    {
        [Fact]
        public void RobberStateCantBeCreatedWithoutRobber()
        {
            var turn = new Mock<IGameTurn>();
            Assert.Throws<ArgumentNullException>(() => new MoveRobberState(turn.Object, null));
        }

        [Fact]
        public void RobberStateCantBeCreatedWithoutTurn()
        {
            var robber = new Mock<IRobber>();
            Assert.Throws<ArgumentNullException>(() => new MoveRobberState(null, robber.Object)); 
        }

        private IRobber CreateRobber()
        {
            var robber = new Mock<IRobber>();
            return robber.Object;
        }

        [Fact]
        public void AfterRobberMoveTransitionToRobberStealingState()
        {
            var turn = new Mock<IGameTurn>();
            var robber = CreateRobber();

            var state = new MoveRobberState(turn.Object, robber);
            state.AfterExecute(GameCommand.MoveRobber);

            turn.Verify(t => t.SetState<RobberStealState>(), Times.Once);
        }

        public static IEnumerable<object> UnusedStateCommands
        {
            get
            {
                return Enum.GetValues(typeof(GameCommand))
                  .Cast<GameCommand>()
                  .Except(new[] { GameCommand.MoveRobber })
                  .Select(x => new object[] { x });
            }
        }

        [Theory, MemberData(nameof(UnusedStateCommands))]
        public void ExpectNothingToHappenIfCommandNotMoveRobber(GameCommand command)
        {
            var turn = new Mock<IGameTurn>();
            var robber = CreateRobber();

            var state = new MoveRobberState(turn.Object, robber);
            state.AfterExecute(command);

            turn.Verify(t => t.SetState<RobberStealState>(), Times.Never);
        }
    }
}

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
            var turn = new Mock<ITurn>();
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
            robber.Setup(r => r.Move(It.IsAny<ITile>())).Raises(r => r.Moved += null, null, new Mock<ITile>().Object);
            return robber.Object;
        }

        [Fact]
        public void AfterRobberMoveTransitionToRobberStealingState()
        {
            var turn = new Mock<ITurn>();
            var robber = CreateRobber();

            var state = new MoveRobberState(turn.Object, robber);
            state.Start();

            robber.Move(new Mock<ITile>().Object);

            turn.Verify(t => t.SetState<RobberStealState>(), Times.Once);
        }

        [Fact]
        public void StateNotStartedShouldNotSetState()
        {
            var turn = new Mock<ITurn>();
            var robber = CreateRobber();

            var state = new MoveRobberState(turn.Object, robber);

            robber.Move(new Mock<ITile>().Object);

            turn.Verify(t => t.SetState<RobberStealState>(), Times.Never);
        }

        [Fact]
        public void StoppedStateShouldNotSetState()
        {
            var turn = new Mock<ITurn>();
            var robber = CreateRobber();

            var state = new MoveRobberState(turn.Object, robber);
            state.Start();
            state.Stop();

            robber.Move(new Mock<ITile>().Object);

            turn.Verify(t => t.SetState<RobberStealState>(), Times.Never);
        }

        [Fact]
        public void TwiceStartedShouldStillCallSetStateOnce()
        {
            var turn = new Mock<ITurn>();
            var robber = CreateRobber();

            var state = new MoveRobberState(turn.Object, robber);
            state.Start();
            state.Start();

            robber.Move(new Mock<ITile>().Object);

            turn.Verify(t => t.SetState<RobberStealState>(), Times.Once);
        }

        [Fact]
        public void MoveTwiceShouldStillCallSetStateOnce()
        {
            var turn = new Mock<ITurn>();
            var robber = CreateRobber();

            var state = new MoveRobberState(turn.Object, robber);
            state.Start();

            robber.Move(new Mock<ITile>().Object);
            robber.Move(new Mock<ITile>().Object);

            turn.Verify(t => t.SetState<RobberStealState>(), Times.Once);
        }
    }
}

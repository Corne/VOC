using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns
{
    public class TurnTest
    {
        [Fact]
        public void CantCreateTurnWithoutPlayer()
        {
            var provider = new Mock<IStateProvider>();
            Assert.Throws<ArgumentNullException>(() => new Turn(null, provider.Object));
        }

        [Fact]
        public void TurnCantBeCreatedWithoutProvider()
        {
            var player = new Mock<IPlayer>();
            Assert.Throws<ArgumentNullException>(() => new Turn(player.Object, null));
        }

        [Fact]
        public void CreateTurnTest()
        {
            var player = new Mock<IPlayer>();
            var provider = new Mock<IStateProvider>();

            var turn = new Turn(player.Object, provider.Object);
            Assert.Equal(player.Object, turn.Player);
        }


        [Fact]
        public void StartSetsFirstFlowState()
        {
            var player = new Mock<IPlayer>();
            var state = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.GetNext()).Returns(state.Object);

            var turn = new Turn(player.Object, provider.Object);

            ITurnState result = null;
            int triggerCount = 0;
            turn.StateChanged += (sender, arg) => { result = arg; triggerCount++; };
            turn.Start();

            provider.Verify(p => p.GetNext(), Times.Once);
            Assert.Equal(state.Object, result);
            Assert.Equal(1, triggerCount);
        }

        [Fact]
        public void CantStartAnAlreadyStartedTurn()
        {
            var player = new Mock<IPlayer>();
            var state = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.GetNext()).Returns(state.Object);

            var turn = new Turn(player.Object, provider.Object);
            turn.Start();

            Assert.Throws<InvalidOperationException>(() => turn.Start());
        }


        [Theory]
        [InlineData(StateCommand.BuildEstablisment)]
        [InlineData(StateCommand.BuildRoad)]
        [InlineData(StateCommand.DiscardResources)]
        [InlineData(StateCommand.MoveRobber)]
        public void ExpectCanExecuteAlwaysFalseIfIfTurnNotStarted(StateCommand command)
        {
            var player = new Mock<IPlayer>();
            var provider = new Mock<IStateProvider>();

            var turn = new Turn(player.Object, provider.Object);
            Assert.False(turn.CanExecute(command));
        }

        [Theory]
        [InlineData(StateCommand.BuildEstablisment, new StateCommand[] { }, false)]
        [InlineData(StateCommand.BuildEstablisment, new StateCommand[] { StateCommand.BuildEstablisment }, true)]
        [InlineData(StateCommand.RollDice, new StateCommand[] { StateCommand.RollDice }, true)]
        [InlineData(StateCommand.RollDice, new StateCommand[] { StateCommand.MoveRobber }, false)]
        [InlineData(StateCommand.PlayDevelopmentCard, new StateCommand[] { StateCommand.MoveRobber, StateCommand.PlayDevelopmentCard, StateCommand.StealResource }, true)]
        [InlineData(StateCommand.PlayDevelopmentCard, new StateCommand[] { StateCommand.MoveRobber, StateCommand.UpdgradeEstablisment, StateCommand.StealResource }, false)]
        public void CanExecuteIfCurrentStateHasCommand(StateCommand command, IEnumerable<StateCommand> stateCommands, bool expected)
        {
            var player = new Mock<IPlayer>();
            var state = new Mock<ITurnState>();
            state.Setup(s => s.Commands).Returns(stateCommands);
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.GetNext()).Returns(state.Object);

            var turn = new Turn(player.Object, provider.Object);
            turn.Start();

            bool result = turn.CanExecute(command);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExpectExceptionOnNextFlowStateIfStateNotStarted()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var state2 = new Mock<ITurnState>();
            var stateQueue = new Queue<ITurnState>(new[] { state1.Object, state2.Object });
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.GetNext()).Returns(stateQueue.Dequeue());

            var turn = new Turn(player.Object, provider.Object);
            Assert.Throws<InvalidOperationException>(() => turn.NextFlowState());

        }

        [Fact]
        public void ExpectNextFlowStateToCallStateProviderForTheNextState()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var state2 = new Mock<ITurnState>();
            var stateQueue = new Queue<ITurnState>(new[] { state1.Object, state2.Object });
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.GetNext()).Returns(() => stateQueue.Dequeue());

            var turn = new Turn(player.Object, provider.Object);
            turn.Start();

            bool stateChanged = false;
            turn.StateChanged += (sender, args) =>
            {
                stateChanged = true;
                Assert.Equal(state2.Object, args);
            };
            turn.NextFlowState();

            Assert.True(stateChanged);
            provider.Verify(p => p.GetNext(), Times.Exactly(2));
        }
    }
}

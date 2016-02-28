using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Items.Cards;
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

        [Theory]
        [InlineData(GameCommand.BuildEstablisment)]
        [InlineData(GameCommand.BuildRoad)]
        [InlineData(GameCommand.DiscardResources)]
        [InlineData(GameCommand.MoveRobber)]
        public void ExpectCanExecuteAlwaysFalseIfIfTurnNotStarted(GameCommand command)
        {
            var player = new Mock<IPlayer>();
            var provider = new Mock<IStateProvider>();

            var turn = new Turn(player.Object, provider.Object);
            Assert.False(turn.CanExecute(command));
        }

        [Theory]
        [InlineData(GameCommand.BuildEstablisment, new GameCommand[] { }, false)]
        [InlineData(GameCommand.BuildEstablisment, new GameCommand[] { GameCommand.BuildEstablisment }, true)]
        [InlineData(GameCommand.RollDice, new GameCommand[] { GameCommand.RollDice }, true)]
        [InlineData(GameCommand.RollDice, new GameCommand[] { GameCommand.MoveRobber }, false)]
        [InlineData(GameCommand.PlayDevelopmentCard, new GameCommand[] { GameCommand.MoveRobber, GameCommand.PlayDevelopmentCard, GameCommand.StealResource }, true)]
        [InlineData(GameCommand.PlayDevelopmentCard, new GameCommand[] { GameCommand.MoveRobber, GameCommand.UpdgradeEstablisment, GameCommand.StealResource }, false)]
        public void CanExecuteIfCurrentStateHasCommand(GameCommand command, IEnumerable<GameCommand> stateCommands, bool expected)
        {
            var player = new Mock<IPlayer>();
            var state = new Mock<ITurnState>();
            state.Setup(s => s.Commands).Returns(stateCommands);
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state.Object);

            var turn = new Turn(player.Object, provider.Object);
            turn.NextFlowState();

            bool result = turn.CanExecute(command);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ExpectNextFlowStateToCallStateProviderForTheNextState()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var state2 = new Mock<ITurnState>();
            var stateQueue = new Queue<ITurnState>(new[] { state1.Object, state2.Object });
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(() => stateQueue.Dequeue());

            var turn = new Turn(player.Object, provider.Object);

            bool stateChanged = false;
            turn.StateChanged += (sender, args) =>
            {
                stateChanged = true;
                Assert.Equal(state1.Object, args);
            };
            turn.NextFlowState();

            Assert.True(stateChanged);
            provider.Verify(p => p.GetNext());
        }

        [Fact]
        public void ExpectToEndTurnifNoNextFlowState()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();

            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(false);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);

            var turn = new Turn(player.Object, provider.Object);
            bool ended = false;
            turn.Ended += (sender, args) => { ended = true; };

            turn.NextFlowState();
            provider.Verify(p => p.GetNext(), Times.Never);
            Assert.True(ended);     
        }


        [Fact]
        public void ExpectSetStateToCallStateProviderForTheNextState()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var state2 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);
            provider.Setup(p => p.Get<ITurnState>()).Returns(state2.Object);

            var turn = new Turn(player.Object, provider.Object);
            bool stateChanged = false;
            turn.StateChanged += (sender, args) =>
            {
                stateChanged = true;
                Assert.Equal(state2.Object, args);
            };
            turn.SetState<ITurnState>();

            Assert.True(stateChanged);
            provider.Verify(p => p.Get<ITurnState>());
        }

        [Fact]
        public void ExpectAfterExectueToDelegateCallToCurrentState()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);

            var turn = new Turn(player.Object, provider.Object);
            turn.NextFlowState();
            turn.AfterExecute(GameCommand.RollDice);

            state1.Verify(s => s.AfterExecute(GameCommand.RollDice));
        }

        [Fact]
        public void ExpectPlayDevelopmentCardToFailIfNoCurrentState()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);
            state1.Setup(s => s.Commands).Returns(new GameCommand[] { GameCommand.PlayDevelopmentCard });

            var card = new Mock<IDevelopmentCard>();
            card.Setup(c => c.Type).Returns(DevelopmentCardType.RoadBuilding);

            var turn = new Turn(player.Object, provider.Object);

            Assert.Throws<InvalidOperationException>(() => turn.PlayDevelopmentCard(card.Object));
        }

        [Fact]
        public void ExpectPlayDevelopmentCardToFailIfCurrentStateCantExecute()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);
            state1.Setup(s => s.Commands).Returns(new GameCommand[] { GameCommand.BuildRoad });

            var card = new Mock<IDevelopmentCard>();
            card.Setup(c => c.Type).Returns(DevelopmentCardType.RoadBuilding);

            var turn = new Turn(player.Object, provider.Object);
            turn.NextFlowState();

            Assert.Throws<InvalidOperationException>(() => turn.PlayDevelopmentCard(card.Object));
        }

        [Fact]
        public void ExpectPlayDevelopmentCardToFailIfNull()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);
            state1.Setup(s => s.Commands).Returns(new GameCommand[] { GameCommand.PlayDevelopmentCard });

            var turn = new Turn(player.Object, provider.Object);
            turn.NextFlowState();

            Assert.Throws<ArgumentNullException>(() => turn.PlayDevelopmentCard(null));
        }

        [Fact]
        public void ExpectPlayDevelopmentCardToFailIfCardIsVictoryPoint()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);
            state1.Setup(s => s.Commands).Returns(new GameCommand[] { GameCommand.PlayDevelopmentCard });

            var card = new Mock<IDevelopmentCard>();
            card.Setup(c => c.Type).Returns(DevelopmentCardType.VictoryPoint);
            var turn = new Turn(player.Object, provider.Object);
            turn.NextFlowState();

            Assert.Throws<ArgumentException>(() => turn.PlayDevelopmentCard(card.Object));
        }

        [Theory]
        [InlineData(DevelopmentCardType.Knight)]
        [InlineData(DevelopmentCardType.Monopoly)]
        [InlineData(DevelopmentCardType.RoadBuilding)]
        [InlineData(DevelopmentCardType.YearOfPlenty)]
        public void ExpectPlayDevelopmentCardToSetDevelomentCardState(DevelopmentCardType type)
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);
            state1.Setup(s => s.Commands).Returns(new GameCommand[] { GameCommand.PlayDevelopmentCard });

            var card = new Mock<IDevelopmentCard>();
            card.Setup(c => c.Type).Returns(type);
            var turn = new Turn(player.Object, provider.Object);
            turn.NextFlowState();

            bool changed = false;
            turn.StateChanged += (sender, args) => { changed = true; };

            turn.PlayDevelopmentCard(card.Object);

            provider.Verify(p => p.Get(type));
            Assert.True(changed);
        }

        [Fact]
        public void CantPlayDevelopmentMultipleDevelopmentCardsIn1Turn()
        {
            var player = new Mock<IPlayer>();
            var state1 = new Mock<ITurnState>();
            var provider = new Mock<IStateProvider>();
            provider.Setup(p => p.HasNext()).Returns(true);
            provider.Setup(p => p.GetNext()).Returns(state1.Object);
            state1.Setup(s => s.Commands).Returns(new GameCommand[] { GameCommand.PlayDevelopmentCard });

            var card = new Mock<IDevelopmentCard>();
            card.Setup(c => c.Type).Returns(DevelopmentCardType.Monopoly);
            var turn = new Turn(player.Object, provider.Object);
            turn.NextFlowState();

            turn.PlayDevelopmentCard(card.Object);
            Assert.Throws<InvalidOperationException>(() => turn.PlayDevelopmentCard(card.Object));
        }
    }
}

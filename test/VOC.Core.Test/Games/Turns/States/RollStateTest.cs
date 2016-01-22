using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns.States
{
    public class RollStateTest
    {
        [Fact]
        public void RollStateCantBeCreatedWithoutTurn()
        {
            var dice = new Mock<IDice>();
            Assert.Throws<ArgumentNullException>(() => new RollState(null, dice.Object));
        }

        [Fact]
        public void RollStateCantBeCreatedWitoutDice()
        {
            var turn = new Mock<ITurn>();
            Assert.Throws<ArgumentNullException>(() => new RollState(turn.Object, null));
        }


        private IDice CreateDice(int result)
        {
            var dice = new Mock<IDice>();
            var diceroll = new DiceRoll(new int[] { result });
            dice.Setup(d => d.Roll()).Callback(() => dice.Raise(d => d.Rolled += null, diceroll)).Returns(diceroll);
            return dice.Object;
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(8)]
        [InlineData(12)]
        public void ExpectNextFlowStateIfDiceResultNot7(int diceResult)
        {
            var dice = CreateDice(diceResult);
            var turn = new Mock<ITurn>();

            var state = new RollState(turn.Object, dice);
            state.Start();

            dice.Roll();

            turn.Verify(t => t.NextFlowState(), Times.Once);
            turn.Verify(t => t.SetState<RobberDiscardState>(), Times.Never);
        }

        [Fact]
        public void ExpectSetStateRobberIfDiceResult7()
        {
            var dice = CreateDice(7);
            var turn = new Mock<ITurn>();
            var state = new RollState(turn.Object, dice);
            state.Start();

            dice.Roll();

            turn.Verify(t => t.NextFlowState(), Times.Never);
            turn.Verify(t => t.SetState<RobberDiscardState>(), Times.Once);
        }

        [Fact]
        public void ExpectNothingToHappenIfStateNotStarted()
        {
            var dice = CreateDice(5);
            var turn = new Mock<ITurn>();

            var state = new RollState(turn.Object, dice);

            dice.Roll();

            turn.Verify(t => t.NextFlowState(), Times.Never);
            turn.Verify(t => t.SetState<RobberDiscardState>(), Times.Never);
        }

        [Fact]
        public void ExpectNothingToHappenIfStateStopped()
        {
            var dice = CreateDice(5);
            var turn = new Mock<ITurn>();

            var state = new RollState(turn.Object, dice);
            state.Start();
            state.Stop();

            dice.Roll();

            turn.Verify(t => t.NextFlowState(), Times.Never);
            turn.Verify(t => t.SetState<RobberDiscardState>(), Times.Never);
        }

        [Fact]
        public void ExpectNothingToHappenAfterDiceRollHasBeenProcessed()
        {
            var dice = CreateDice(5);
            var turn = new Mock<ITurn>();

            var state = new RollState(turn.Object, dice);
            state.Start();

            dice.Roll();
            dice.Roll(); //should trigger nothing

            turn.Verify(t => t.NextFlowState(), Times.Once);
            turn.Verify(t => t.SetState<RobberDiscardState>(), Times.Never);
        }

        [Fact]
        public void ExpectMultipleStartCallsToHaveNoDifferentEffectAs1Call()
        {
            var dice = CreateDice(5);
            var turn = new Mock<ITurn>();

            var state = new RollState(turn.Object, dice);
            state.Start();
            state.Start();

            dice.Roll();

            turn.Verify(t => t.NextFlowState(), Times.Once);
            turn.Verify(t => t.SetState<RobberDiscardState>(), Times.Never);
        }

        [Fact]
        public void ExpectRollStateToBeCompletedAfterDiceRoll()
        {
            var dice = CreateDice(5);
            var turn = new Mock<ITurn>();

            var state = new RollState(turn.Object, dice);
            state.Start();

            dice.Roll();

            Assert.True(state.Completed);
        }
    }
}

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
    public class RoadBuildingStateTest
    {
        [Fact]
        public void CantBeCreatedWithoutGameTurn()
        {
            Assert.Throws<ArgumentNullException>(() => new RoadBuildingState(null));
        }

        [Fact]
        public void ExpectNothingAfeter1RoadBuild()
        {
            var turn = new Mock<IGameTurn>();
            var state = new RoadBuildingState(turn.Object);

            state.AfterExecute(GameCommand.BuildRoad);

            turn.Verify(t => t.NextFlowState(), Times.Never);
        }

        [Fact]
        public void ExpectNextFlowStateAfterExecuteRoadBuildTwice()
        {
            var turn = new Mock<IGameTurn>();
            var state = new RoadBuildingState(turn.Object);

            state.AfterExecute(GameCommand.BuildRoad);
            state.AfterExecute(GameCommand.BuildRoad);

            turn.Verify(t => t.NextFlowState(), Times.Exactly(1));
        }

        [Theory]
        [InlineData(GameCommand.BuildEstablisment, GameCommand.BuildEstablisment)]
        [InlineData(GameCommand.BuildEstablisment, GameCommand.BuildRoad)]
        [InlineData(GameCommand.BuildRoad, GameCommand.BuildEstablisment)]
        public void ExpectNothingFromOtherCommands(GameCommand command1, GameCommand command2)
        {
            var turn = new Mock<IGameTurn>();
            var state = new RoadBuildingState(turn.Object);

            state.AfterExecute(command1);
            state.AfterExecute(command2);

            turn.Verify(t => t.NextFlowState(), Times.Never);
        }
    }
}

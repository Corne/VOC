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
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns.States
{
    public class RobberStealStateTest
    {
        private static IBoard CreateBoard()
        {
            var board = new Mock<IBoard>();
            var robber = new Mock<IRobber>();
            board.Setup(b => b.Robber).Returns(robber.Object);
            board.Setup(b => b.GetPlayers(It.IsAny<ITile>())).Returns(new[] { new Mock<IPlayer>().Object });

            return board.Object;
        }

        [Fact]
        public void StateCantBeCreatedWithoutTurn()
        {
            var board = CreateBoard();
            Assert.Throws<ArgumentNullException>(() => new RobberStealState(null, board));
        }

        [Fact]
        public void StateCantBeCreatedWithoutBoard()
        {
            var turn = new Mock<ITurn>();
            Assert.Throws<ArgumentNullException>(() => new RobberStealState(turn.Object, null));
        }

        [Fact]
        public void StateCantBeCreatedWithoutRobber()
        {
            var turn = new Mock<ITurn>();
            var board = new Mock<IBoard>();
            Assert.Throws<ArgumentNullException>(() => new RobberStealState(turn.Object, board.Object));
        }

        [Fact]
        public void ExpectNextFlowStateAfterStealExecuted()
        {
            var turn = new Mock<ITurn>();
            var board = CreateBoard();

            var state = new RobberStealState(turn.Object, board);

            turn.Verify(t => t.NextFlowState(), Times.Never); //verify not called yet
            state.AfterExecute(StateCommand.StealResource);
            turn.Verify(t => t.NextFlowState());
        }

        [Fact]
        public void ExpectNextFlowStateIfRobberIsOnTileWithoutPlayers()
        {
            var turn = new Mock<ITurn>();

            var board = new Mock<IBoard>();
            var robber = new Mock<IRobber>();
            board.Setup(b => b.Robber).Returns(robber.Object);
            board.Setup(b => b.GetPlayers(It.IsAny<ITile>())).Returns(new IPlayer[0]);

            var state = new RobberStealState(turn.Object, board.Object);
            turn.Verify(t => t.NextFlowState());
        }

        [Fact]
        public void ExpectNextFlowStateIfOnlyPlayerIsCurrentTurnPlayer()
        {
            var player = new Mock<IPlayer>();
            var turn = new Mock<ITurn>();
            turn.Setup(t => t.Player).Returns(player.Object);
            var board = new Mock<IBoard>();
            var robber = new Mock<IRobber>();
            board.Setup(b => b.Robber).Returns(robber.Object);
            board.Setup(b => b.GetPlayers(It.IsAny<ITile>())).Returns(new IPlayer[1] { player.Object });

            var state = new RobberStealState(turn.Object, board.Object);
            turn.Verify(t => t.NextFlowState());
        }

        public static IEnumerable<object> UnusedStateCommands
        {
            get
            {
                return Enum.GetValues(typeof(StateCommand))
                  .Cast<StateCommand>()
                  .Except(new[] { StateCommand.StealResource })
                  .Select(x => new object[] { x });
            }
        }

        [Theory, MemberData(nameof(UnusedStateCommands))]

        public void ExpectNothingToHappenIfNoStealCommand(StateCommand command)
        {
            var turn = new Mock<ITurn>();
            var board = CreateBoard();

            var state = new RobberStealState(turn.Object, board);

            state.AfterExecute(command);

            turn.Verify(t => t.NextFlowState(), Times.Never);
        }


    }
}

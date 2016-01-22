using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns.States
{
    public class RobberDiscardStateTest
    {
        [Fact]
        public void RobberDiscardCantBeCreatedWithoutPlayers()
        {
            var turn = new Mock<ITurn>();
            Assert.Throws<ArgumentNullException>(() => new RobberDiscardState(turn.Object, null));
            
        }

        [Fact]
        public void RobberDiscardCantBeCreatedWithoutTurn()
        {
            var players = new List<IPlayer>();
            Assert.Throws<ArgumentNullException>(() => new RobberDiscardState(null, players));
        }

        [Fact]
        public void ExpectStateTransitionIfNoPlayers()
        {
            var turn = new Mock<ITurn>();
            var players = new List<IPlayer>();
            var state = new RobberDiscardState(turn.Object, players);

            state.Start();

            turn.Verify(t => t.SetState<MoveRobberState>(), Times.Once);
        }

        [Fact]
        public void ExpectStateTransitionIfNoPlayerToMuchResources()
        {
            var turn = new Mock<ITurn>();
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();

            player1.Setup(p => p.Inventory).Returns(new IRawMaterial[] { });
            player2.Setup(p => p.Inventory).Returns(Enumerable.Range(0, 7).Select(i => new Mock<IRawMaterial>().Object));
            var players = new List<IPlayer>() { player1.Object, player2.Object };
            var state = new RobberDiscardState(turn.Object, players);

            state.Start();

            turn.Verify(t => t.SetState<MoveRobberState>(), Times.Once);
        }

        [Fact]
        public void ExpectStateTransitionAfterPlayerDiscardedTheirResources()
        {
            var turn = new Mock<ITurn>();
            var player = new Mock<IPlayer>();
            player.Setup(p => p.Inventory).Returns(Enumerable.Range(0, 8).Select(i => new Mock<IRawMaterial>().Object));
            var players = new List<IPlayer>() { player.Object };


            var state = new RobberDiscardState(turn.Object, players);
            state.Start();
            //expect 4 removed
            player.Setup(p => p.Inventory).Returns(Enumerable.Range(0, 4).Select(i => new Mock<IRawMaterial>().Object));

            
        }

        [Fact]
        public void ExpectNoStateTransitionIfNotStarted()
        {
            var turn = new Mock<ITurn>();
            var players = new List<IPlayer>();
            var state = new RobberDiscardState(turn.Object, players);

            turn.Verify(t => t.SetState<MoveRobberState>(), Times.Never);
        }

        [Fact]
        public void ExpectNoStateTransitionIfStpped()
        {
            var turn = new Mock<ITurn>();
            var players = new List<IPlayer>();
            var state = new RobberDiscardState(turn.Object, players);

            state.Start();
            state.Stop();

            turn.Verify(t => t.SetState<MoveRobberState>(), Times.Never);
        }

        [Fact]
        public void ExpectOneSetStateCallAfterMultipleStartCalls()
        {
            var turn = new Mock<ITurn>();
            var players = new List<IPlayer>();
            var state = new RobberDiscardState(turn.Object, players);

            state.Start();
            state.Start();

            turn.Verify(t => t.SetState<MoveRobberState>(), Times.Once);
        }
    }
}

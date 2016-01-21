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


    }
}

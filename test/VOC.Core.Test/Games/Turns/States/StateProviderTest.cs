using System;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Items.Cards;
using Xunit;

namespace VOC.Core.Test.Games.Turns.States
{
    public class StateProviderTest
    {
        [Fact]
        public void StateProviderCantBeCreatedWithoutFactory()
        {
            var flowstates = new IFlowSate[] { new Mock<IFlowSate>().Object, new Mock<IFlowSate>().Object, new Mock<IFlowSate>().Object };
            Assert.Throws<ArgumentNullException>(() => new StateProvider(flowstates, null));
        }

        private IStateFactory CreateFactory()
        {
            var factory = new Mock<IStateFactory>();
            factory.Setup(f => f.Create<ITurnState>())
                .Returns(new Mock<ITurnState>().Object);
            factory.Setup(f => f.Create<MoveRobberState>())
                .Returns(new MoveRobberState(new Mock<IGameTurn>().Object, new Mock<IRobber>().Object));
            factory.Setup(f => f.Create<MonopolyState>())
                .Returns(new MonopolyState());
            factory.Setup(f => f.Create<RoadBuildingState>())
                .Returns(new RoadBuildingState());
            factory.Setup(f => f.Create<YearOfPlentyState>())
                .Returns(new Mock<YearOfPlentyState>().Object);
            return factory.Object;
        }

        [Fact]
        public void StateProviderCantBeCreatedWithoutFlowStates()
        {

            Assert.Throws<ArgumentNullException>(() => new StateProvider(null, CreateFactory()));
        }

        [Fact]
        public void StateProviderCantBeCreatedWithEmptyFlowStateList()
        {
            Assert.Throws<ArgumentException>(() => new StateProvider(new IFlowSate[0], CreateFactory()));
        }

        [Fact]
        public void ExpectInitialStateToBeFirst()
        {
            var state1 = new Mock<IFlowSate>();
            var state2 = new Mock<IFlowSate>();
            var flowstates = new IFlowSate[] { state1.Object, state2.Object };

            var factory = CreateFactory();
            var provider = new StateProvider(flowstates, factory);
            var result = provider.GetNext();
            Assert.Equal(state1.Object, result);
        }


        [Fact]
        public void ExpectNextGivenStateIfCurrentCopmpleted()
        {
            var state1 = new Mock<IFlowSate>();
            var state2 = new Mock<IFlowSate>();
            var flowstates = new IFlowSate[] { state1.Object, state2.Object };

            var factory = CreateFactory();
            var provider = new StateProvider(flowstates, factory);
            state1.Setup(s => s.Completed).Returns(true);
            var result = provider.GetNext();

            Assert.Equal(state2.Object, result);
        }

        [Fact]
        public void ExpectInitialStateWhileNotCompleted()
        {
            var state1 = new Mock<IFlowSate>();
            var state2 = new Mock<IFlowSate>();
            var flowstates = new IFlowSate[] { state1.Object, state2.Object };

            var factory = CreateFactory();
            var provider = new StateProvider(flowstates, factory);
            var result = provider.GetNext();
            var result2 = provider.GetNext();
            var result3 = provider.GetNext();

            Assert.Equal(state1.Object, result);
            Assert.Equal(state1.Object, result2);
            Assert.Equal(state1.Object, result3);
        }

        [Fact]
        public void ExpectExceptionIfProviderHasNoMoreNextStates()
        {
            var state1 = new Mock<IFlowSate>();
            var state2 = new Mock<IFlowSate>();
            var flowstates = new IFlowSate[] { state1.Object, state2.Object };

            var factory = CreateFactory();
            var provider = new StateProvider(flowstates, factory);
            state1.Setup(s => s.Completed).Returns(true);
            state2.Setup(s => s.Completed).Returns(true);

            Assert.Throws<InvalidOperationException>(() => provider.GetNext());
        }

        [Theory]
        [InlineData(DevelopmentCardType.Knight, typeof(MoveRobberState))]
        [InlineData(DevelopmentCardType.Monopoly, typeof(MonopolyState))]
        [InlineData(DevelopmentCardType.RoadBuilding, typeof(RoadBuildingState))]
        [InlineData(DevelopmentCardType.YearOfPlenty, typeof(YearOfPlentyState))]
        public void TestGetDevelopmentCard(DevelopmentCardType cardType, Type expectedState)
        {
            var state1 = new Mock<IFlowSate>();
            var state2 = new Mock<IFlowSate>();
            var flowstates = new IFlowSate[] { state1.Object, state2.Object };
            var factory = CreateFactory();

            var provider = new StateProvider(flowstates, factory);
            var result = provider.Get(cardType);
            Assert.IsAssignableFrom(expectedState, result);
        }

        [Theory]
        [InlineData((DevelopmentCardType)(-1))]
        [InlineData((DevelopmentCardType)5)]
        [InlineData(DevelopmentCardType.VictoryPoint)]
        public void InvalidCardGivesException(DevelopmentCardType type)
        {
            var state1 = new Mock<IFlowSate>();
            var state2 = new Mock<IFlowSate>();
            var flowstates = new IFlowSate[] { state1.Object, state2.Object };
            var factory = CreateFactory();

            var provider = new StateProvider(flowstates, factory);
            Assert.Throws<ArgumentException>(() => provider.Get(type));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
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
    }
}

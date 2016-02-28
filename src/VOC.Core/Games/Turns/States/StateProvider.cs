using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.Cards;

namespace VOC.Core.Games.Turns.States
{
    public class StateProvider : IStateProvider
    {
        private readonly IStateFactory factory;
        private readonly Queue<IFlowSate> flowstates;

        public StateProvider(IEnumerable<IFlowSate> flowstates, IStateFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (flowstates == null)
                throw new ArgumentNullException(nameof(flowstates));
            if (!flowstates.Any())
                throw new ArgumentException("Flowstates cant be empty");
            this.factory = factory;
            this.flowstates = new Queue<IFlowSate>(flowstates);
        }

        public ITurnState Get(DevelopmentCardType card)
        {
            throw new NotImplementedException();
        }

        public ITurnState Get<T>() where T : ITurnState
        {
            return factory.Create<T>();
        }

        public ITurnState GetNext()
        {
            if (!HasNext())
                throw new InvalidOperationException("Provider has no longer");

            return flowstates.Peek();
        }

        public bool HasNext()
        {
            if (!flowstates.Any())
                return false;

            if (flowstates.Peek().Completed)
            {
                flowstates.Dequeue();
                return HasNext();
            }
            return true;
        }
    }
}

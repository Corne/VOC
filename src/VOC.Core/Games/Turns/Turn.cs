using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public class Turn : ITurn
    {
        private readonly IStateProvider stateprovider;
        private ITurnState currentState;

        public Turn(IPlayer player, IStateProvider stateprovider)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (stateprovider == null)
                throw new ArgumentNullException(nameof(stateprovider));


            Player = player;
            this.stateprovider = stateprovider;
        }

        public bool DevelopmentCardPlayed { get; private set; }
        public IPlayer Player { get; }

        public event EventHandler<ITurnState> StateChanged;

        public void NextFlowState()
        {
            throw new NotImplementedException();
        }

        public void SetState<T>() where T : ITurnState
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            if (currentState != null)
                throw new InvalidOperationException("Can't start an already started turn");

            currentState = stateprovider.GetNext();
            StateChanged?.Invoke(this, currentState);
        }

        public bool CanExecute(StateCommand command)
        {
            throw new NotImplementedException();
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        public void PlayDevelopmentCard(IDevelopmentCard card)
        {
            throw new NotImplementedException();
        }
    }
}

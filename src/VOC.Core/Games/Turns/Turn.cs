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
        private ITurnState _currentState;
        private ITurnState currentState
        {
            get { return _currentState; }
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    StateChanged?.Invoke(this, value);
                }
            }
        }

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
            if (currentState == null)
                throw new InvalidOperationException("Can't switch states if turn is not active");
            currentState = stateprovider.GetNext();
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
        }

        public bool CanExecute(StateCommand command)
        {
            if (currentState == null)
                return false;
            return currentState.Commands.Contains(command);
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

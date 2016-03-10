using System;
using System.Linq;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Items.Cards;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public class Turn : IGameTurn
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
        public event EventHandler Ended;

        public void NextFlowState()
        {
            if (stateprovider.HasNext())
                currentState = stateprovider.GetNext();
            else
                Ended?.Invoke(this, EventArgs.Empty);
        }

        public void SetState<T>() where T : ITurnState
        {
            currentState = stateprovider.Get<T>();
        }

        public bool CanExecute(GameCommand command)
        {
            if (currentState == null)
                return false;
            return currentState.Commands.Contains(command);
        }

        public void AfterExecute(GameCommand command)
        {
            currentState.AfterExecute(command);
        }

        public void PlayDevelopmentCard(IDevelopmentCard card)
        {
            //CvB todo: can't play cards that were bought in this turn (not here?)
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            if (DevelopmentCardPlayed)
                throw new InvalidOperationException("Can't play multiple development cards in 1 turn");
            if (card.Type == DevelopmentCardType.VictoryPoint)
                throw new ArgumentException("Cant play victory point cards");
            if (!CanExecute(GameCommand.PlayDevelopmentCard))
                throw new InvalidOperationException("Current turnstate can't execute a developmentcard");

            currentState = stateprovider.Get(card.Type);
            DevelopmentCardPlayed = true;
        }


    }
}

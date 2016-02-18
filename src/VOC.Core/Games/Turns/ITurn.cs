using System;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public interface ITurn
    {
        event EventHandler<ITurnState> StateChanged;
        event EventHandler Ended;
        IPlayer Player { get; }
        bool DevelopmentCardPlayed { get; }
        void PlayDevelopmentCard(IDevelopmentCard card);

        void NextFlowState();
        void SetState<T>() where T : ITurnState;

        bool CanExecute(StateCommand command);


    }
}
